using Server.Tools;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameServer.Core.Executor
{
    public class TaskInternalLock
    {
        private int _LockCount;
        private bool LogRunTime;

        public bool TryEnter()
        {
            if (Interlocked.CompareExchange(ref _LockCount, 1, 0) != 0)
            {
                LogRunTime = true;
                return false;
            }
            return true;
        }

        public bool Leave()
        {
            bool logRunTime = LogRunTime;
            Interlocked.CompareExchange(ref _LockCount, 0, 1);
            LogRunTime = false;
            return logRunTime;
        }
    }

    public interface ScheduleTask
    {
        TaskInternalLock InternalLock { get; }

        void run();
    }

    /// <summary>
    /// Task包装类,封装Task执行需要的一些参数
    /// </summary>
    internal class TaskWrapper : IComparer<TaskWrapper>
    {
        //要执行的任务
        private ScheduleTask currentTask;

        //开始执行时间（毫秒）
        private long startTime = -1;

        //执行周期间隔时间（毫秒）
        private long periodic = -1;

        //执行次数
        private int executeCount = 0;

        public bool canExecute = true;

        public TaskWrapper(ScheduleTask task, long delay, long periodic)
        {
            this.currentTask = task;
            this.startTime = TimeUtil.NOW() + delay;
            this.periodic = periodic;
        }

        public ScheduleTask CurrentTask
        {
            get { return currentTask; }
        }

        public long StartTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// 重置开始执行时间
        /// </summary>
        public void resetStartTime()
        {
            this.startTime = this.startTime + this.periodic;
        }

        public long Periodic
        {
            get { return periodic; }
        }

        public void release()
        {
            currentTask = null;
        }

        public void addExecuteCount()
        {
            executeCount++;
        }

        public int ExecuteCount
        {
            get { return this.executeCount; }
        }

        public int Compare(TaskWrapper x, TaskWrapper y)
        {
            long ret = x.startTime - y.startTime;
            if (ret == 0)
            {
                return 0;
            }
            else if (ret > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    public interface PeriodicTaskHandle
    {
        void cannel();

        bool isCanneled();

        int getExecuteCount();

        long getPeriodic();
    }

    internal class PeriodicTaskHandleImpl : PeriodicTaskHandle
    {
        private TaskWrapper taskWrapper;
        private ScheduleExecutor executor;
        private bool canneled = false;

        public PeriodicTaskHandleImpl(TaskWrapper taskWrapper, ScheduleExecutor executor)
        {
            this.taskWrapper = taskWrapper;
            this.executor = executor;
        }

        public void cannel()
        {
            if (canneled)
                return;

            if (null != executor && null != taskWrapper)
            {
                executor.removeTask(taskWrapper);
                executor = null;
            }

            canneled = true;
        }

        public bool isCanneled()
        {
            return canneled;
        }

        public int getExecuteCount()
        {
            return taskWrapper.ExecuteCount;
        }

        public long getPeriodic()
        {
            return taskWrapper.Periodic;
        }
    }

    /// <summary>
    /// Task执行器
    /// </summary>
    internal class Worker
    {
        private ScheduleExecutor executor = null;

        private Thread currentThread = null;
        private static int nThreadCount = 0;
        private int nThreadOrder = 0;

        // 对象锁
        private static Object _lock = new Object();

        public Worker(ScheduleExecutor executor)
        {
            this.executor = executor;
        }

        public Thread CurrentThread
        {
            set { this.currentThread = value; }
        }

        private TaskWrapper getCanExecuteTask(long ticks)
        {
            TaskWrapper taskWrapper = executor.GetPreiodictTask(ticks);

            if (null != taskWrapper)
            {
                return taskWrapper;
            }

            int getNum = 0;

            // 任务队列上线200/线程 ChenXiaojun
            int nMaxProcCount = 200;
            int nTaskCount = executor.GetTaskCount();

            // 处理条数不能超过队列中元素个数，避免之前只要队列中有一个元素，重复删除添加1000次 ChenXiaojun
            if (nTaskCount == 0)
            {
                return null;
            }
            else if (nTaskCount < nMaxProcCount)
            {
                nMaxProcCount = nTaskCount;
            }

            while (null != (taskWrapper = executor.getTask()))
            {
                //还没开始执行的时间
                if (ticks < taskWrapper.StartTime)
                {
                    if (taskWrapper.canExecute)
                        executor.addTask(taskWrapper);
                    getNum++;

                    // 任务队列上线200/线程 ChenXiaojun
                    if (getNum >= nMaxProcCount)
                    {
                        break;
                    }
                    continue;
                }

                return taskWrapper;
            }

            return null;
        }

        public void work()
        {
            lock (_lock)
            {
                nThreadCount++;
                nThreadOrder = nThreadCount;
            }

            TaskWrapper taskWrapper = null;

            int lastTickCount = int.MinValue;
            while (!Program.NeedExitServer)
            {
                //检索可执行的任务
                int tickCount = Environment.TickCount;
                if (tickCount <= lastTickCount + 5)
                {
                    if (lastTickCount <= 0 || tickCount >= 0) //考虑当打到int最大值时的情况
                    {
                        Thread.Sleep(5);
                        continue;
                    }
                }
                lastTickCount = tickCount;

                long ticks = TimeUtil.NOW();
                while (true)
                {
                    try
                    {
                        taskWrapper = getCanExecuteTask(ticks);
                        if (null == taskWrapper || null == taskWrapper.CurrentTask)
                            break;

                        if (taskWrapper.canExecute)
                        {
                            try
                            {
                                taskWrapper.CurrentTask.run();
                            }
                            catch (System.Exception ex)
                            {
                                DataHelper.WriteFormatExceptionLog(ex, "异步调度任务执行异常", false);
                            }
                        }

                        //如果是周期执行的任务
                        if (taskWrapper.Periodic > 0 && taskWrapper.canExecute)
                        {
                            //设置下一次执行的时间
                            taskWrapper.resetStartTime();
                            executor.addTask(taskWrapper);
                            taskWrapper.addExecuteCount();
                        }
                    }
                    catch (System.Exception/* ex*/)
                    {
                        //LogManager.WriteLog(LogTypes.Error, string.Format("异步调度任务执行错误: {0}", ex));
                    }
                }
            }

            SysConOut.WriteLine(string.Format("ScheduleTask Worker{0}退出...", nThreadOrder));
        }
    }

    public class ScheduleExecutor
    {
        private List<Worker> workerQueue = null;
        private List<Thread> threadQueue = null;
        private LinkedList<TaskWrapper> TaskQueue = null;
        private List<TaskWrapper> PreiodictTaskList = new List<TaskWrapper>();
        private int maxThreadNum = 0;

        /// <summary>
        /// 设置线程池最大值
        /// </summary>
        /// <param name="maxThreadNum"></param>
        public ScheduleExecutor(int maxThreadNum)
        {
            this.maxThreadNum = maxThreadNum;
            threadQueue = new List<Thread>();
            workerQueue = new List<Worker>();
            TaskQueue = new LinkedList<TaskWrapper>();
            for (int i = 0; i < maxThreadNum; i++)
            {
                Worker worker = new Worker(this);
                Thread thread = new Thread(new ThreadStart(worker.work));
                worker.CurrentThread = thread;
                workerQueue.Add(worker);
                threadQueue.Add(thread);
            }
        }

        public void start()
        {
            lock (threadQueue)
            {
                foreach (Thread thread in threadQueue)
                {
                    thread.Start();
                }
            }
        }

        public void stop()
        {
            lock (threadQueue)
            {
                foreach (Thread thread in threadQueue)
                {
                    thread.Abort();
                }

                threadQueue.Clear();
            }

            lock (workerQueue)
            {
                workerQueue.Clear();
            }
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        public bool execute(ScheduleTask task)
        {
            TaskWrapper wrapper = new TaskWrapper(task, -1, -1);

            addTask(wrapper);

            return true;
        }

        /// <summary>
        /// 延迟异步执行任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="delay">延迟开始时间（毫秒）</param>
        /// <returns></returns>
        public PeriodicTaskHandle scheduleExecute(ScheduleTask task, long delay)
        {
            return scheduleExecute(task, delay, -1);
        }

        /// <summary>
        /// 周期性任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="delay">延迟开始时间（毫秒）</param>
        /// <param name="periodic">间隔周期时间（毫秒）</param>
        /// <returns></returns>
        public PeriodicTaskHandle scheduleExecute(ScheduleTask task, long delay, long periodic)
        {
            TaskWrapper wrapper = new TaskWrapper(task, delay, periodic);

            PeriodicTaskHandle handle = new PeriodicTaskHandleImpl(wrapper, this);

            addTask(wrapper);

            return handle;
        }

        internal TaskWrapper GetPreiodictTask(long ticks)
        {
            lock (PreiodictTaskList)
            {
                if (PreiodictTaskList.Count == 0)
                {
                    return null;
                }
                else if (PreiodictTaskList[0].StartTime > ticks)
                {
                    return null;
                }

                TaskWrapper taskWrapper = PreiodictTaskList[0];
                PreiodictTaskList.RemoveAt(0);
                return taskWrapper;
            }
        }

        internal TaskWrapper getTask()
        {
            lock (TaskQueue) //使用TryEnter 导致了cpu的占用不稳定，升高，恢复原来的代码
            //if (Monitor.TryEnter(TaskQueue))
            {
                try
                {
                    if (TaskQueue.Count <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        TaskWrapper currentTask = TaskQueue.First.Value;
                        TaskQueue.RemoveFirst();
                        if (currentTask.canExecute)
                        {
                            return currentTask;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (System.Exception/* ex*/)
                {
                }
                //finally
                //{
                //    Monitor.Exit(TaskQueue);
                //}
            }
            //else
            //{
            //    return null;
            //}

            return null;
        }

        internal int GetTaskCount()
        {
            lock (TaskQueue)
            {
                return TaskQueue.Count;
            }
        }

        internal void addTask(TaskWrapper taskWrapper)
        {
            if (taskWrapper.Periodic > 0)
            {
                lock (PreiodictTaskList)
                {
                    ListExt.BinaryInsertAsc(PreiodictTaskList, taskWrapper, taskWrapper);
                }
            }
            else
            {
                lock (TaskQueue)
                {
                    TaskQueue.AddLast(taskWrapper);
                    taskWrapper.canExecute = true;
                }
            }
        }

        internal void removeTask(TaskWrapper taskWrapper)
        {
            lock (TaskQueue)
            {
                TaskQueue.Remove(taskWrapper);
                taskWrapper.canExecute = false;
            }
        }
    }
}
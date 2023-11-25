using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Tools;
using System.Threading;

namespace GameDBServer.Core.Executor
{

    public interface ScheduleTask
    {
        void run();
    }

    /// <summary>
    /// Task包装类,封装Task执行需要的一些参数
    /// </summary>
    internal class TaskWrapper
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

        public Worker(ScheduleExecutor executor)
        {
            this.executor = executor;
        }

        public Thread CurrentThread
        {
            set { this.currentThread = value; }
        }

        private TaskWrapper getCanExecuteTask()
        {
            TaskWrapper TaskWrapper = null;

            int getNum = 0;

            while (null != (TaskWrapper = executor.getTask()))
            {
                //还没开始执行的时间 
                if (TimeUtil.NOW() < TaskWrapper.StartTime)
                {
                    if (TaskWrapper.canExecute)
                        executor.addTask(TaskWrapper);
                    getNum++;
                    //任务队列上线1000/线程,如果当先没有可执行的任务，停止检索
                    if (getNum >= 1000)
                    {
                        break;
                    }
                    continue;
                }

                return TaskWrapper;
            }


            return null;
        }

        public void work()
        {
            TaskWrapper TaskWrapper = null;

            while (true)
            {
                //检索可执行的任务
                TaskWrapper = getCanExecuteTask();

                if (null == TaskWrapper)
                {
                    try
                    {
                        Thread.Sleep(5);
                    }
                    catch (ThreadInterruptedException)
                    {
                        continue;
                    }
                    //继续检索可执行的任务
                    continue;
                }

                try
                {
                    if (null == TaskWrapper || null == TaskWrapper.CurrentTask)
                        continue;

                    if (TaskWrapper.canExecute)
                        try
                        {
                            TaskWrapper.CurrentTask.run();
                        }
                        catch (System.Exception ex)
                        {
                            LogManager.WriteLog(LogTypes.Error, string.Format("异步调度任务执行错误: {0}", ex));
                        }
                        

                    //如果是周期执行的任务
                    if (TaskWrapper.Periodic > 0 && TaskWrapper.canExecute)
                    {
                        //设置下一次执行的时间
                        TaskWrapper.resetStartTime();
                        executor.addTask(TaskWrapper);
                        TaskWrapper.addExecuteCount();
                    }

                }
                catch (System.Exception ex)
                {
                    DataHelper.WriteFormatExceptionLog(ex, "异步调度任务执行异常", false);
                }

            }

        }

    }

    public class ScheduleExecutor
    {

        private List<Worker> workerQueue = null;
        private List<Thread> threadQueue = null;
        private LinkedList<TaskWrapper> TaskQueue = null;
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

        internal TaskWrapper getTask()
        {
            lock (TaskQueue)
            {
                if (TaskQueue.Count == 0)
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

        }

        internal void addTask(TaskWrapper taskWrapper)
        {
            lock (TaskQueue)
            {
                TaskQueue.AddLast(taskWrapper);
                taskWrapper.canExecute = true;
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

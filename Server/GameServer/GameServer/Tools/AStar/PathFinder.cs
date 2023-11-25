﻿#define DEBUGON

using System;
using System.Collections.Generic;

namespace HSGameEngine.Tools.AStar 
{
    #region Structs
    public struct PathFinderNode
    {
        #region Variables Declaration
        public int     F;
        public int     G;
        public int     H;  // f = gone + heuristic
        public int     X;
        public int     Y;
        public int     PX; // Parent
        public int     PY;
        #endregion
    }
    #endregion

    #region Enum
    public enum PathFinderNodeType
    {
        Start   = 1,
        End     = 2,
        Open    = 4,
        Close   = 8,
        Current = 16,
        Path    = 32
    }

    public enum HeuristicFormula
    {
        Manhattan           = 1,
        MaxDXDY             = 2,
        DiagonalShortCut    = 3,
        Euclidean           = 4,
        EuclideanNoSQR      = 5,
        Custom1             = 6
    }
    #endregion

    #region Delegates
    public delegate void PathFinderDebugHandler(int fromX, int fromY, int x, int y, PathFinderNodeType type, int totalCost, int cost);
    #endregion

    public class PathFinder : IPathFinder
    {

        #region Events
        public event PathFinderDebugHandler PathFinderDebug;
        #endregion

        #region Variables Declaration
        private byte[,]                         mGrid                   = null;
        private PriorityQueueB<PathFinderNode>  mOpen                   = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private List<PathFinderNode>            mClose                  = new List<PathFinderNode>();
        private bool                            mStop                   = false;
        private bool                            mStopped                = true;
        private int                             mHoriz                  = 0;
        private HeuristicFormula                mFormula                = HeuristicFormula.Manhattan;
        private bool                            mDiagonals              = true;
        private int                             mHEstimate              = 2;
        private bool                            mPunishChangeDirection  = false;
        private bool                            mReopenCloseNodes       = false;
        private bool                            mTieBreaker             = false;
        private bool                            mHeavyDiagonals         = false;
        private int                             mSearchLimit            = 2000;
        private double                          mCompletedTime          = 0;
        private bool                            mDebugProgress          = false;
        private bool                            mDebugFoundPath         = false;
        #endregion

        #region Constructors
        public PathFinder(byte[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mGrid = grid;
        }
        #endregion

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool Diagonals
        {
            get { return mDiagonals; }
            set { mDiagonals = value; }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool ReopenCloseNodes
        {
            get { return mReopenCloseNodes; }
            set { mReopenCloseNodes = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public List<PathFinderNode> FindPath(Point2D start, Point2D end)
        {
            PathFinderNode parentNode;
            bool found  = false;
            int  gridX  = mGrid.GetUpperBound(0);
            int  gridY  = mGrid.GetUpperBound(1);

            mStop       = false;
            mStopped    = false;
            mOpen.Clear();
            mClose.Clear();

            #if DEBUGON
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
            #endif

            sbyte[,] direction;
            if (mDiagonals)
                direction = new sbyte[8,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}, {1,-1}, {1,1}, {-1,1}, {-1,-1}};
            else
                direction = new sbyte[4,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}};

            parentNode.G         = 0;
            parentNode.H         = mHEstimate;
            parentNode.F         = parentNode.G + parentNode.H;
            parentNode.X         = start.X;
            parentNode.Y         = start.Y;
            parentNode.PX        = parentNode.X;
            parentNode.PY        = parentNode.Y;
            mOpen.Push(parentNode);
            while(mOpen.Count > 0 && !mStop)
            {
                parentNode = mOpen.Pop();

                #if DEBUGON
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Current, -1, -1);
                #endif

                if (parentNode.X == end.X && parentNode.Y == end.Y)
                {
                    mClose.Add(parentNode);
                    found = true;
                    break;
                }

                if (mClose.Count > mSearchLimit)
                {
                    mStopped = true;
                    return null;
                }

                if (mPunishChangeDirection)
                    mHoriz = (parentNode.X - parentNode.PX); 

                //Lets calculate each successors
                for (int i=0; i<(mDiagonals ? 8 : 4); i++)
                {
                    PathFinderNode newNode;
                    newNode.X = parentNode.X + direction[i,0];
                    newNode.Y = parentNode.Y + direction[i,1];

                    if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= gridX || newNode.Y >= gridY)
                        continue;

                    int newG;
                    if (mHeavyDiagonals && i>3)
                        newG = parentNode.G + (int) (mGrid[newNode.X, newNode.Y] * 2.41);
                    else
                        newG = parentNode.G + mGrid[newNode.X, newNode.Y];


                    if (newG == parentNode.G)
                    {
                        //Unbrekeable
                        continue;
                    }

                    if (mPunishChangeDirection)
                    {
                        if ((newNode.X - parentNode.X) != 0)
                        {
                            if (mHoriz == 0)
                                newG += 20;
                        }
                        if ((newNode.Y - parentNode.Y) != 0)
                        {
                            if (mHoriz != 0)
                                newG += 20;

                        }
                    }

                    int     foundInOpenIndex = -1;
                    for(int j=0; j<mOpen.Count; j++)
                    {
                        if (mOpen[j].X == newNode.X && mOpen[j].Y == newNode.Y)
                        {
                            foundInOpenIndex = j;
                            break;
                        }
                    }
                    if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].G <= newG)
                        continue;

                    int     foundInCloseIndex = -1;
                    for(int j=0; j<mClose.Count; j++)
                    {
                        if (mClose[j].X == newNode.X && mClose[j].Y == newNode.Y)
                        {
                            foundInCloseIndex = j;
                            break;
                        }
                    }
                    if (foundInCloseIndex != -1 && (mReopenCloseNodes || mClose[foundInCloseIndex].G <= newG))
                        continue;

                    newNode.PX      = parentNode.X;
                    newNode.PY      = parentNode.Y;
                    newNode.G       = newG;

                    switch(mFormula)
                    {
                        default:
                        case HeuristicFormula.Manhattan:
                            newNode.H       = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                            break;
                        case HeuristicFormula.MaxDXDY:
                            newNode.H       = mHEstimate * (Math.Max(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y)));
                            break;
                        case HeuristicFormula.DiagonalShortCut:
                            int h_diagonal  = Math.Min(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y));
                            int h_straight  = (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                            newNode.H       = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
                            break;
                        case HeuristicFormula.Euclidean:
                            newNode.H       = (int) (mHEstimate * Math.Sqrt(Math.Pow((newNode.X - end.X) , 2) + Math.Pow((newNode.Y - end.Y), 2)));
                            break;
                        case HeuristicFormula.EuclideanNoSQR:
                            newNode.H       = (int) (mHEstimate * (Math.Pow((newNode.X - end.X) , 2) + Math.Pow((newNode.Y - end.Y), 2)));
                            break;
                        case HeuristicFormula.Custom1:
                            Point2D dxy = new Point2D(Math.Abs(end.X - newNode.X), Math.Abs(end.Y - newNode.Y));
                            int Orthogonal  = Math.Abs(dxy.X - dxy.Y);
                            int Diagonal    = Math.Abs(((dxy.X + dxy.Y) - Orthogonal) / 2);
                            newNode.H       = mHEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
                            break;
                    }
                    if (mTieBreaker)
                    {
                        int dx1 = parentNode.X - end.X;
                        int dy1 = parentNode.Y - end.Y;
                        int dx2 = start.X - end.X;
                        int dy2 = start.Y - end.Y;
                        int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                        newNode.H = (int) (newNode.H + cross * 0.001);
                    }
                    newNode.F       = newNode.G + newNode.H;

                    #if DEBUGON
                    if (mDebugProgress && PathFinderDebug != null)
                        PathFinderDebug(parentNode.X, parentNode.Y, newNode.X, newNode.Y, PathFinderNodeType.Open, newNode.F, newNode.G);
                    #endif
                    

                    //It is faster if we leave the open node in the priority queue
                    //When it is removed, all nodes around will be closed, it will be ignored automatically
                    //if (foundInOpenIndex != -1)
                    //    mOpen.RemoveAt(foundInOpenIndex);

                    //if (foundInOpenIndex == -1)
                        mOpen.Push(newNode);
                }

                mClose.Add(parentNode);

                #if DEBUGON
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Close, parentNode.F, parentNode.G);
                #endif
            }

            if (found)
            {
                PathFinderNode fNode = mClose[mClose.Count - 1];
                for(int i=mClose.Count - 1; i>=0; i--)
                {
                    if (fNode.PX == mClose[i].X && fNode.PY == mClose[i].Y || i == mClose.Count - 1)
                    {
                        #if DEBUGON
                        if (mDebugFoundPath && PathFinderDebug != null)
                            PathFinderDebug(fNode.X, fNode.Y, mClose[i].X, mClose[i].Y, PathFinderNodeType.Path, mClose[i].F, mClose[i].G);
                        #endif
                        fNode = mClose[i];
                    }
                    else
                        mClose.RemoveAt(i);
                }
                mStopped = true;

                //进行简单的平滑处理
                //mClose = Floyd(mClose);

                return mClose;
            }
            mStopped = true;
            return null;
        }
        #endregion

        #region Inner Classes
        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            #region IComparer Members
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
            #endregion
        }
        #endregion

        #region 平滑处理
        /* 弗洛伊德路径平滑处理 
		form http://wonderfl.net/c/aWCe
        */
		public List<PathFinderNode> Floyd(List<PathFinderNode> _floydPath){
			if (null == _floydPath)
            {
				return null;
            }

            _floydPath = ReverseList(_floydPath);

			int len = _floydPath.Count;
			if (len > 2)
			{
				PathFinderNode vector = new PathFinderNode();
				PathFinderNode tempVector = new PathFinderNode();

				//遍历路径数组中全部路径节点，合并在同一直线上的路径节点
				//假设有1,2,3,三点，若2与1的横、纵坐标差值分别与3与2的横、纵坐标差值相等则
				//判断此三点共线，此时可以删除中间点2
				FloydVector(vector, _floydPath[len - 1], _floydPath[len - 2]);

				for (int i = _floydPath.Count - 3; i >= 0; i--)
				{
					FloydVector(tempVector, _floydPath[i + 1], _floydPath[i]);
                    if (vector.PX == tempVector.PX && vector.PY == tempVector.PY)
					{
                        _floydPath.RemoveAt(i + 1);
					} 
					else 
					{
                        vector.PX = tempVector.PX;
                        vector.PY = tempVector.PY;
					}
				}
			}

            _floydPath = ReverseList(_floydPath);

            return _floydPath;
			//合并共线节点后进行第二步，消除拐点操作。算法流程如下：
			//如果一个路径由1-10十个节点组成，那么由节点10从1开始检查
			//节点间是否存在障碍物，若它们之间不存在障碍物，则直接合并
			//此两路径节点间所有节点。
            //len = _floydPath.Count;
            //for (int i = len - 1; i >= 0; i--)
            //{
            //    for (int j = 0; j <= i - 2; j++)
            //    {
            //        if ( _grid.hasBarrier(_floydPath[i].X, _floydPath[i].Y, _floydPath[j].X, _floydPath[j].Y) == false )
            //        {
            //            for (int k = i - 1; k > j; k--)
            //            {
            //                _floydPath.RemoveAt(k);
            //            }
            //            i = j;
            //            len = _floydPath.Count;
            //            break;
            //        }
            //    }
            //}
		}

        private List<PathFinderNode> ReverseList(List<PathFinderNode> floydPath)
        {
            List<PathFinderNode> myFloydPath = new List<PathFinderNode>();
            for (int n = floydPath.Count - 1; n >= 0; n--)
            {
                myFloydPath.Add(floydPath[n]);
            }

            return myFloydPath;
        }

		private void FloydVector(PathFinderNode target, PathFinderNode n1, PathFinderNode n2) 
        {
            target.PX = n1.PX - n2.PX;
            target.PY = n1.PY - n2.PY;
		}

        #endregion
    }
}

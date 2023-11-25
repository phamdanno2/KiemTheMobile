using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Shapes;
using System.Runtime.InteropServices;
using UnityEngine;
using GameServer.KiemThe;
using GameServer.Logic;

namespace HSGameEngine.Tools.AStarEx
{
    #region Structs
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NodeFast
    {
        #region Variables Declaration
        public double f;
        public double g;
        public double h;
        public int parentX;
        public int parentY;
        #endregion
    }
    #endregion

    public class NodeGrid
    {		
		private int _startNodeX;
        private int _startNodeY;
		private int _endNodeX;
        private int _endNodeY;

        private static NodeFast[,] _nodes;

        private byte[,] _fixedObstruction;
        private byte[,] _blurObstruction;

        private static int _numCols;
        private static int _numRows;
		
		/**
		 * Constructor.
		 */
		public NodeGrid(int numCols, int numRows)
		{
			setSize( numCols, numRows );
		}

        public byte[,] GetFixedObstruction()
        {
            return _fixedObstruction;
        }

        public byte[,] GetBlurObstruction()
        {
            return _blurObstruction;
        }

        public void SetFixedObstruction(byte[,] obs)
		{
            this._fixedObstruction = obs;
		}

        public void SetBlurObstruction(byte[,] blurs)
		{
            this._blurObstruction = blurs;
		}
		

		
		////////////////////////////////////////
		// public methods
		////////////////////////////////////////
		
		public void setSize( int numCols, int numRows)
		{
            if (_nodes == null || _numCols < numCols || _numRows < numRows)
            {
                _numCols = Math.Max(numCols, _numCols);
                _numRows = Math.Max(numRows, _numRows);

                _nodes = new NodeFast[_numCols, _numRows];
            }

            _fixedObstruction = new byte[numCols, numRows];

            for (int i = 0; i < numCols; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    _fixedObstruction[i, j] = 1;
                }
            }
		}

        public void Clear()
        {
            Array.Clear(_nodes, 0, _nodes.Length);
        }

        public NodeFast[,] Nodes
        {
            get
            {
                return _nodes;
            }
        }

        /** 判断两个节点的对角线路线是否可走 */
        public bool isDiagonalWalkable(long node1, long node2)
        {
            int node1x = ANode.GetGUID_X(node1);
            int node1y = ANode.GetGUID_Y(node1);

            int node2x = ANode.GetGUID_X(node2);
            int node2y = ANode.GetGUID_Y(node2);

            if (1 == _fixedObstruction[node1x, node2y] && 1 == _fixedObstruction[node2x, node1y])
            {
                return true;
            }
			return false;
        }

        /// <summary>
        /// Check the corresponding pos can be reached
        /// <para>(Grid pos)</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool isWalkable(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }
            else if (x >= _fixedObstruction.GetUpperBound(0) || y >= _fixedObstruction.GetUpperBound(1))
            {
                return false;
            }
            return 1 == _fixedObstruction[x, y];
        }

        /// <summary>
        /// Kiểm tra có đường đi giữa 2 nút cho trước không
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <returns></returns>
        public bool HasPath(Point fromPos, Point toPos)
		{
            return _blurObstruction[(int) fromPos.X, (int) fromPos.Y] / 2 == _blurObstruction[(int) toPos.X, (int) toPos.Y] / 2;
        }
		
		////////////////////////////////////////
		// getters / setters
		////////////////////////////////////////
		
		/**
		 * Returns the end node.
		 */
		public int endNodeX
		{
            get { return  _endNodeX; }
		}

        /**
         * Returns the end node.
         */
        public int endNodeY
        {
            get { return _endNodeY; }
        }
		
		/**
		 * Returns the number of columns in the grid.
		 */
		public int numCols
		{
            get { return _numCols; }
		}
		
		/**
		 * Returns the number of rows in the grid.
		 */
		public int numRows
		{
            get { return _numRows; }
		}
		
		/**
		 * Returns the start node.
		 */
		public int startNodeX
		{
            get {  return _startNodeX; }
		}

        /**
         * Returns the start node.
         */
        public int startNodeY
        {
            get { return _startNodeY; }
        }
    }
}

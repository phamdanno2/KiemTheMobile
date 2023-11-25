using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GameServer.Logic
{
    public class SearchTable
    {
        /// <summary>
        /// 静态的点数组
        /// </summary>
        private static List<Point> _SearchTableList = new List<Point>();

        /// <summary>
        /// 获取静态的点数组
        /// </summary>
        /// <returns></returns>
        public static List<Point> GetSearchTableList()
        {
            return _SearchTableList;
        }

        /// <summary>
        /// 初始化大小
        /// </summary>
        /// <param name="num"></param>
        public static void Init(int num)
        {
            for (int i = -num; i <= num; i++)
            {
                for (int j = -num; j <= num; j++)
                {
                    _SearchTableList.Add(new Point(i, j));
                }
            }

            _SearchTableList.Sort(delegate(Point x, Point y)
            {
                return ((int)Math.Pow(x.X, 2) + (int)Math.Pow(x.Y, 2)) - ((int)Math.Pow(y.X, 2) + (int)Math.Pow(y.Y, 2));
            });
            /*
            int colsNum = num * 2 + 1;
            int rowsNum = num * 2 + 1;
            int centerX = num;
            int centerY = num;
            for (int i = 0; i < colsNum; i++)
            {
                for (int j = 0; j < rowsNum; j++)
                {
                    _SearchTableList.Add(new Point(i - centerX, j - centerY));
                }
            }

            _SearchTableList.Sort(delegate(Point x, Point y)
            {
                return ((int)Math.Abs(x.X) + (int)Math.Abs(x.Y)) - ((int)Math.Abs(y.X) + (int)Math.Abs(y.Y));
            });
            */
        }
    }
}

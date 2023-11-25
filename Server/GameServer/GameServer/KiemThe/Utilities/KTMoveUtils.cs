using GameServer.KiemThe.Entities;
using GameServer.Logic;
using GameServer.Server;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameServer.KiemThe.Utilities
{
    public class KTMoveUtils
    {
        public KTMoveUtils()
        {
        }

     
        /// <summary>
        /// Lấy postion tiếp theo theo hướng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nDir"></param>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        protected static void WalkNextPos(GameObject obj, Dircetions nDir, out int nX, out int nY)
        {
            Point grid = obj.CurrentGrid;
            int nCurrX = (int)grid.X;
            int nCurrY = (int)grid.Y;

            nX = nCurrX;
            nY = nCurrY;

            switch (nDir)
            {
                case Dircetions.DR_UP:
                    nX = nCurrX;
                    nY = nCurrY + 1;
                    break;
                case Dircetions.DR_UPRIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY + 1;
                    break;
                case Dircetions.DR_RIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY;
                    break;
                case Dircetions.DR_DOWNRIGHT:
                    nX = nCurrX + 1;
                    nY = nCurrY - 1;
                    break;
                case Dircetions.DR_DOWN:
                    nX = nCurrX;
                    nY = nCurrY - 1;
                    break;
                case Dircetions.DR_DOWNLEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY - 1;
                    break;
                case Dircetions.DR_LEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY;
                    break;
                case Dircetions.DR_UPLEFT:
                    nX = nCurrX - 1;
                    nY = nCurrY + 1;
                    break;
            }
        }

     
     
    }
}

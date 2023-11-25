using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSPlay.GameEngine.Sprite;

namespace FSPlay.GameEngine.Logic
{
    /// <summary>
    /// Sự kiện tọa độ thay đổi
    /// </summary>
    /// <param name="sprite"></param>
    public delegate void CoordinateEventHandler(GSprite sprite);

    /// <summary>
    /// Sự kiện thông báo đối tượng
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SpriteNotifyEventHandler(object sender, SpriteNotifyEventArgs e);
}

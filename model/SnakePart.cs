using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeQuest.model
{
    
    public class SnakePart : GamePage
    {
        // 定位蛇
        public UIElement UiElement { get; set; }
        public Point Position { get; set; }
        public bool IsHead {  get; set; }

     
    }
}

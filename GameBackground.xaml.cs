using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeQuest
{
    /// <summary>
    /// GameBackground.xaml 的交互逻辑
    /// </summary>
    public partial class GameBackground : UserControl
    {
        public GameBackground()
        {
            InitializeComponent();

            // 游戏界面图
            ImageBrush img = new ImageBrush(
                new BitmapImage(new Uri("/img/Snake_OG-logo.jpg",
                UriKind.Relative)));
            InitPicture.Background = img;
        }
    }
}

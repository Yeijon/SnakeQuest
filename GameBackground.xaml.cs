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
using SnakeQuest.model;

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
            this.DataContext = this;

        }

        private void GameBegin_Click(object sender, RoutedEventArgs e)
        {
            // 启动游戏界面
            GameWindow gameWindow = new GameWindow();
            gameWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            gameWindow.Show();

        }

        private void GameRank_Click(object sender, RoutedEventArgs e)
        {
            // 打开排行榜
            GameRank gameRank = new GameRank();
            gameRank.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            gameRank.Show();
        }

        private void GameSetting_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 实现跨界绑定
            Setting setting = new Setting();
            setting.Show(); 
        }
    }
}

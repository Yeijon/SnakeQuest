using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 游戏窗体界面出现的话，那么游戏开始界面需要最小化
            // TEST
            if (!IsWindowOpen<GameWindow>())
            {
                var gameBackground = new GameBackground();
                mainwin.Children.Add(gameBackground);
  
            } else
            {
                this.WindowState = WindowState.Minimized;
            }
        }
    
        // 控制窗体切换
        /// <summary>
        /// 判断是否存在某窗体，静态函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowName"></param>
        /// <returns></returns>
        public static bool IsWindowOpen<T>(string windowName = "") where T : Window
        {
            var windows = Application.Current.Windows.OfType<T>();
            if (!string.IsNullOrEmpty(windowName))
                return windows.Any(w => w.Name == windowName);
            else
                return windows.Any();
        }

    }
}
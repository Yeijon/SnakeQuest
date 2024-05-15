using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Xps;

namespace SnakeQuest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 编写启动程序
        /// </summary>
        private void App_StartUp(object sender, StartupEventArgs e)
        {
           
            // 开始界面
            MainWindow Beginwindow = new MainWindow();
            Beginwindow.Show();

            // TODO: 实现跨界绑定
            //Setting setting = new Setting();
            //setting.Show();

            

            // TEST: database connection Great! 通过!
            /* GameRank test = new GameRank();
             test.Show();*/

        } 
    }

}

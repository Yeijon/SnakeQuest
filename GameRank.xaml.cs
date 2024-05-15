using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Shapes;
using SnakeQuest.model;

namespace SnakeQuest
{
    /// <summary>
    /// GameRank.xaml 的交互逻辑
    /// </summary>
    public partial class GameRank : Window
    {
        public GameRank()
        {
            InitializeComponent();

            // 数据库连接
            database = new Database();
            myconnection = database.ConnectSQL();

            // 加载后就刷新一下加载数据库
            refresh();


            
        }

        // 数据库连接
        private Database database;
        private SQLiteConnection myconnection;


        #region 数据库控制函数

        private void refresh()
        {
            /* using (OperateData context = new OperateData())
             {
                 databaseUsers = context.DataItems.ToList();
                 ItemList.ItemsSource = databaseUsers;

             }*/
            // TEST: 测试Database.cs  Great! 通过！
            
            /*database.CreatTable(myconnection);
            string t = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            database.AddData(myconnection, "test1", 1, t);
            database.AddData(myconnection, "test2", 2, t);
            database.AddData(myconnection, "test3", 3, t);
            database.AddData(myconnection, "test4", 4, t);*/
           // TEST: 从数据库中读取数据，并且绑定到 前端界面 Great! 通过！
           ItemList.ItemsSource = database.ReadData(myconnection);


        }

        #endregion

        /// <summary>
        /// 刷新读取数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            refresh();

        }
    }
}

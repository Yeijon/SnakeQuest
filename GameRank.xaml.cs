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
using Database;
using System.ComponentModel;

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
            database = new database();
            myconnection = database.ConnectSQL();

            // 加载后就刷新一下加载数据库
            refresh();


            
        }

        // 数据库连接
        private database database;
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


        private void delete()
        {
            // TODO: FINISTH
            string name = searchbox.Text;
            database.DeleteData(myconnection, name);

        }


        /// <summary>
        /// 刷新读取数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            refresh();

        }
    
        /// <summary>
        /// 点击列头进行排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sort_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //获得点击的列
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                if (clickedColumn != null)

                {
                    //Get binding property of clicked column

                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path;
                    //获得listview项是如何排序的
                    SortDescriptionCollection sdc = this.ItemList.Items.SortDescriptions;

                    //按升序进行排序
                    ListSortDirection sortDirection = ListSortDirection.Ascending;
                    if (sdc.Count > 0)
                    {
                        SortDescription sd = sdc[0];
                        sortDirection = (ListSortDirection)((((int)sd.Direction) + 1) % 2);
                        sdc.Clear();
                    }
                    sdc.Add(new SortDescription(bindingProperty, sortDirection));
                }
            }
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            delete();
            refresh();
        }
    }
}

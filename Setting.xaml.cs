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
using System.Windows.Shapes;

namespace SnakeQuest
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (MusicRadioButton.IsChecked == true)
            {
                SettingUpdateResponse.musicOK = true;
            } else
            {
                SettingUpdateResponse.musicOK = false;
            }

        }

        private void speedvalue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SettingUpdateResponse.SnakeSpeedControl = (int)speedvalue.Value;
        }
    }
}

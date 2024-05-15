using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

/* 出现问题
namespace SnakeQuest.model
{
    /// <summary>
    /// 继承 GamePage.xaml : 用于调用画布 GameArea
    /// </summary>
    public class DrawGameBG : Window // QUESTION: 继承关系？会有问题吗？果然有问题，见 GamePage.xaml.cs中 HACK 
    {
        // 成员变量
        public const int SquareSize = 25; // 在450*800大小小，在公约数中取值25

        public void DrawGameArea()
        {

            bool drawFlag = false; // 画图标志
            bool isOdd = false; // 判断交错的方块 
            int x = 0, y = 0; // 定位小方块
            int rowCounter = 0;

            // 画出黑白方块交错的游戏背景图
            while (!drawFlag)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SquareSize,
                    Height = SquareSize,
                    Fill = isOdd ? Brushes.White : Brushes.Black,
                };
                // 在画布上添加小方块
                // GameArea.Children.Add(rect);  见 Hack 注释的错误 // 失败。。。
                
                // Canvas GameArea = (Canvas)FindName("GameArea"); 失败
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, y);
                Canvas.SetLeft(rect, x);

                isOdd = true;
                x += SquareSize;

                // 不能把小方块添加超出画布：也就是说完成一列/行的添加要重新计算添加位置
                if (x >= GameArea.ActualWidth)
                {
                    x = 0;
                    y += SquareSize;
                    rowCounter++;
                    isOdd = (rowCounter % 2 != 0);
                }

                if (y >= GameArea.ActualHeight)
                {
                    drawFlag = true;
                }
            }
        }



    }
}

*/

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
    /// GamePage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : UserControl
    {

        DrawGameBG gamebg =  new DrawGameBG(); // 画出游戏背景
        int SnakeSquareSize = DrawGameBG.SquareSize; // 需要是画布计算的整数倍，所以封装起来固定了

        // 设置蛇的相关参数
        private SolidColorBrush snakeBodyBrush = Brushes.Green;
        private SolidColorBrush snakeHeadBrush = Brushes.YellowGreen;
        private List<SnakePart> snakeParts = new List<SnakePart>(); 

        // 设置蛇移动的相关参数
        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection snakeDirection; // 确定蛇的方向
        private int snakeLength; // 用来计算蛇长

        // 常量值：更改口
        // TODO: 将放入设置中，并入菜单，待考虑
        const int SnakeStartLength = 3;
        const int SnakeStartSpend = 300; // 启示速度
        const int SnakeSpeedLevel = 100; // 速度变化增长值

        // 时间检测
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new();

        // 构造函数
        public GamePage()
        {
            InitializeComponent();

            gameTickTimer.Tick += GameTickTimer_Tick;
        }
        
        // 通过时间检测让蛇移动
        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        // 与前端交互的控制函数

        // TODO: 当 GamePage页面受“开始”按钮加载后，页面初始化控制事件函数
        private void GamePage_Initialized(object sender, EventArgs e)
        {
            gamebg.DrawGameArea(); // 当该页面被加载就画出游戏背景（黑白方块交错构成）
            StartNewGame(); // 加载游戏
        }
        
        // 其余控制函数

        private void DrawSnake()
        {
            foreach (SnakePart snakePart in snakeParts)
            {
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = SnakeSquareSize,
                        Height = SnakeSquareSize,
                        Fill = (snakePart.IsHead ? snakeHeadBrush : snakeBodyBrush)
                    };

                    GameArea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);

                }
            }
        }
       
        private void MoveSnake()
        {
            // 每移动一步，会凋亡一个蛇尾
            while (snakeParts.Count >= snakeLength)
            {
                GameArea.Children.Remove(snakeParts[0].UiElement);
                snakeParts.RemoveAt(0);
            }

            // 移动后原来的蛇头变成蛇身了 
            foreach (SnakePart snakePart in snakeParts)
            {
                (snakePart.UiElement as Rectangle).Fill = snakeBodyBrush;
                snakePart.IsHead = false;
            }
            
            // 开始定位蛇头
            SnakePart snakeHead = snakeParts[snakeParts.Count - 1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= SnakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += SnakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= SnakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += SnakeSquareSize;
                    break;
            }
            
            // 这是一个新位置的蛇
            snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextX),
                IsHead = true,
            });
            
            // 在画布上画出来
            DrawSnake();

            // 碰撞检测
        }
    
        private void StartNewGame()
        {
            snakeLength = SnakeStartLength;
            snakeDirection = SnakeDirection.Right;
            snakeParts.Add(new SnakePart()
            {
                Position = new Point(SnakeSquareSize * 3, SnakeSquareSize * 3)
            });
            
            // 通过控制时间计时器来控制速度的变化
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpend);

            DrawSnake() ;

            // 计时开始，游戏开始
            gameTickTimer.IsEnabled = true;

        }
    }
}

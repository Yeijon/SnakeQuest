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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SnakeQuest.model;

namespace SnakeQuest
{
    /// <summary>
    /// GameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();

            // TODO: 实现跨界绑定
            //Setting setting = new Setting();
            //setting.Show();
            
            // 播放音乐设定
            if (SettingUpdateResponse.musicOK)
            {
                player = new MediaPlayer();
                string path = AppDomain.CurrentDomain.BaseDirectory + "snake_music.mp3";
                player.Open(new Uri(path, UriKind.Absolute));
                player.Play();
                
            }


            gameTickTimer.Tick += GameTickTimer_Tick;

            DrawGameArea();
            StartNewGame();

            // 创建数据库连接
            database = new Database();
            myconnection = database.ConnectSQL();
            database.CreatTable(myconnection);

        }


        #region 成员变量：游戏的设定
        // 游戏声音
        private MediaPlayer player;

        // 数据库
        private Database database;
        private SQLiteConnection myconnection;

        // 绘图相关
        private const int SquareSize = 25; // 在450*800大小小，在公约数中取值25
        private int SnakeSquareSize = SquareSize;


        // 设置蛇的相关参数
        private SolidColorBrush snakeBodyBrush = Brushes.Green;
        private SolidColorBrush snakeHeadBrush = Brushes.YellowGreen;
        private List<SnakePart> snakeParts = new List<SnakePart>();

        // 设置蛇移动的相关参数
        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection snakeDirection; // 确定蛇的方向
        private int? snakeLength = null; // 用来计算蛇长

        // 设置食物的相关参数
        private UIElement? snakeFood = null;
        private SolidColorBrush foodBrush = Brushes.Red;

        // 常量值：更改口
        // TODO: 将放入设置中，并入菜单，待考虑
        const int SnakeStartLength = 3;
        int SnakeStartSpend = SettingUpdateResponse.SnakeSpeedControl; // 起始速度
        const int SnakeSpeedLevel = 80; // 速度极限

        // 记录分数
        private int currentScore = 0;

        // 时间检测
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new();

        // 随机数
        private Random rd = new();

        #endregion

        // 通过时间检测让蛇移动
        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        #region 与前端交互的控制函数

        // TODO: 当 GamePage页面受“开始”按钮加载后，页面初始化控制事件函数
        private void GamePage_Initialized(object sender, EventArgs e)
        {
            DrawGameArea(); // 当该页面被加载就画出游戏背景（黑白方块交错构成）
            StartNewGame(); // 加载游戏
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            SnakeDirection originalSnakeDirection = snakeDirection;
            switch (e.Key)
            {
                case Key.Up or Key.K:
                    if (snakeDirection != SnakeDirection.Down)
                        snakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down or Key.J:
                    if (snakeDirection != SnakeDirection.Up)
                        snakeDirection = SnakeDirection.Down;
                    break;
                case Key.Left or Key.H:
                    if (snakeDirection != SnakeDirection.Right)
                        snakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right or Key.L:
                    if (snakeDirection != SnakeDirection.Left)
                        snakeDirection = SnakeDirection.Right;
                    break;
                case Key.Space:
                    this.Close();
                    break;
            }
            if (snakeDirection != originalSnakeDirection)
                MoveSnake();
        }

        #endregion

        // 其余控制函数
        #region 1. 绘出画布
        private void DrawGameArea()
        {

            bool drawFlag = false; // 画图标志
            bool isOdd = false; // 判断交错的方块 
            int x = 0, y = 0; // 定位小方块
            int rowCounter = 0;

            // 画出黑白方块交错的游戏背景图
            while (drawFlag == false)
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

                isOdd = !isOdd;
                x += SquareSize;

                // 不能把小方块添加超出画布：也就是说完成一列/行的添加要重新计算添加位置
                if (x >= GameArea.Width)
                {
                    x = 0;
                    y += SquareSize;
                    rowCounter++;
                    isOdd = (rowCounter % 2 != 0);
                }

                if (y >= GameArea.Height)
                {
                    drawFlag = true;
                }
            }
        }

        #endregion

        #region 2. 贪吃蛇的移动

        /// <summary>
        /// 画布上画出贪吃蛇
        /// </summary>
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

        /// <summary>
        /// 画布上贪吃蛇的移动
        /// </summary>
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
                Position = new Point(nextX, nextY),
                IsHead = true,
            });

            // 在画布上画出来
            DrawSnake();

            // 碰撞检测
            CollisionCheck();
        }

        #endregion

        #region 3. 贪吃蛇的食物

        /// <summary>
        /// 画布上绘出食物
        /// </summary>
        private void DrawSnakeFood()
        {
            Point foodPosition = GetFoodPosition();
            snakeFood = new Ellipse()
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize,
                Fill = foodBrush,
            };
            GameArea.Children.Add(snakeFood);
            Canvas.SetTop(snakeFood, foodPosition.Y);
            Canvas.SetLeft(snakeFood, foodPosition.X);
        }

        /// <summary>
        /// 获取食物的位置
        /// </summary>
        /// <returns></returns>
        private Point GetFoodPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / SnakeSquareSize);
            int maxY = (int)(GameArea.ActualHeight / SnakeSquareSize);
            int foodX = rd.Next(0, maxX) * SnakeSquareSize;
            int foodY = rd.Next(0, maxY) * SnakeSquareSize;

            // 食物点不能落在蛇身上
            foreach (SnakePart snakePart in snakeParts)
            {
                if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
                {
                    return GetFoodPosition();
                }
            }

            return new Point(foodX, foodY);
        }

        /// <summary>
        /// 检测贪吃蛇吃到食物后加快速度
        /// </summary>
        private void EatSnakeFood()
        {
            snakeLength++;
            currentScore++;

            // 随着分数加快速度
            int timerInterval = Math.Max(SnakeSpeedLevel, (int)gameTickTimer.Interval.TotalMilliseconds - (currentScore * 2));
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);

            GameArea.Children.Remove(snakeFood);
            DrawSnakeFood();

            UpdateGameStatus();
        }

        #endregion

        #region 4. 游戏控制函数
        /// <summary>
        /// 游戏启动函数
        /// </summary>
        private void StartNewGame()
        {
            snakeLength = SnakeStartLength;
            snakeDirection = SnakeDirection.Right;
            snakeParts.Add(new SnakePart()
            {
                Position = new Point(SnakeSquareSize * 5, SnakeSquareSize * 5)
            });

            // 通过控制时间计时器来控制速度的变化
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpend);

            DrawSnake();
            DrawSnakeFood();

            UpdateGameStatus();

            // 计时开始，游戏开始
            gameTickTimer.IsEnabled = true;

        }

        /// <summary>
        /// 更新游戏得分状态（绑定在窗体名称）
        /// </summary>
        private void UpdateGameStatus()
        {
            this.Title = "SnakeWPF - Score: " + currentScore + " - Game speed: " + gameTickTimer.Interval.TotalMilliseconds;
        }
        
        /// <summary>
        /// 游戏结束：停止计时器，并弹出MessageBox
        /// </summary>
        private void EndGame()
        {
            gameTickTimer.IsEnabled = false;
            //player.Stop();

            // 添加成绩入数据库中
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "游戏结束！\n是否将成绩录入？^V^", "Game Over",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    string userName = Microsoft.VisualBasic.Interaction.InputBox
                        ("输入你的名字：", "Name", "you name here", 500, 500);

                    // TEST: 向数据库中添加数据
                    string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.AddData(myconnection, userName, currentScore, time);

                    MessageBox.Show("按空格键退出游戏界面，快去看看排行榜瞧瞧别人的成绩吧！", "Bye~", MessageBoxButton.OK, MessageBoxImage.Information);

                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("╮(╯-╰)╭ 好吧 ~，你可以按空格键退出游戏界面！", "Bye~", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
        
        /// <summary>
        /// 碰撞检测
        /// </summary>
        private void CollisionCheck()
        {
            SnakePart snakeHead = snakeParts[snakeParts.Count - 1];

            if ((snakeHead.Position.X == Canvas.GetLeft(snakeFood)) && (snakeHead.Position.Y == Canvas.GetTop(snakeFood)))
            {
                EatSnakeFood();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= GameArea.Height) ||
            (snakeHead.Position.X < 0) || (snakeHead.Position.X >= GameArea.Width))
            {
                EndGame();
            }

            // 蛇撞上自己了
            foreach (SnakePart snakeBodyPart in snakeParts.Take(snakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }

        #endregion

        #region 

        // TEST: Error! 使用 Microsoft.En....sqlite 包加载数据库一直错误，遂换一种方式实现数据库连接 
        /// <summary>
        /// 给用户添加成绩
        /// </summary>
        //private void create(string name, int score)
        //{
        //    DateTime time = DateTime.Now;
        //    using OperateData context = new OperateData();
        //    if (name != null)
        //    {
        //        context.DataItems.Add(new DataItem() { Name = name, Score = score, Time = time });
        //        context.SaveChanges();
        //    }

        //}

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            if (SettingUpdateResponse.musicOK)
            {
                player.Stop();
            }
        }
    }
}

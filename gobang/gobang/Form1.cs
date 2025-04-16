using System.Drawing.Drawing2D;
using System.Media;

namespace gobang
{
    public partial class Form0 : Form
    {
        private Game game;
        private Graphics g;
        private SoundPlayer sound_player;
        private bool handling_click;
        private Consts consts;
        public Form0()
        {
            InitializeComponent();
            game = new Game();
            g = mainpanel.CreateGraphics();
            sound_player = new SoundPlayer();
            handling_click = false;
            //获取窗体的宽度和高度
            Size size = new Size();
            size = this.Size;
            int width = size.Width;
            int height = size.Height;
            //获取mainpanel的宽度和高度
            size = mainpanel.Size;
            int width1 = size.Width;
            int height1 = size.Height;
            consts = new Consts(width, height, width1, height1);
        }

        //棋盘
        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {
            DrawingBoard(g);
            DrawAllChess(g, game.Board);
        }

        //提示
        private void subpanel_Paint(object sender, PaintEventArgs e)
        {
            subpanel.Size = new Size(consts.window_width - consts.board_width - consts.margin / 2, consts.board_height);
        }

        //开始按钮
        private void button_start_Click(object sender, EventArgs e)
        {
            bool? valid;//记录是否做了选择
            using (var dialog = new SelectionForm())
            {
                dialog.ShowDialog();
                valid = dialog.auto;
            }
            if (valid != null)
            {
                game.auto = (bool)valid;
                game.InitializeGame();
                button_start.Enabled = false;
                button_restart.Visible = true;
                button_withdraw.Visible = true;
                ToggleText(false);
                mainpanel.Invalidate();//Invalidate用于重绘控件
                sound_player.SoundLocation = consts.sound_location_4;
                sound_player.Play();
            }
        }

        //重新开始按钮
        private void button_restart_Click(object sender, EventArgs e)
        {
            //MessageBox.Show的参数为提示信息、标题、按钮样式、图标样式
            if (MessageBox.Show("确定重开吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                button_start.Enabled = true;
                button_start_Click(sender, e);
            }
        }

        //退出按钮
        private void button_quit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Dispose();//Dispose用于释放资源
            }
        }

        //悔棋按钮
        private void button_withdraw_Click(object sender, EventArgs e)
        {
            int flag = 0;
            if (!game.auto)
                flag = 1;
            else if (game.auto && game.IsBlack)
                flag = 2;
            else if (game.over && !game.IsBlack)
                flag = 3;
            else
                return;
            if (MessageBox.Show("确定悔棋吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sound_player.SoundLocation = consts.sound_location_3;
                sound_player.Play();
                if (game.count > 0)
                {
                    game.IsStarted = true;
                    //if (flag==3)
                    {
                        game.over = false;
                        //game.IsBlack = !game.IsBlack;
                    }
                    if (game.autoprior != null)
                        game.auto = true;
                    game.Withdraw();
                    if (game.auto && flag != 3)//如果是AI模式，需要悔两步
                        game.Withdraw();
                    mainpanel.Invalidate();
                    string msg = "";
                    if (game.LastPos[1] != int.MaxValue && game.LastPos[1] != int.MaxValue)
                        msg = "对方下在了:\n" + game.LastPos[1] + "行" + game.LastPos[0] + "列\n";
                    ToggleText(!game.IsBlack, game.auto, msg);
                }
                else
                {
                    MessageBox.Show("无棋可悔!");
                    ToggleText(!game.IsBlack, game.auto);
                }
            }

        }

        //窗体加载
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(800, 150);
            hint.Text = "\n游戏未开始\n";
            button_start.Visible = true;
            button_restart.Visible = false;
            button_withdraw.Visible = false;
        }

        //绘制棋盘
        private void DrawingBoard(Graphics g)
        {
            const int num = Consts.board_size;
            int width = consts.grid_width;
            int height = consts.grid_height;
            int semimargin = consts.semimargin;

            //g.Clear(mainpanel.BackColor);
            Pen pen = new Pen(Color.Brown, 3);

            for (int i = 0; i < num + 1; i++)
            {
                //DrawLine的参数为画笔、起点坐标和终点坐标
                g.DrawLine(pen, new Point(semimargin, i * height + semimargin), new Point(mainpanel.Width - semimargin, i * height + semimargin));//横线
                g.DrawLine(pen, new Point(i * width + semimargin, semimargin), new Point(i * width + semimargin, mainpanel.Width - semimargin));//竖线
            }
        }

        //绘制单个棋子
        private void DrawSingleChess(Graphics g, Point point, bool isBlack)
        {
            //Point center = new Point(point.X - consts.margin, point.Y - consts.margin);
            Point center = point;
            g.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量插值算法

            if (isBlack)
            {
                LinearGradientBrush brush = new LinearGradientBrush(new Point(consts.margin, consts.semimargin + consts.grid_height / 2), new Point(consts.margin, consts.semimargin + (int)(consts.grid_height * 1.5)), Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0));
                g.FillEllipse(brush, center.X - consts.piece_size, center.Y - consts.piece_size, consts.piece_size, consts.piece_size);
            }
            else
            {
                LinearGradientBrush brush = new LinearGradientBrush(new Point(consts.margin, consts.semimargin + consts.grid_height / 2), new Point(consts.margin, consts.semimargin + (int)(consts.grid_height * 1.5)), Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204));
                g.FillEllipse(brush, center.X - consts.piece_size, center.Y - consts.piece_size, consts.piece_size, consts.piece_size);
            }

        }

        //绘制所有棋子
        private void DrawAllChess(Graphics g, bool?[,] board)
        {
            for (int i = 0; i <= Consts.board_size; i++)
            {
                for (int j = 0; j <= Consts.board_size; j++)
                {
                    if (board[i, j] != null)
                    {
                        Point point = new Point(consts.margin + i * consts.grid_width, consts.margin + j * consts.grid_height);
                        DrawSingleChess(g, point, (bool)board[i, j]);
                    }
                }
            }
        }

        //标记最新下的棋子
        private void DrawLocation(Graphics g, Point priopoint, Point point, bool isBlack)
        {
            if (!priopoint.Equals(new Point(consts.margin, consts.margin)))//第一次标记不需要重画，而且标记在本次下的棋子上
                DrawSingleChess(g, priopoint, isBlack);//通过重画去除上次标记
            else
                priopoint = point;
            point.X -= consts.margin / 2;
            point.Y -= consts.margin / 2;

            //在对应位置画标记
            Brush brush = new SolidBrush(Color.Green);
            g.FillEllipse(brush, point.X - consts.location_mark / 2, point.Y - consts.location_mark / 2, consts.location_mark, consts.location_mark);
        }

        private void ToggleText(bool isblack, bool start = false, string msg = "")
        {//当前回合是黑方，提示下一回合白方下棋
            string text;
            if (isblack && !start)
                text = msg + "现在请白方下棋";
            else
                text = msg + "现在请黑方下棋";
            hint.Text = text;
            hint.Refresh();
        }

        //鼠标点击棋盘
        private void mainpanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (handling_click)
                return;
            else
            {
                handling_click = true;
                if (game.IsStarted && (!game.auto || game.auto && game.IsBlack))//游戏开始；不使用AI下棋，或者是使用AI，但轮到下黑棋
                {
                    int?[] accurate = consts.Accurate(e.Location);//对应点位的数组位置
                    if (accurate[0] != null && accurate[1] != null)
                    {
                        if (game[accurate[0].Value, accurate[1].Value] == null)//判断该位置是否已有棋子
                            SettleChess(false, accurate[0].Value, accurate[1].Value);
                        else
                        {
                            handling_click = false;
                            return;
                        }
                    }
                    else
                        hint.Text = "\n请点击开始游戏\n";

                    if (game.IsStarted && game.auto)
                        AutoSettle(game.LastPos);
                }
                handling_click = false;
            }
        }

        //画棋子、标记棋子、判断输赢
        private bool SettleChess(bool isAuto, int pos1, int pos2)
        {

            DrawSingleChess(g, consts.Inexact(pos1, pos2), game.IsBlack);
            if (game.LastPos[0] != int.MaxValue && game.LastPos[1] != int.MaxValue)
                DrawLocation(g, consts.Inexact(game.LastPos[0], game.LastPos[1]), consts.Inexact(pos1, pos2), !game.IsBlack);
            sound_player.SoundLocation = consts.sound_location_1;
            sound_player.Play();
            game[pos1, pos2] = game.IsBlack;//记录对应位置的棋子情况
            if (!game.auto || game.auto && isAuto)
            {
                string msg;
                msg = "对方下在了:\n" + pos2 + "行" + pos1 + "列\n";

                //将信息追加地写入到文本文件中
                //File.AppendAllText("log.txt", msg);

                ToggleText(game.IsBlack, game.count == 0, msg);
            }
            bool? status = game.JudgeGame(new int[] { pos1, pos2 });
            game.IsBlack = !game.IsBlack;
            if (status != false)
            {
                sound_player.SoundLocation = consts.sound_location_2;
                sound_player.Play();
                if (status == true)
                {
                    if (!game.IsBlack)//已切换到白方回合，但是黑方已经获胜
                    {
                        MessageBox.Show("黑方获胜，技高一筹!");
                        hint.Text = "黑方已获胜\n.\n请重新开始游戏";
                    }
                    else
                    {
                        MessageBox.Show("白方获胜，棋高一着!");
                        hint.Text = "白方已获胜\n.\n请重新开始游戏";
                    }
                    hint.Refresh();
                    //禁止后续点击事件
                    game.IsStarted = false;
                    game.over = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("平局，势均力敌!");
                    hint.Text = "平局\n.\n请重新开始游戏";
                    hint.Refresh();
                    game.IsStarted = false;
                    return true;
                }
            }

            return false;
        }

        private void AutoSettle(int[] lastpos)
        {
            game.IsStarted = false;
            hint.Text = ".\n对手正在思考中\n.";
            hint.Refresh();
            int[] pos = game.minmax.GetAction(lastpos, game.count);
            if (SettleChess(true, pos[0], pos[1]))//棋局胜利
                game.auto = false;
            else
                game.IsStarted = true;
        }

    }
}
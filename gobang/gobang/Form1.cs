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
            //��ȡ����Ŀ�Ⱥ͸߶�
            Size size = new Size();
            size = this.Size;
            int width = size.Width;
            int height = size.Height;
            //��ȡmainpanel�Ŀ�Ⱥ͸߶�
            size = mainpanel.Size;
            int width1 = size.Width;
            int height1 = size.Height;
            consts = new Consts(width, height, width1, height1);
        }

        //����
        private void mainpanel_Paint(object sender, PaintEventArgs e)
        {
            DrawingBoard(g);
            DrawAllChess(g, game.Board);
        }

        //��ʾ
        private void subpanel_Paint(object sender, PaintEventArgs e)
        {
            subpanel.Size = new Size(consts.window_width - consts.board_width - consts.margin / 2, consts.board_height);
        }

        //��ʼ��ť
        private void button_start_Click(object sender, EventArgs e)
        {
            bool? valid;//��¼�Ƿ�����ѡ��
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
                mainpanel.Invalidate();//Invalidate�����ػ�ؼ�
                sound_player.SoundLocation = consts.sound_location_4;
                sound_player.Play();
            }
        }

        //���¿�ʼ��ť
        private void button_restart_Click(object sender, EventArgs e)
        {
            //MessageBox.Show�Ĳ���Ϊ��ʾ��Ϣ�����⡢��ť��ʽ��ͼ����ʽ
            if (MessageBox.Show("ȷ���ؿ���", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                button_start.Enabled = true;
                button_start_Click(sender, e);
            }
        }

        //�˳���ť
        private void button_quit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ȷ���˳���", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Dispose();//Dispose�����ͷ���Դ
            }
        }

        //���尴ť
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
            if (MessageBox.Show("ȷ��������", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
                    if (game.auto && flag != 3)//�����AIģʽ����Ҫ������
                        game.Withdraw();
                    mainpanel.Invalidate();
                    string msg = "";
                    if (game.LastPos[1] != int.MaxValue && game.LastPos[1] != int.MaxValue)
                        msg = "�Է�������:\n" + game.LastPos[1] + "��" + game.LastPos[0] + "��\n";
                    ToggleText(!game.IsBlack, game.auto, msg);
                }
                else
                {
                    MessageBox.Show("����ɻ�!");
                    ToggleText(!game.IsBlack, game.auto);
                }
            }

        }

        //�������
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(800, 150);
            hint.Text = "\n��Ϸδ��ʼ\n";
            button_start.Visible = true;
            button_restart.Visible = false;
            button_withdraw.Visible = false;
        }

        //��������
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
                //DrawLine�Ĳ���Ϊ���ʡ����������յ�����
                g.DrawLine(pen, new Point(semimargin, i * height + semimargin), new Point(mainpanel.Width - semimargin, i * height + semimargin));//����
                g.DrawLine(pen, new Point(i * width + semimargin, semimargin), new Point(i * width + semimargin, mainpanel.Width - semimargin));//����
            }
        }

        //���Ƶ�������
        private void DrawSingleChess(Graphics g, Point point, bool isBlack)
        {
            //Point center = new Point(point.X - consts.margin, point.Y - consts.margin);
            Point center = point;
            g.SmoothingMode = SmoothingMode.HighQuality;//�����
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//��������ֵ�㷨

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

        //������������
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

        //��������µ�����
        private void DrawLocation(Graphics g, Point priopoint, Point point, bool isBlack)
        {
            if (!priopoint.Equals(new Point(consts.margin, consts.margin)))//��һ�α�ǲ���Ҫ�ػ������ұ���ڱ����µ�������
                DrawSingleChess(g, priopoint, isBlack);//ͨ���ػ�ȥ���ϴα��
            else
                priopoint = point;
            point.X -= consts.margin / 2;
            point.Y -= consts.margin / 2;

            //�ڶ�Ӧλ�û����
            Brush brush = new SolidBrush(Color.Green);
            g.FillEllipse(brush, point.X - consts.location_mark / 2, point.Y - consts.location_mark / 2, consts.location_mark, consts.location_mark);
        }

        private void ToggleText(bool isblack, bool start = false, string msg = "")
        {//��ǰ�غ��Ǻڷ�����ʾ��һ�غϰ׷�����
            string text;
            if (isblack && !start)
                text = msg + "������׷�����";
            else
                text = msg + "������ڷ�����";
            hint.Text = text;
            hint.Refresh();
        }

        //���������
        private void mainpanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (handling_click)
                return;
            else
            {
                handling_click = true;
                if (game.IsStarted && (!game.auto || game.auto && game.IsBlack))//��Ϸ��ʼ����ʹ��AI���壬������ʹ��AI�����ֵ��º���
                {
                    int?[] accurate = consts.Accurate(e.Location);//��Ӧ��λ������λ��
                    if (accurate[0] != null && accurate[1] != null)
                    {
                        if (game[accurate[0].Value, accurate[1].Value] == null)//�жϸ�λ���Ƿ���������
                            SettleChess(false, accurate[0].Value, accurate[1].Value);
                        else
                        {
                            handling_click = false;
                            return;
                        }
                    }
                    else
                        hint.Text = "\n������ʼ��Ϸ\n";

                    if (game.IsStarted && game.auto)
                        AutoSettle(game.LastPos);
                }
                handling_click = false;
            }
        }

        //�����ӡ�������ӡ��ж���Ӯ
        private bool SettleChess(bool isAuto, int pos1, int pos2)
        {

            DrawSingleChess(g, consts.Inexact(pos1, pos2), game.IsBlack);
            if (game.LastPos[0] != int.MaxValue && game.LastPos[1] != int.MaxValue)
                DrawLocation(g, consts.Inexact(game.LastPos[0], game.LastPos[1]), consts.Inexact(pos1, pos2), !game.IsBlack);
            sound_player.SoundLocation = consts.sound_location_1;
            sound_player.Play();
            game[pos1, pos2] = game.IsBlack;//��¼��Ӧλ�õ��������
            if (!game.auto || game.auto && isAuto)
            {
                string msg;
                msg = "�Է�������:\n" + pos2 + "��" + pos1 + "��\n";

                //����Ϣ׷�ӵ�д�뵽�ı��ļ���
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
                    if (!game.IsBlack)//���л����׷��غϣ����Ǻڷ��Ѿ���ʤ
                    {
                        MessageBox.Show("�ڷ���ʤ������һ��!");
                        hint.Text = "�ڷ��ѻ�ʤ\n.\n�����¿�ʼ��Ϸ";
                    }
                    else
                    {
                        MessageBox.Show("�׷���ʤ�����һ��!");
                        hint.Text = "�׷��ѻ�ʤ\n.\n�����¿�ʼ��Ϸ";
                    }
                    hint.Refresh();
                    //��ֹ��������¼�
                    game.IsStarted = false;
                    game.over = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("ƽ�֣��ƾ�����!");
                    hint.Text = "ƽ��\n.\n�����¿�ʼ��Ϸ";
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
            hint.Text = ".\n��������˼����\n.";
            hint.Refresh();
            int[] pos = game.minmax.GetAction(lastpos, game.count);
            if (SettleChess(true, pos[0], pos[1]))//���ʤ��
                game.auto = false;
            else
                game.IsStarted = true;
        }

    }
}
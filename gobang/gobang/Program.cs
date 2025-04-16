using System.Diagnostics;

namespace gobang
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form0());
        }
    }

    internal class Game
    {
        private bool isStarted;//游戏是否开始
        private bool isBlack;//是否轮到黑棋
        private const int size = Consts.board_size + 1;//棋盘大小
        private bool?[,] board;//棋盘。null表示无子，true表示黑子，false表示白子
        private int[] lastPos;
        public bool auto;//是否开启AI。AI下白棋
        public MinMax minmax;
        public int count;//落子数 
        private List<Point> retrace;//悔棋用的记录
        public bool? autoprior;//AI是否先手。0.1的概率先手
        public bool over;//游戏是否结束
        public Game()
        {
            board = new bool?[size, size];
            lastPos = new int[2] { int.MaxValue, int.MaxValue };
            count = 0;
            retrace = new List<Point>();
            over=false;
        }

        public int[] LastPos
        {
            get { return lastPos; }
            set { lastPos = value; }
        }

        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }

        public bool IsBlack
        {
            get { return isBlack; }
            set { isBlack = value; }
        }

        public bool?[,] Board
        {
            get { return board; }
        }

        //初始化游戏
        public void InitializeGame()
        {
            //bool？类型默认为null，不需要初始化;但重新开始游戏时需要初始化
            isStarted = true;
            isBlack = true;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j] = null;
            lastPos[0] = 0;
            lastPos[1] = 0;
            if (auto)
            {
                Random random = new Random();
                int rad = random.Next(0, 10);
                autoprior = false;
                if (rad == 0)
                {
                    board[size / 2, size / 2] = false;
                    autoprior = true;
                    MessageBox.Show("稀有开局！对方先手。");
                    LastPos[0] = size / 2;
                    LastPos[1] = size / 2;
                    count++;
                }
                minmax = new MinMax(autoprior);
            }
            else 
                autoprior = null;
        }

        //数组的索引器
        public bool? this[int x, int y]
        {
            get
            {
                return board[x, y];
            }
            set
            {
                board[x, y] = value;
                count++;
                lastPos[0] = x;
                lastPos[1] = y;
                Point point=new Point(x,y);
                retrace.Add(point);
                if (auto)
                {
                    minmax.SynchroMinmax(x,y,value);
                    //悔棋时不处理visitable
                    //同步修改MinMax的visitable，将刚下的位置的周围8个位置设为可访问，以便剪枝
                    minmax.visitable[x, y] = true;
                    for (int i = x - 2; i <= x + 2; i++)
                        for (int j = y - 2; j <= y + 2; j++)
                            if (i >= 0 && i < size && j >= 0 && j < size)
                                minmax.visitable[i, j] = true;
                }
            }
        }

        //悔棋逻辑
        public void Withdraw()
        {
            if (retrace.Count > 0)
            {
                isBlack = !isBlack;
                int index = retrace.Count - 1;
                Point point = retrace[index];
                board[point.X, point.Y] = null;
                if (auto)
                    minmax.SynchroMinmax(point.X, point.Y, null);
                if (index > 0)
                {
                    point = retrace[index - 1];//记录悔棋的前一步
                    LastPos[0] = point.X;
                    LastPos[1] = point.Y;
                }
                else
                {
                    LastPos[0] = int.MaxValue;
                    LastPos[1] = int.MaxValue;
                }
                count--;
                retrace.RemoveAt(index);
            }
            else if(auto&&(bool)autoprior)
            {
                LastPos[0] = size / 2;
                LastPos[1] = size / 2;
                count--;    
            }
        }

        //判断游戏是否结束
        public bool? JudgeGame(int[] accurate)
        {
            //检查以accurate为中心的横、竖、左斜、右斜四个方向是否有五个连续的棋子
            int x = accurate[0];
            int y = accurate[1];
            int count = 1;
            bool? color = board[x, y];//当前棋子颜色
            if(count==size*size)
                return null;
            //横向
            for (int i = x - 1; i >= 0; i--)
            {
                if (board[i, y] == color)
                    count++;
                else
                    break;
            }
            for (int i = x + 1; i < size; i++)
            {
                if (board[i, y] == color)
                    count++;
                else
                    break;
            }
            if (count >= 5)
                return true;
            //纵向
            count = 1;
            for (int i = y - 1; i >= 0; i--)
            {
                if (board[x, i] == color)
                    count++;
                else
                    break;
            }
            for (int i = y + 1; i < size; i++)
            {
                if (board[x, i] == color)
                    count++;
                else
                    break;
            }
            if (count >= 5)
                return true;
            //左斜
            count = 1;
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (board[i, j] == color)
                    count++;
                else
                    break;
            }
            for (int i = x + 1, j = y + 1; i < size && j < size; i++, j++)
            {
                if (board[i, j] == color)
                    count++;
                else
                    break;
            }
            if (count >= 5)
                return true;
            //右斜
            count = 1;
            for (int i = x - 1, j = y + 1; i >= 0 && j < size; i--, j++)
            {
                if (board[i, j] == color)
                    count++;
                else
                    break;
            }
            for (int i = x + 1, j = y - 1; i < size && j >= 0; i++, j--)
            {
                if (board[i, j] == color)
                    count++;
                else
                    break;
            }
            if (count >= 5)
                return true;
            return false;
        }
    }

    //极大极小搜索依赖于对整个棋局的搜索，而棋局的评估函数依赖于遍历每个行列，貌似不如直接在第一步寻找最优解
    internal class MinMax
    {
        private const int search_depth = 1;//深度大于1时速度非常慢
        private const int size = Consts.board_size + 1;//棋盘大小
        private char[,] state;
        private Search search;
        public bool[,] visitable;//记录搜索空间
        public Stack<int[]> changed;//记录改变的点
        private int[] lastpos;//记录对方最后一步的位置
        private int[] planpos;//记录AI的计划位置
        private bool autoprior;//AI是否先手
        private int count;//记录回合数
        public MinMax(bool? prior)
        {
            state = new char[size, size];
            visitable = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    state[i, j] = '_';
                    visitable[i, j] = false;
                }
            }
            if ((bool)prior)
                state[size / 2, size / 2]='x';
            search = new Search();
            changed = new Stack<int[]>();
            autoprior = (bool)prior;
            lastpos = new int[2];
            planpos = new int[2];
        }
        //数组的索引器
        public void SynchroMinmax(int x,int y,bool?value)
        {
            //同步修改MinMax的state
            switch (value)
            {
                case true:
                    state[x, y] = 'o';
                    break;
                case false:
                    state[x, y] = 'x';
                    break;
                case null:
                    state[x, y] = '_';
                    break;
            }
        }

        public int[] GetAction(int[]LostPos,int count)
        {
            int value = -int.MaxValue;
            int alpha = -int.MaxValue;
            int beta = int.MaxValue;
            int[] action = new int[2];
            search.GetSequenceAttached(state);
            search.GetAllScore();
            lastpos = LostPos;
            this.count = count;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (state[i, j] == '_' && visitable[i, j] == true)
                    {
                        int[] point = new int[] { i, j };
                        planpos=point;
                        int oringinscore = search.GetSingleScore(point, 'x');
                        search.xscore -= oringinscore;
                        state[i, j] = 'x';
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Push(point);
                        int temp = GetValue(false, 0, alpha, beta);
                        //输出位置和分数到控制台
                        Debug.WriteLine("[x,y]=" + "(" + i + "," + j + ")" + "=" + temp);
                        if (i % (size - 1) == 0)//每行输出完换行
                            Debug.WriteLine("\n\n");

                        if (temp > value)
                        {
                            value = temp;
                            action[0] = i;
                            action[1] = j;
                        }
                        state[i, j] = '_';
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Pop();
                        search.xscore += oringinscore;
                    }
                }
            }
            Debug.WriteLine("完成一步搜索，位置为" + "(" + action[0] + "," + action[1] + ")");
            return action;
        }

        private int GetValue(bool isWhite, int depth, int alpha, int beta)
        {
            if (isWhite)
            {
                depth += 1;
                if (depth == search_depth)
                {
                    return Evaluate(isWhite);
                }
                else
                    return GetMaxValue(isWhite, depth, alpha, beta);
            }
            else
                return GetMinValue(isWhite, depth, alpha, beta);
        }

        public int GetMaxValue(bool isWhite, int depth, int alpha, int beta)
        {
            int value = -int.MaxValue;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (state[i, j] == '_' && visitable[i, j] == true)
                    {
                        char piece = isWhite ? 'x' : 'o';
                        state[i, j] = piece;
                        int[] point = new int[] { i, j };
                        int oringinscore = search.GetSingleScore(point, piece);
                        search.xscore -= oringinscore;
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Push(point);
                        value = Math.Min(value, GetValue(!isWhite, depth, alpha, beta));
                        state[i, j] = '_';
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Pop();
                        search.xscore += oringinscore;
                        if (value > beta)
                            return value;
                        alpha = Math.Max(alpha, value);
                    }
                }
            }
            return value;
        }

        public int GetMinValue(bool isWhite, int depth, int alpha, int beta)
        {
            int value = int.MaxValue;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (state[i, j] == '_' && visitable[i, j] == true)
                    {
                        char piece = isWhite ? 'x' : 'o';
                        state[i, j] = piece;
                        int[] point = new int[] { i, j };
                        int oringinscore = search.GetSingleScore(point, piece);
                        search.oscore -= oringinscore;
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Push(point);
                        value = Math.Min(value, GetValue(!isWhite, depth, alpha, beta));
                        state[i, j] = '_';
                        search.UpdateLinesThroughPoint(state, i, j);
                        changed.Pop();
                        search.oscore += oringinscore;
                        if (value < alpha)
                            return value;
                        beta = Math.Min(beta, value);
                    }
                }
            }
            return value;
        }

        public int Evaluate(bool isWhite)
        {
            int newxscore = 0;
            int newoscore = 0;
            int manhattanscore = 0;
            if(count<=size)
                manhattanscore=size* Consts.ManhattanDistance(lastpos, planpos)/(count+1);
            foreach (int[] pos in changed)
            {
                newxscore += search.GetSingleScore(pos, 'x');
                newoscore += search.GetSingleScore(pos, 'o');
            }
            if(autoprior)
                return (int)(1.2*(search.xscore + newxscore) - (search.oscore + newoscore))- manhattanscore;
            else
                return (int)((search.xscore + newxscore) - 1.5*(search.oscore + newoscore))- manhattanscore;
        }
    }

    internal class Search
    {//利用字符串匹配每行列的情况。
     //使用_表示空的点位，o表示对方点位，x表示己方点位
     //private string line;//一行或一列的情况
        private int size = Consts.board_size + 1;
        private int sequence;

        //记录o的分值；字典类型
        private static Dictionary<string, int> oDictionary;

        //记录x的分值；字典类型
        private static Dictionary<string, int> xDictionary;

        //棋盘的所有序列串；字符串数组
        private string[] lines;

        public int oscore = 0; //整个棋盘中，o的分数，用户，默认为0
        public int xscore = 0; //整个棋盘中，x的分数，电脑，默认为0

        public Search()
        {
            //line = "_______________";
            oDictionary = new Dictionary<string, int>();
            xDictionary = new Dictionary<string, int>();
            sequence = size + size + (2 * size - 3 - 6) * 2;
            //lines = new string[sequence];
            //for (int i = 0; i < sequence; i++)
            //    lines[i] = line;

            //初始化oDictory，不考虑对称
            //连五
            oDictionary.Add("ooooo", 100000);
            //活四
            oDictionary.Add("_oooo_", 30000);
            //连冲四
            oDictionary.Add("_oooox", 2500);
            //跳冲四
            oDictionary.Add("_ooo_o_", 2500);
            oDictionary.Add("_oo_oo_", 2000);
            oDictionary.Add("ooo_ox", 1500);
            oDictionary.Add("oo_oox", 1500);
            oDictionary.Add("o_ooox", 1500);
            //连活三
            oDictionary.Add("_ooo__", 3500);
            //跳活三
            oDictionary.Add("_oo_o_", 2000);
            //眠三
            oDictionary.Add("__ooox", 50);
            oDictionary.Add("_o_oox", 100);
            oDictionary.Add("_oo_ox", 150);
            oDictionary.Add("_o__oo_", 200);
            oDictionary.Add("x_ooo_x", 100);
            oDictionary.Add("_o_o_o_", 200);
            //活二
            oDictionary.Add("__oo__", 100);
            oDictionary.Add("_o_o_", 50);
            oDictionary.Add("_o__o", 30);
            //眠二
            oDictionary.Add("___oox", 10);
            oDictionary.Add("__o_ox", 10);
            oDictionary.Add("_o__ox", 10);
            oDictionary.Add("_o___o", 5);
            //单子
            oDictionary.Add("____o_", 2);
            oDictionary.Add("___o__", 2);

            //初始化xDictory，不考虑对称
            //连五
            xDictionary.Add("xxxxx", 100000);
            //活四
            xDictionary.Add("_xxxx_", 30000);
            //连冲四
            xDictionary.Add("_xxxxo", 2500);
            //跳冲四
            xDictionary.Add("_xxx_x", 2500);
            xDictionary.Add("_xx_xx_", 2000);
            xDictionary.Add("xxx_ox", 1500);
            xDictionary.Add("xx_xxo", 1500);
            xDictionary.Add("x_xxxx", 1500);
            //连活三
            xDictionary.Add("_xxx__", 3500);
            //跳活三
            xDictionary.Add("_xx_x_", 2000);
            //眠三
            xDictionary.Add("__xxxo", 50);
            xDictionary.Add("_x_xxo", 100);
            xDictionary.Add("_xx_xo", 150);
            xDictionary.Add("_x__xx_", 200);
            xDictionary.Add("o_xxx_o", 100);
            xDictionary.Add("_x_x_x_", 200);
            //活二
            xDictionary.Add("__xx__", 100);
            xDictionary.Add("_x_x_", 50);
            xDictionary.Add("_x__x", 30);
            //眠二
            xDictionary.Add("___xxo", 10);
            xDictionary.Add("__x_xo", 10);
            xDictionary.Add("_x__xo", 10);
            xDictionary.Add("_x___x", 5);
            //单子
            xDictionary.Add("____x_", 2);
            xDictionary.Add("___x__", 2);
        }
        //将棋盘中的每一行列转换为字符串
        public void GetSequenceAttached(char[,] state)
        {
            //[0:size-1]为棋盘的列|
            //[size:2*size-1]为棋盘的行—
            //[2*size:2*size+2*(size-4)-2]为棋盘的右斜\，排除了边界的六条不够五子的斜线
            //[2*size+2*(size-4)-1:2*size+2*(size-4)+2*(size-4)-3]为棋盘的左斜/，排除了边界的六条不够五子的斜线

            //重置lines
            lines = new string[sequence];
            for (int i = 0; i < sequence; i++)
                lines[i] = "";

            //行列
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    lines[i] += state[i, j];//列
                    lines[j + size] += state[i, j];//行
                }
            }
            //右斜
            for (int i = 0; i < size - 4; i++)
                for (int j = 0; j < size - i; j++)
                    lines[2 * size + i] += state[i + j, j];//捺及其上方
            for (int i = 1; i < size - 4; i++)
                for (int j = i; j < size; j++)
                    lines[2 * size + size - 4 + i - 1] += state[j - i, j];//捺下方
            //左斜
            for (int i = 4; i < size; i++)
                for (int j = 0; j <= i; j++)
                    lines[2 * size + 2 * (size - 4) - 1 + i - 4] += state[i - j, j];//撇及其上方
            for (int i = 4; i < size - 1; i++)
                for (int j = 0; j <= i; j++)
                    lines[2 * size + 2 * (size - 4) - 1 + size - 4 + i - 4] += state[size - 1 - j, size - 1 - i + j];//撇下方
        }
        //获取棋盘中的某个点所在的四条序列的索引
        private int[] GetLinesThroughPoint(int i, int j)
        {
           int[] lineIndices = new int[4];

            // 横线
            lineIndices[0] = i;

            // 竖线
            lineIndices[1] = j + size;

            // 左斜线 \
            if (i - j >= size - 4 || j - i >= size - 4)
                lineIndices[2] = int.MaxValue;
            else
            {
                if (i >= j)
                    lineIndices[2] = 2 * size + i - j;
                else
                    lineIndices[2] = 2 * size + size - 4 + j - i;
            }
            // 右斜线 /
            if (i + j <= 4 || i + j >= 2 * size - 6)
                lineIndices[3] = int.MaxValue;

            else
            {
                if (i + j <= size - 1)
                    lineIndices[3] = 2 * size + 2 * (size - 4) - 1 + i + j - 4;
                else
                    lineIndices[3] = 2 * size + 2 * (size - 4) - 1 + size - 4 + 2 * size - 6 - (i + j);
            }
            return lineIndices;
        }
        //更新指定的四条序列
        public void UpdateLinesThroughPoint(char[,] state, int i, int j)
        {
            //?
            int[] lineIndices = GetLinesThroughPoint(i, j);
            int length;
            // 更新竖线
            length = lines[lineIndices[0]].Length;
            lines[lineIndices[0]] = "";
            for (int row = 0; row < length; row++)
                lines[lineIndices[0]] += state[i, row];

            // 更新横线
            length = lines[lineIndices[1]].Length;
            lines[lineIndices[1]] = "";
            for (int col = 0; col < length; col++)
                lines[lineIndices[1]] += state[col, j];
            if (lineIndices[2] != int.MaxValue)
            {
                // 更新右斜线 \
                length = lines[lineIndices[2]].Length;
                lines[lineIndices[2]] = "";
                if (i >= j)
                    for (int k = 0; k < length; k++)
                        lines[lineIndices[2]] += state[i - j + k, k];
                else
                    for (int k = 0; k < length; k++)
                        lines[lineIndices[2]] += state[j - i + k, k];
            }
            if (lineIndices[3] != int.MaxValue)
            {
                // 更新左斜线 /
                length = lines[lineIndices[3]].Length;
                lines[lineIndices[3]] = "";
                if (i + j <= size - 1)
                    for (int k = 0; k < length; k++)
                        lines[lineIndices[3]] += state[i + j - k, k];
                else
                    for (int k = 0; k < length; k++)
                        lines[lineIndices[3]] += state[size - 1 - k, i + j - size + 1 + k];
            }
        }
        //计算整个棋盘对当前棋手来说的分数
        public void GetAllScore()
        {
            oscore = 0;
            xscore = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                oscore += CalculateSingleScore(lines[i], 'o');
                xscore += CalculateSingleScore(lines[i], 'x');
            }
        }
        //计算特定点相关的分数
        public int GetSingleScore(int[] pos, char flag)
        {
            //?
            int[] lineIndices = GetLinesThroughPoint(pos[0],pos[1] );
            int score = 0;
            for (int i = 0; i < lineIndices.Length; i++)
                if (lineIndices[i] != int.MaxValue)
                    score += CalculateSingleScore(lines[lineIndices[i]], flag);
            return score;
        }
        //计算特定序列的分数
        public static int CalculateSingleScore(string sequence, char flag)
        {
            Dictionary<string, int> Dictionary;
            if (flag == 'x')
                Dictionary = xDictionary;
            else if (flag == 'o')
                Dictionary = oDictionary;
            else
                throw (new Exception("flag error"));
            int score = 0;
            if (sequence.Contains(flag))
            {
                foreach (var entry in Dictionary)
                {
                    string item = entry.Key;
                    int value = entry.Value;
                    char[] sub = item.ToCharArray();
                    Array.Reverse(sub);
                    string subitem = new string(sub);
                    int count = KMPCountOccurrences(sequence, item);
                    if (count > 0)
                        score += value * count;
                    //if (sequence.Contains(item))
                    //    score += value;
                    //if (sequence.Contains(subitem))
                    //    score += value;
                }
            }
            return score;
        }
        // KMP算法计算字符串中子串出现的次数
        private static int KMPCountOccurrences(string str, string sub)
        {
            int[] lps = ComputeLPSArray(sub);
            int count = 0;
            int i = 0;
            int j = 0;
            while (i < str.Length)
            {
                if (sub[j] == str[i])
                {
                    j++;
                    i++;
                }
                if (j == sub.Length)
                {
                    count++;
                    j = lps[j - 1];
                }
                else if (i < str.Length && sub[j] != str[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }

            // 倒置str重新计算
            char[] arr = sub.ToCharArray();
            Array.Reverse(arr);
            sub = new string(arr);
            lps = ComputeLPSArray(sub);
            i = 0;
            j = 0;
            while (i < str.Length)
            {
                if (sub[j] == str[i])
                {
                    j++;
                    i++;
                }
                if (j == sub.Length)
                {
                    count++;
                    j = lps[j - 1];
                }
                else if (i < str.Length && sub[j] != str[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }

            return count;
        }
        // 计算LPS数组
        private static int[] ComputeLPSArray(string pat)
        {
            int length = 0;
            int i = 1;
            int[] lps = new int[pat.Length];
            lps[0] = 0;

            while (i < pat.Length)
            {
                if (pat[i] == pat[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }
    }
}
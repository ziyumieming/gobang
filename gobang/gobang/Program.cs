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
        private bool isStarted;//��Ϸ�Ƿ�ʼ
        private bool isBlack;//�Ƿ��ֵ�����
        private const int size = Consts.board_size + 1;//���̴�С
        private bool?[,] board;//���̡�null��ʾ���ӣ�true��ʾ���ӣ�false��ʾ����
        private int[] lastPos;
        public bool auto;//�Ƿ���AI��AI�°���
        public MinMax minmax;
        public int count;//������ 
        private List<Point> retrace;//�����õļ�¼
        public bool? autoprior;//AI�Ƿ����֡�0.1�ĸ�������
        public bool over;//��Ϸ�Ƿ����
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

        //��ʼ����Ϸ
        public void InitializeGame()
        {
            //bool������Ĭ��Ϊnull������Ҫ��ʼ��;�����¿�ʼ��Ϸʱ��Ҫ��ʼ��
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
                    MessageBox.Show("ϡ�п��֣��Է����֡�");
                    LastPos[0] = size / 2;
                    LastPos[1] = size / 2;
                    count++;
                }
                minmax = new MinMax(autoprior);
            }
            else 
                autoprior = null;
        }

        //�����������
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
                    //����ʱ������visitable
                    //ͬ���޸�MinMax��visitable�������µ�λ�õ���Χ8��λ����Ϊ�ɷ��ʣ��Ա��֦
                    minmax.visitable[x, y] = true;
                    for (int i = x - 2; i <= x + 2; i++)
                        for (int j = y - 2; j <= y + 2; j++)
                            if (i >= 0 && i < size && j >= 0 && j < size)
                                minmax.visitable[i, j] = true;
                }
            }
        }

        //�����߼�
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
                    point = retrace[index - 1];//��¼�����ǰһ��
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

        //�ж���Ϸ�Ƿ����
        public bool? JudgeGame(int[] accurate)
        {
            //�����accurateΪ���ĵĺᡢ������б����б�ĸ������Ƿ����������������
            int x = accurate[0];
            int y = accurate[1];
            int count = 1;
            bool? color = board[x, y];//��ǰ������ɫ
            if(count==size*size)
                return null;
            //����
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
            //����
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
            //��б
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
            //��б
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

    //����С���������ڶ�������ֵ�����������ֵ��������������ڱ���ÿ�����У�ò�Ʋ���ֱ���ڵ�һ��Ѱ�����Ž�
    internal class MinMax
    {
        private const int search_depth = 1;//��ȴ���1ʱ�ٶȷǳ���
        private const int size = Consts.board_size + 1;//���̴�С
        private char[,] state;
        private Search search;
        public bool[,] visitable;//��¼�����ռ�
        public Stack<int[]> changed;//��¼�ı�ĵ�
        private int[] lastpos;//��¼�Է����һ����λ��
        private int[] planpos;//��¼AI�ļƻ�λ��
        private bool autoprior;//AI�Ƿ�����
        private int count;//��¼�غ���
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
        //�����������
        public void SynchroMinmax(int x,int y,bool?value)
        {
            //ͬ���޸�MinMax��state
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
                        //���λ�úͷ���������̨
                        Debug.WriteLine("[x,y]=" + "(" + i + "," + j + ")" + "=" + temp);
                        if (i % (size - 1) == 0)//ÿ������껻��
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
            Debug.WriteLine("���һ��������λ��Ϊ" + "(" + action[0] + "," + action[1] + ")");
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
    {//�����ַ���ƥ��ÿ���е������
     //ʹ��_��ʾ�յĵ�λ��o��ʾ�Է���λ��x��ʾ������λ
     //private string line;//һ�л�һ�е����
        private int size = Consts.board_size + 1;
        private int sequence;

        //��¼o�ķ�ֵ���ֵ�����
        private static Dictionary<string, int> oDictionary;

        //��¼x�ķ�ֵ���ֵ�����
        private static Dictionary<string, int> xDictionary;

        //���̵��������д����ַ�������
        private string[] lines;

        public int oscore = 0; //���������У�o�ķ������û���Ĭ��Ϊ0
        public int xscore = 0; //���������У�x�ķ��������ԣ�Ĭ��Ϊ0

        public Search()
        {
            //line = "_______________";
            oDictionary = new Dictionary<string, int>();
            xDictionary = new Dictionary<string, int>();
            sequence = size + size + (2 * size - 3 - 6) * 2;
            //lines = new string[sequence];
            //for (int i = 0; i < sequence; i++)
            //    lines[i] = line;

            //��ʼ��oDictory�������ǶԳ�
            //����
            oDictionary.Add("ooooo", 100000);
            //����
            oDictionary.Add("_oooo_", 30000);
            //������
            oDictionary.Add("_oooox", 2500);
            //������
            oDictionary.Add("_ooo_o_", 2500);
            oDictionary.Add("_oo_oo_", 2000);
            oDictionary.Add("ooo_ox", 1500);
            oDictionary.Add("oo_oox", 1500);
            oDictionary.Add("o_ooox", 1500);
            //������
            oDictionary.Add("_ooo__", 3500);
            //������
            oDictionary.Add("_oo_o_", 2000);
            //����
            oDictionary.Add("__ooox", 50);
            oDictionary.Add("_o_oox", 100);
            oDictionary.Add("_oo_ox", 150);
            oDictionary.Add("_o__oo_", 200);
            oDictionary.Add("x_ooo_x", 100);
            oDictionary.Add("_o_o_o_", 200);
            //���
            oDictionary.Add("__oo__", 100);
            oDictionary.Add("_o_o_", 50);
            oDictionary.Add("_o__o", 30);
            //�߶�
            oDictionary.Add("___oox", 10);
            oDictionary.Add("__o_ox", 10);
            oDictionary.Add("_o__ox", 10);
            oDictionary.Add("_o___o", 5);
            //����
            oDictionary.Add("____o_", 2);
            oDictionary.Add("___o__", 2);

            //��ʼ��xDictory�������ǶԳ�
            //����
            xDictionary.Add("xxxxx", 100000);
            //����
            xDictionary.Add("_xxxx_", 30000);
            //������
            xDictionary.Add("_xxxxo", 2500);
            //������
            xDictionary.Add("_xxx_x", 2500);
            xDictionary.Add("_xx_xx_", 2000);
            xDictionary.Add("xxx_ox", 1500);
            xDictionary.Add("xx_xxo", 1500);
            xDictionary.Add("x_xxxx", 1500);
            //������
            xDictionary.Add("_xxx__", 3500);
            //������
            xDictionary.Add("_xx_x_", 2000);
            //����
            xDictionary.Add("__xxxo", 50);
            xDictionary.Add("_x_xxo", 100);
            xDictionary.Add("_xx_xo", 150);
            xDictionary.Add("_x__xx_", 200);
            xDictionary.Add("o_xxx_o", 100);
            xDictionary.Add("_x_x_x_", 200);
            //���
            xDictionary.Add("__xx__", 100);
            xDictionary.Add("_x_x_", 50);
            xDictionary.Add("_x__x", 30);
            //�߶�
            xDictionary.Add("___xxo", 10);
            xDictionary.Add("__x_xo", 10);
            xDictionary.Add("_x__xo", 10);
            xDictionary.Add("_x___x", 5);
            //����
            xDictionary.Add("____x_", 2);
            xDictionary.Add("___x__", 2);
        }
        //�������е�ÿһ����ת��Ϊ�ַ���
        public void GetSequenceAttached(char[,] state)
        {
            //[0:size-1]Ϊ���̵���|
            //[size:2*size-1]Ϊ���̵��С�
            //[2*size:2*size+2*(size-4)-2]Ϊ���̵���б\���ų��˱߽�������������ӵ�б��
            //[2*size+2*(size-4)-1:2*size+2*(size-4)+2*(size-4)-3]Ϊ���̵���б/���ų��˱߽�������������ӵ�б��

            //����lines
            lines = new string[sequence];
            for (int i = 0; i < sequence; i++)
                lines[i] = "";

            //����
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    lines[i] += state[i, j];//��
                    lines[j + size] += state[i, j];//��
                }
            }
            //��б
            for (int i = 0; i < size - 4; i++)
                for (int j = 0; j < size - i; j++)
                    lines[2 * size + i] += state[i + j, j];//�༰���Ϸ�
            for (int i = 1; i < size - 4; i++)
                for (int j = i; j < size; j++)
                    lines[2 * size + size - 4 + i - 1] += state[j - i, j];//���·�
            //��б
            for (int i = 4; i < size; i++)
                for (int j = 0; j <= i; j++)
                    lines[2 * size + 2 * (size - 4) - 1 + i - 4] += state[i - j, j];//Ʋ�����Ϸ�
            for (int i = 4; i < size - 1; i++)
                for (int j = 0; j <= i; j++)
                    lines[2 * size + 2 * (size - 4) - 1 + size - 4 + i - 4] += state[size - 1 - j, size - 1 - i + j];//Ʋ�·�
        }
        //��ȡ�����е�ĳ�������ڵ��������е�����
        private int[] GetLinesThroughPoint(int i, int j)
        {
           int[] lineIndices = new int[4];

            // ����
            lineIndices[0] = i;

            // ����
            lineIndices[1] = j + size;

            // ��б�� \
            if (i - j >= size - 4 || j - i >= size - 4)
                lineIndices[2] = int.MaxValue;
            else
            {
                if (i >= j)
                    lineIndices[2] = 2 * size + i - j;
                else
                    lineIndices[2] = 2 * size + size - 4 + j - i;
            }
            // ��б�� /
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
        //����ָ������������
        public void UpdateLinesThroughPoint(char[,] state, int i, int j)
        {
            //?
            int[] lineIndices = GetLinesThroughPoint(i, j);
            int length;
            // ��������
            length = lines[lineIndices[0]].Length;
            lines[lineIndices[0]] = "";
            for (int row = 0; row < length; row++)
                lines[lineIndices[0]] += state[i, row];

            // ���º���
            length = lines[lineIndices[1]].Length;
            lines[lineIndices[1]] = "";
            for (int col = 0; col < length; col++)
                lines[lineIndices[1]] += state[col, j];
            if (lineIndices[2] != int.MaxValue)
            {
                // ������б�� \
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
                // ������б�� /
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
        //�����������̶Ե�ǰ������˵�ķ���
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
        //�����ض�����صķ���
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
        //�����ض����еķ���
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
        // KMP�㷨�����ַ������Ӵ����ֵĴ���
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

            // ����str���¼���
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
        // ����LPS����
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
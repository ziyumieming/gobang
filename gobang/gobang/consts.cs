namespace gobang
{
    internal class Consts//存储常量参数
    {
        //静态成员无需实例化
        public int window_width;
        public int window_height;
        public int board_width;
        public int board_height;
        public const int board_size = 14;
        public int margin;
        public int semimargin;
        public int grid_width;
        public int grid_height;
        public int piece_size;
        public int location_mark;
        public string file_path;
        public string sound_location_1;
        public string sound_location_2;
        public string sound_location_3;
        public string sound_location_4;
        public Consts(int width, int height, int b_width, int b_height)
        {
            window_width = width;
            window_height = height;
            board_height = b_height;
            board_width = board_height;
            margin = (int)(height * 0.045);
            semimargin = margin / 2;
            grid_width = (board_width - margin) / board_size;
            grid_height = (board_height - margin) / board_size;
            piece_size = (int)(grid_width * 0.8);
            location_mark = (int)(height * 0.018);

            file_path = AppDomain.CurrentDomain.BaseDirectory;
            sound_location_1 = System.IO.Path.Combine(file_path, "Properties", "落子声.wav");
            sound_location_2 = System.IO.Path.Combine(file_path, "Properties", "结束声.wav");
            sound_location_3 = System.IO.Path.Combine(file_path, "Properties", "提示音.wav");
            sound_location_4 = System.IO.Path.Combine(file_path, "Properties", "开局音效.wav");
        }
        //将点击的范围转换为数组下标
        public int?[] Accurate(Point click_position)
        {
            int? a = (click_position.X - semimargin) / grid_width;
            int? b = (click_position.Y - semimargin) / grid_height;
            int offset_x = (click_position.X - semimargin) % grid_width;
            int offset_y = (click_position.Y - semimargin) % grid_height;
            if (offset_x > grid_width / 2)
            {
                a++;
            }
            if (offset_y > grid_height / 2)
            {
                b++;
            }
            if (a < 0 || a > board_size)
                a = null;
            if (b < 0 || b > board_size)
                b = null;
            return new int?[] { a, b };
        }

        //将给定的数组下标和固定值转换为网格中心点坐标
        public Point Inexact(int a, int b)
        {
            int x = margin + a * grid_width;
            int y = margin + b * grid_height;
            Point point = new Point(x, y);
            return point;
        }

        //计算两点之间的曼哈顿距离
        public static int ManhattanDistance(int[] pos1, int[] pos2)
        {
            return Math.Abs(pos1[0] - pos2[0]) + Math.Abs(pos1[1] - pos2[1]);
        }

        //向上寻找项目根目录
        private static string FindProjectDirectory(string startDirectory, string targetDirectoryName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(startDirectory);

            while (directoryInfo != null && directoryInfo.Exists)
            {
                if (directoryInfo.Name.Equals(targetDirectoryName, StringComparison.OrdinalIgnoreCase))
                {
                    return directoryInfo.FullName;
                }
                directoryInfo = directoryInfo.Parent;
            }
            return null;
        }
    }
}
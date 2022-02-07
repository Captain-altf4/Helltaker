using System;
using System.Threading;

namespace hw2
{
    enum Dir
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        STOP
    }
    class Program
    {
        static bool gameOver = false;
        static bool levelComplete = false;
        static bool gameClosing = false;
        static int score = 0;

        static int width = 20;
        static int height = 20;

        static int xPlayerPos = width / 2;
        static int yPlayerPos = height / 2;

        static int xFinishPoint = 0;
        static int yFinishPoint = 0;

        static int xBlockPos = 0;
        static int yBlockPos = 0;

        static Dir dir = Dir.STOP;

        static void getRandCoord()
        {
            Random ran = new Random();
            xFinishPoint = ran.Next(1, width - 1);
            yFinishPoint = ran.Next(1, height - 1);
            xBlockPos = ran.Next(2, width - 2);
            yBlockPos = ran.Next(2, height - 2);
            if (xFinishPoint == xPlayerPos && yFinishPoint == yPlayerPos ||
                xBlockPos == xPlayerPos && yBlockPos == yPlayerPos)
            {
                getRandCoord();
            }
        }

        static void draw()
        {
            Console.SetCursorPosition(0, 0); // Перемещение курсора на начальную позицию

            // Отрисовка поля
            for (int x = 0; x < width; x++)
            {
                Console.Write("#");
            }

            for (int y = 1; y < height - 1; y++)
            {
                Console.Write("\n#");
                for (int x = 1; x < width - 1; x++)
                {
                    bool cellEmpty = true;
                    if (xPlayerPos == x && yPlayerPos == y)
                    {
                        Console.Write("O");
                        cellEmpty = false;
                    }
                    if(xFinishPoint == x && yFinishPoint == y && cellEmpty == true)
                    {
                        Console.Write("X");
                        cellEmpty = false;
                    }
                    if(xBlockPos == x && yBlockPos == y && cellEmpty == true)
                    {
                        Console.Write("B");
                        cellEmpty = false;
                    }
                    if (cellEmpty == true)
                    {
                         Console.Write(" ");
                    }
                }
                Console.Write("#");
            }

            Console.Write("\n");
            for (int x = 0; x < width; x++)
            {
                Console.Write("#");
            }
        }

        static void input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case (ConsoleKey.W):
                        dir = Dir.UP;
                        break;
                    case (ConsoleKey.S):
                        dir = Dir.DOWN;
                        break;
                    case (ConsoleKey.A):
                        dir = Dir.LEFT;
                        break;
                    case (ConsoleKey.D):
                        dir = Dir.RIGHT;
                        break;
                }
            }
        }

        static void logic()
        {
            if (xBlockPos == xPlayerPos + 1 && dir == Dir.LEFT)
            {
                xBlockPos ++;
            }
            else if (xBlockPos == xPlayerPos - 1 && dir == Dir.RIGHT)
            {
                xBlockPos--;
            }


            switch (dir)
            {
                case (Dir.UP):
                    if (yPlayerPos > 1)
                    {
                        if(yBlockPos == yPlayerPos - 1)
                        {
                            yBlockPos--;
                        }
                        yPlayerPos--;
                    }
                    dir = Dir.STOP;
                    break;
                case (Dir.DOWN):
                    if(yPlayerPos <= height - 3)
                    {
                        yPlayerPos++;
                    }
                    dir = Dir.STOP;
                    break;
                case (Dir.LEFT):
                    if(xPlayerPos > 1)
                    {
                        xPlayerPos--;
                    }
                    dir = Dir.STOP;
                    break;
                case (Dir.RIGHT):
                    if (xPlayerPos <= width - 3)
                    {
                        xPlayerPos++;
                    }
                    dir = Dir.STOP;
                    break;
            }
        }

        static void Main(string[] args)
        {
            /*getRandCoord();
            Console.CursorVisible = false;
            while (true)
            {
                Thread.Sleep(150);
                draw();
                input();
                logic();
            }*/
            while (gameClosing == false)
            {
                while (gameOver == false)
                {
                    Thread.Sleep(150);
                    input();
                    logic();
                    draw();
                }
                Console.Clear();
                //init();
            }
        }
    }
}

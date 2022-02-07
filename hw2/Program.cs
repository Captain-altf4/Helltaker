using System;
using System.Collections.Generic;
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
        static bool gameClosing = false;
        static bool levelComplete = false;
        static int score = 0;
        static int bestScore = 0;

        static int width = 20;
        static int height = 20;
        static int xPlayerPos = width / 2;
        static int yPlayerPos = height / 2;
        static int xFinishPoint = 0;
        static int yFinishPoint = 0;
        static int xBlockPos = 0;
        static int yBlockPos = 0;
        static Dir dir = Dir.STOP;

        static int wallSize = 4;
        static int[] xWall = new int[wallSize];
        static int[] yWall = new int[wallSize];

        static void wall1(int[] xWall, int[] yWall)
        {
            Random ran = new Random();
            wallSize = 4;
            xWall[0] = ran.Next(1, width - 2);
            xWall[1] = xWall[0] + 1;
            xWall[2] = xWall[0];
            xWall[3] = xWall[0];

            yWall[0] = ran.Next(1, height - 2);
            yWall[1] = yWall[0];
            yWall[2] = yWall[0] + 1;
            yWall[3] = yWall[0] + 2;
        }

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

        static void init()
        {
            if(score > bestScore)
            {
                bestScore = score;
            }
            if(gameOver == true)
            {
                score = 0;
            }
            xPlayerPos = width / 2;
            yPlayerPos = height / 2;
            levelComplete = false;
            gameOver = false;
            Console.Clear();
            getRandCoord();
        }

        static void draw()
        {
            Console.SetCursorPosition(0, 0); // Перемещение курсора на начальную позицию
            if (levelComplete == true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write("Уровень пройден!");
                Console.ReadKey();
                init();
            }
            else if (gameOver == true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write("Вы проиграли!");
                Console.ReadKey();
                init();
            }
            // Отрисовка поля
            for (int x = 0; x < width; x++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("#");
            }

            int el = 0;
            for (int y = 1; y < height - 1; y++)
            {
                Console.Write("\n#");
                for (int x = 1; x < width - 1; x++)
                {
                    bool cellEmpty = true;
                    
                    if(el < wallSize && x == xWall[el] && y == yWall[el])
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("#");
                        cellEmpty = false;
                        el++;
                    }
                    if(xPlayerPos == x && yPlayerPos == y && cellEmpty == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("O");
                        cellEmpty = false;
                    }
                    if(xFinishPoint == x && yFinishPoint == y && cellEmpty == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        cellEmpty = false;
                    }
                    if(xBlockPos == x && yBlockPos == y && cellEmpty == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("@");
                        cellEmpty = false;
                    }
                    if (cellEmpty == true)
                    {
                         Console.Write(" ");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("#");
            }

            Console.Write("\n");
            for (int x = 0; x < width; x++)
            {
                Console.Write("#");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\nСчёт - {score}");
            Console.Write($"\nЛучший результат - {bestScore}\n");
            for (int i = 0; i < wallSize; i++)
            {
                Console.Write($"{xWall[i]}" + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < wallSize; i++)
            {
                Console.Write($"{yWall[i]}" + " ");
            }
            Console.WriteLine();
            Console.WriteLine(xPlayerPos);
            Console.WriteLine(yPlayerPos);
        }

        static void input()
        {
            if(Console.KeyAvailable)
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
                    case (ConsoleKey.R):
                        init();
                        break;
                    case (ConsoleKey.Escape):
                        gameClosing = true;
                        break;
                }
            }
        }

        static void logic()
        {
            // Логка перемещения
            switch (dir)
            {
                // Движение вверх
                case (Dir.UP):
                    if(yPlayerPos - 1 > 0)
                    {
                        if(yBlockPos == yPlayerPos - 1 && xBlockPos == xPlayerPos)
                        {
                            yBlockPos--;
                        }
                        bool canMove = true;
                        for (int i = 0; i < wallSize; i++)
                        {
                            if (yPlayerPos - 1 == yWall[i] && xPlayerPos == xWall[i])
                            {
                                canMove = false;
                                break;
                            }
                        }
                        if(canMove == true)
                        {
                            yPlayerPos--;
                        }
                    }

                    if(score > 0)
                    {
                        score--;
                    }
                    dir = Dir.STOP;
                    break;

                // Движение вниз
                case (Dir.DOWN):
                    if(yPlayerPos + 1 < height - 1)
                    {
                        if(yBlockPos == yPlayerPos + 1 && xBlockPos == xPlayerPos)
                        {
                            yBlockPos ++;
                        }
                        else
                        {
                            yPlayerPos++;
                        }
                    }

                    if (score > 0)
                    {
                        score--;
                    }
                    dir = Dir.STOP;
                    break;

                // Движение влево
                case (Dir.LEFT):
                    if(xPlayerPos - 1 > 0)
                    {
                        if(xBlockPos == xPlayerPos - 1 && yBlockPos == yPlayerPos)
                        {
                            xBlockPos--;
                        }
                        else
                        {
                            xPlayerPos--;
                        }
                    }

                    if (score > 0)
                    {
                        score--;
                    }
                    dir = Dir.STOP;
                    break;

                // Движение вправо
                case (Dir.RIGHT):
                    if(xPlayerPos + 1 < width - 1)
                    {
                        if(xBlockPos == xPlayerPos + 1 && yBlockPos == yPlayerPos)
                        {
                            xBlockPos++;
                        }
                        else
                        {
                            xPlayerPos++;
                        }
                    }

                    if(score > 0)
                    {
                        score--;
                    }
                    dir = Dir.STOP;
                    break;
            }

            // Логика поражения
            if(xBlockPos <= 1 && yBlockPos <= 1 ||
                xBlockPos <= 1 && yBlockPos >= height - 2 ||
                xBlockPos >= width - 2 && yBlockPos <= 1 ||
                xBlockPos >= width - 2 && yBlockPos >= height - 2)
            {
                gameOver = true;
            }

            // Логика победы
            if (xBlockPos == xFinishPoint && yBlockPos == yFinishPoint)
            {
                levelComplete = true;
                score += 100;
            }
        }

        static void Main(string[] args)
        {
            getRandCoord();
            wall1(xWall, yWall);
            Console.CursorVisible = false;
            while (gameClosing == false)
            {
                while (gameOver == false && gameClosing == false)
                {
                    input();
                    logic();
                    draw();
                }
                Console.Clear();
                init();
            }            
        }
    }
}



namespace SnakeGame
{
    class Program
    {
        static int mapWidth = 30;
        static int mapHeight = 15;
        static int snakeX, snakeY;
        static int foodX, foodY;
        static int score;
        static bool gameOver;
        static List<int> tailX = new List<int>();
        static List<int> tailY = new List<int>();
        static int tailLength;

        enum Direction
        {
            STOP,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        static Direction snakeDirection = Direction.STOP;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            SetupGame();

            Thread inputThread = new Thread(GetUserInput);
            inputThread.Start();

            while (!gameOver)
            {
                Draw();
                MoveSnake();
                CheckCollision();
                Thread.Sleep(300); 
            }

            Console.SetCursorPosition(mapWidth / 2 - 5, mapHeight / 2);
            Console.WriteLine("Game Over. Score: " + score);
            Console.SetCursorPosition(mapWidth / 2 - 10, mapHeight / 2 + 1);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void SetupGame()
        {
            snakeX = mapWidth / 2;
            snakeY = mapHeight / 2;
            GenerateFood();
            gameOver = false;
        }

        static void GetUserInput()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (snakeDirection != Direction.RIGHT)
                                snakeDirection = Direction.LEFT;
                            break;
                        case ConsoleKey.RightArrow:
                            if (snakeDirection != Direction.LEFT)
                                snakeDirection = Direction.RIGHT;
                            break;
                        case ConsoleKey.UpArrow:
                            if (snakeDirection != Direction.DOWN)
                                snakeDirection = Direction.UP;
                            break;
                        case ConsoleKey.DownArrow:
                            if (snakeDirection != Direction.UP)
                                snakeDirection = Direction.DOWN;
                            break;
                    }
                }
            }
        }

        static void Draw()
        {
            Console.Clear();

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (i == 0 || i == mapHeight - 1 || j == 0 || j == mapWidth - 1)
                    {
                        Console.Write("#");
                    }
                    else if (i == snakeY && j == snakeX)
                    {
                        Console.Write("O");
                    }
                    else if (i == foodY && j == foodX)
                    {
                        Console.Write("F");
                    }
                    else
                    {
                        bool printTail = false;
                        for (int k = 0; k < tailLength; k++)
                        {
                            if (tailX[k] == j && tailY[k] == i)
                            {
                                Console.Write("o");
                                printTail = true;
                            }
                        }
                        if (!printTail)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }




        static void MoveSnake()
        {
            int prevX = tailX.Count > 0 ? tailX[0] : snakeX;
            int prevY = tailY.Count > 0 ? tailY[0] : snakeY;
            int prev2X, prev2Y;

            if (tailLength > 0)
            {
                tailX[0] = snakeX;
                tailY[0] = snakeY;

                for (int i = 1; i < tailLength; i++)
                {
                    prev2X = tailX[i];
                    prev2Y = tailY[i];
                    tailX[i] = prevX;
                    tailY[i] = prevY;
                    prevX = prev2X;
                    prevY = prev2Y;
                }
            }

            switch (snakeDirection)
            {
                case Direction.LEFT:
                    snakeX--;
                    break;
                case Direction.RIGHT:
                    snakeX++;
                    break;
                case Direction.UP:
                    snakeY--;
                    break;
                case Direction.DOWN:
                    snakeY++;
                    break;
            }
        }

        static void CheckCollision()
        {
            if (snakeX == 0 || snakeX == mapWidth - 1 || snakeY == 0 || snakeY == mapHeight - 1)
            {
                gameOver = true;
            }

            for (int i = 1; i < tailLength; i++)
            {
                if (snakeX == tailX[i] && snakeY == tailY[i])
                {
                    gameOver = true;
                }
            }

            if (snakeX == foodX && snakeY == foodY)
            {
                score += 10;
                tailLength++;
                tailX.Add(snakeX); // Добавляем новый сегмент хвоста
                tailY.Add(snakeY);
                GenerateFood();
            }
        }

        static void GenerateFood()
        {
            Random rnd = new Random();
            foodX = rnd.Next(1, mapWidth - 1);
            foodY = rnd.Next(1, mapHeight - 1);
        }
    }
}

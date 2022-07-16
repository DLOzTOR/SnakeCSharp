using System;
using Snake;

namespace Program
{
    class Program
    {
        
        static void Main(string[] args)
        {
            SnakeGame snake = new SnakeGame();
            snake.Run(7,7,2);
        }
    }
}

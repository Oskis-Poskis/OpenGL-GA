﻿namespace OpenTK_Learning
{
    class Program
    {
        static void Main()
        {
            using Main game = new Main(1800, 900, "OpenGL - GA");
            game.Run();
        }
    }
}
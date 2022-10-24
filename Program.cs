using System;

namespace OpenTK_Learning
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(2200, 1200, "OpenGL - GA");
            game.Run();
        }
    }
}
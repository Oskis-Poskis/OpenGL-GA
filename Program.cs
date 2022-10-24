using System;

namespace OpenTK_Learning
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(1800, 950, "OpenGL - GA");
            game.Run();
        }
    }
}
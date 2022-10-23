using System;

namespace OpenTK_Learning
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(2500, 1250, "OpenGL - GA");
            game.Run();
        }
    }
}
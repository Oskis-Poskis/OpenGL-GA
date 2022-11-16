using System;

namespace Engine
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(1920, 1080, "Axyz");
            game.Run();
        }
    }
}
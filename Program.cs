using System;

namespace Engine
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(2560, 1440, "Axyz");
            game.Run();
        }
    }
}
using System;

namespace Axyz
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
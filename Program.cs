using System;

namespace OpenTK_Learning
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
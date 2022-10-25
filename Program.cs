using System;

namespace OpenTK_Learning
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            using Main game = new Main(2400, 1300, "Axyz");
            game.Run();
        }
    }
}
namespace OpenTK_Learning
{
    class Program
    {
        static void Main()
        {
            using Main game = new Main(2560, 1440, "OpenGL - GA");

            game.Run();
        }
    }
}
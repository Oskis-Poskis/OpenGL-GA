namespace OpenTK_Learning
{
    class Program
    {
        static void Main()
        {
            using Main game = new Main(1700, 900, "OpenGL - GA");

            game.Run();
        }
    }
}
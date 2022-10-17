namespace OpenTK_Learning
{
    class Program
    {
        static void Main()
        {
            using Game game = new Game(1920, 1080, "OpenGL - GA");

            game.Run();
        }
    }
}
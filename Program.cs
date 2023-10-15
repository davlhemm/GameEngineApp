
using GameEngineApp.Engine;
using System.Diagnostics;

namespace GameEngineApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameEngine gameEngine = new GameEngine(
                new VectorTwo(512,512), 
                "Game Test", 
                new Renderer(),
                GameLoop.Instance);
            Debug.WriteLine("Beyond engine scope");
        }
    }
}


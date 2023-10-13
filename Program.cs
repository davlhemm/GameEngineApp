
using GameEngineApp.Engine;
using System.Diagnostics;

namespace GameEngineApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            GameEngine gameEngine = new GameEngine(
                new ScreenEng(512,512), 
                "Game Test", 
                new Renderer(),
                new GameLoop());
            Debug.WriteLine("Beyond engine scope");
        }
    }
}


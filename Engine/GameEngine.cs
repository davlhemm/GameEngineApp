using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class GameEngine : EngineBase
    {
        private GameEngine() : base() { }

        public GameEngine(VectorTwo screen, string? title, IRenderer renderer, IGameLoop gameLoop)
            : base(screen, title, renderer, gameLoop) { }

        public override void OnLoad()
        {
            //New up a shape, registration already handled...
            Shape2D aShape = new Shape2D(
                new VectorTwo(16, 16),
                new VectorTwo(16, 16),
                "Shape");
        }

        public override void OnUpdate()
        {
            //TODO: Apply physics...update object locations
            foreach (var shape in EngineBase._shapes)
            {
                //Move a percentage of X blocks downward until floor reached (screen height)
                Debug.WriteLine("Y: " + shape?.Position?.Y);
                Debug.WriteLine("Canvas Height: " + canvas.Size.Height);
                //TODO: Figure out inconsistent wall boundary
                if ((shape?.Position?.Y + 2 * shape?.Scale?.Y) < (canvas.Size.Height - 2 * shape?.Scale?.Y))
                {
                    //TODO: Gravity, speed(s), direction(s)
                    shape!.Position!.Y += GameLoop.Instance.DeltaTime.Milliseconds/10;
                }
            }
        }
    }
}

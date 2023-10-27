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
        Shape2D player = null!;
        float playerSpeed = 0.2f;

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
            player = new Shape2D(
                new VectorTwo(64, 16),
                new VectorTwo(16, 16),
                "Player");
            SetFramerate(240f);
        }

        public override void OnUpdate()
        {
            if(player != null)
            {

                if(inputManager != null)
                {
                    if (inputManager.up)
                    {
                        player!.Position!.Y -= playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.down)
                    {
                        player!.Position!.Y += playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.left)
                    {
                        player!.Position!.X -= playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.right)
                    {
                        player!.Position!.X += playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                }
            }

            //TODO: deltaTime/2 trick for frame-rate independent update consistency
            //TODO: Apply physics...update object locations
            foreach (var shape in EngineBase._shapes)
            {
                //Move a percentage of X blocks downward until floor reached (screen height)
                //TODO: Figure out inconsistent wall boundary
                if ((shape?.Position?.Y + 2 * shape?.Scale?.Y) < (_canvas.Size.Height - 2 * shape?.Scale?.Y))
                {
                    //TODO: Gravity, speed(s), direction(s)
                    shape!.Position!.Y += 0.1f*(float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                }
                else
                {
                    Debug.WriteLine("End simulation time: " +
                        (GameLoop.Instance.Frames / GameLoop.FrameRate) + " seconds");
                    //Application.Exit();
                }
            }
        }
    }
}

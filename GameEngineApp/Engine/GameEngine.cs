using GameEngineApp.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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
        Map map = new Map();

        private GameEngine() : base() { }

        public GameEngine(IVectorTwo<float> screen, string? title, IRenderer renderer, IGameLoop gameLoop)
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
            int mapChunkWidth = 32, mapChunkHeight = 32;
            for(int i = 0; i < map.Tiles.GetLength(0); i++)
            {
                for(int j = 0; j < map.Tiles.GetLength(1); j++)
                {
                    if (map.Tiles[i,j] != "")
                    {
                        new Shape2D(
                            new VectorTwo(mapChunkWidth * j, mapChunkHeight * i),
                            new VectorTwo(mapChunkWidth, mapChunkHeight), 
                            "Map");
                    }
                    Logger.Info(String.Format("Map tile at [{1},{2}]: {0}", map.Tiles[i,j],i,j));
                }
            }
        }

        /// <summary>
        /// Run any update needed based on deltaTime
        /// TODO: Entity Component System for composition update(s) instead of piecemeal here...
        /// </summary>
        public override void OnUpdate()
        {
            if(player != null)
            {
                if(inputManager != null)
                {
                    if (inputManager.Up)
                    {
                        player!.Position!.Y -= playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.Down)
                    {
                        player!.Position!.Y += playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.Left)
                    {
                        player!.Position!.X -= playerSpeed * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                    if (inputManager.Right)
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
                    if(shape?.Key != "Map")
                    {
                        shape!.Position!.Y += 0.1f * (float)GameLoop.Instance.DeltaTime.TotalMilliseconds;
                    }
                }
                else
                {
                    //Debug.WriteLine("End simulation time: " +
                    //    (GameLoop.Instance.Frames / GameLoop.FrameRate) + " seconds");
                    //Application.Exit();
                }
            }
        }
    }
}

using GameEngineApp.Tools;

namespace GameEngineApp.Engine
{
    public class GameEngine : EngineBase
    {
        Shape2D player = null!;
        public static float playerSpeed = 0.2f;
        Map map = new Map();

        private GameEngine() : base() { }

        public GameEngine(IVectorTwo<float> screen, string? title, IRenderer renderer, IGameLoop gameLoop)
            : base(screen, title, renderer, gameLoop) { }

        public override void OnLoad()
        {
            var playerSize = (baseSize, baseSize);
            //New up a shape
            // TODO: Maybe inject shape/entity system to keep track of registration instead
            player = new Image2D(
                new VectorTwo(64, playerSize.Item2),
                new VectorTwo(playerSize.Item1, playerSize.Item2),
                "Player",
                "Images/player.jpg");
            player.RegisterShape(ref _shapes);
            int mapChunkWidth = playerSize.Item1, mapChunkHeight = playerSize.Item2;
            for(int i = 0; i < map.Tiles.GetLength(0); i++)
            {
                for(int j = 0; j < map.Tiles.GetLength(1); j++)
                {
                    if (map.Tiles[i,j] != "")
                    {
                        var newShape = new Shape2D(
                            new VectorTwo(mapChunkWidth * j, mapChunkHeight * i),
                            new VectorTwo(mapChunkWidth, mapChunkHeight), 
                            "Map");
                        newShape.RegisterShape(ref _shapes);
                    }
                    //Logger.Info($"Map tile at [{i},{j}]: {map.Tiles[i, j]}");
                }
            }
            base.OnLoad();
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
                    player.Update(inputManager);
                }
            }

            //TODO: deltaTime/2 trick for frame-rate independent update consistency
            //TODO: This is the physics system.  Apply physics...update object locations
            foreach (var shape in _shapes)
            {
                // TODO: System based update that gets called in groups for entities registered within each category
                // shape.Update(inputManager!);
                //Move a percentage of X blocks downward until floor reached (screen height)
                //TODO: Figure out inconsistent wall boundary
                if ((shape?.Position?.Y + 2 * shape?.Scale?.Y) < (_canvas.Size.Height - 2 * shape?.Scale?.Y))
                {
                    //TODO: Gravity, speed(s), direction(s)
                    //TODO: Entity tagging for physics system
                    if(shape?.Key != nameof(Map))
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public abstract class EngineBase : IEngine
    {
        //Include any base game/engine details
        private VectorTwo screen = new VectorTwo(512,512);
        private string? Title = "Base Title";
        private Canvas canvas = null!;
        private IRenderer renderer = null!;
        private IGameLoop gameLoop = null!;
        public static List<Shape2D> shapes = new List<Shape2D>();

        protected EngineBase() { }

        public EngineBase(VectorTwo screen, string? title, IRenderer renderer, IGameLoop gameLoop)
        {
            this.screen = screen;
            Title = title;
            this.renderer = renderer;
            this.gameLoop = gameLoop;

            canvas = new Canvas();
            canvas.Size = new Size((int)screen.X, (int)screen.Y);
            canvas.Text = title;
            //TODO: Intercept this renderer callback and let it know what to draw...
            //Consider static/global entity management?
            canvas.Paint += Render;
            canvas.FormClosing += StopLoop;
            
            //Thread already active, just point the loop hooks to the right place
            gameLoop.GameRedraw += canvas.Refresh;
            gameLoop.GameLooped += this.OnDrawCallback;
            gameLoop.GameUpdate += this.OnUpdateCallback;

            OnLoad();
            Start();

            Application.Run(canvas);
        }

        private void Render(object? sender, PaintEventArgs e)
        {
            renderer.Render(sender, e);
        }

        private void StopLoop(object? sender, FormClosingEventArgs e)
        {
            Stop();
        }

        public virtual void Stop()
        {
            gameLoop.Stop();
        }

        /// <summary>
        /// Load whatever bullshit occurs before gameloop
        /// Asset management, etc.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnLoad()
        {
            Debug.WriteLine("OnLoad()");
            //New up a shape, registration already handled...
            Shape2D aShape = new Shape2D(
                new VectorTwo(16,16), 
                new VectorTwo(16,16),
                "Shape");
        }

        public virtual void Start()
        {
            gameLoop.Start();
        }

        public virtual void OnDraw()
        {
            Debug.WriteLine("OnDraw()");
        }

        public virtual void OnUpdate()
        {
            //Debug.WriteLine("OnUpdate()");
            //TODO: Apply physics...update object locations
            foreach(var shape in EngineBase.shapes)
            {
                //Move a percentage of X blocks downward until floor reached (screen height)
                Debug.WriteLine("Y: "+shape.Position.Y);
                Debug.WriteLine("Canvas Height: " + canvas.Size.Height);
                //TODO: Figure out inconsistent wall boundary
                if (shape.Position.Y < (canvas.Size.Height-shape.Scale.Y-64))
                {
                    shape.Position.Y += 3;
                }
            }
        }

        public void OnDrawCallback(object? sender, GameLoopedEventArgs e)
        {
            OnDraw();
        }

        public void OnUpdateCallback(object? sender, EventArgs e)
        {
            OnUpdate();
        }

        public static void RegisterEntity(Shape2D shape)
        {
            shapes.Add(shape);
        }

        public static void UnregisterEntity(Shape2D shape)
        {
            shapes.Add(shape);
        }
    }

    public interface IEngine
    {
        void Stop();
        void OnLoad();
        void Start();
        void OnDraw();
        void OnUpdate();
    }
}

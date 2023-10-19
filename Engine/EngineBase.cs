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
        protected Canvas canvas = null!;
        protected IRenderer _renderer = null!;
        protected IGameLoop _gameLoop = null!;
        public static List<Shape2D> _shapes = new List<Shape2D>();

        protected EngineBase() { }

        public EngineBase(VectorTwo screen, string? title, IRenderer renderer, IGameLoop gameLoop)
        {
            this.screen = screen;
            Title = title;
            this._renderer = renderer;
            this._gameLoop = gameLoop;

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
            _renderer.Render(sender, e);
        }

        private void StopLoop(object? sender, FormClosingEventArgs e)
        {
            Stop();
        }

        public virtual void Stop()
        {
            _gameLoop.Stop();
        }

        /// <summary>
        /// Load whatever bullshit occurs before gameloop
        /// Asset management, etc.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnLoad()
        {
            Debug.WriteLine("OnLoad()");
        }

        public virtual void Start()
        {
            _gameLoop.Start();
        }

        public virtual void OnDraw()
        {
            Debug.WriteLine("OnDraw()");
        }

        public virtual void OnUpdate()
        {
            Debug.WriteLine("OnUpdate()");
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
            _shapes.Add(shape);
        }

        public static void UnregisterEntity(Shape2D shape)
        {
            _shapes.Remove(shape);
        }

        public void SetFramerate(float framerate)
        {
            _gameLoop.SetFrameRate(framerate);
        }
    }

    public interface IEngine
    {
        void Stop();
        void OnLoad();
        void Start();
        void OnDraw();
        void OnUpdate();
        void SetFramerate(float framerate);
    }
}

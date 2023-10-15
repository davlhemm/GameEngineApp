using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public abstract class EngineBase : IEngine
    {
        //Include any base game/engine details
        private ScreenEng screen = new ScreenEng(512,512);
        private string? Title = "Base Title";
        private Canvas canvas = null!;
        private IRenderer renderer = null!;
        private IGameLoop gameLoop = null!;

        protected EngineBase() { }

        public EngineBase(ScreenEng screen, string? title, IRenderer renderer, IGameLoop gameLoop)
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
            canvas.Paint += renderer.Renderer;
            canvas.FormClosing += StopLoop;
            
            //Thread already active, just point the loop hooks to the right place
            gameLoop.GameRedraw += canvas.Redraw;
            gameLoop.GameLooped += this.OnDrawCallback;
            gameLoop.GameUpdate += this.OnUpdateCallback;

            OnLoad();
            Start();

            Application.Run(canvas);
        }

        private void StopLoop(object? sender, FormClosingEventArgs e)
        {
            gameLoop.Stop();
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

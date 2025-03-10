﻿using GameEngineApp.Tools;
using System.Diagnostics;
using System.Drawing.Printing;

namespace GameEngineApp.Engine
{
    public abstract class EngineBase : IEngine
    {
        //Include any base game/engine details
        protected IVectorTwo<float> _screen = new VectorTwo(512,512);
        protected string? _title = "Base Title";
        protected Canvas _canvas = null!;
        protected IRenderer _renderer = null!;
        protected IGameLoop _gameLoop = null!;
        protected IInputManager inputManager;
        //TODO: Render entity system, pass to renderer
        public IList<IShape2D> _shapes = new List<IShape2D>();
        public int baseSize = 32;

        protected EngineBase() {
            inputManager = null!;
        }

        public EngineBase(IVectorTwo<float> screen, string? title, IRenderer renderer, IGameLoop gameLoop)
        {
            _screen     = screen;
            _title      = title;
            _renderer   = renderer;
            _gameLoop   = gameLoop;

            _canvas = new Canvas();
            _canvas.Size = new Size((int)screen.X, (int)screen.Y);
            _canvas.Text = title;
            //TODO: Intercept this renderer callback and let it know what to draw...
            //Consider static/global entity management?
            _canvas.Paint += (sender, e) => Render(sender, e, ref _shapes);
            _canvas.FormClosing += StopLoop;

            inputManager = new InputManager(ref _canvas);

            //Thread already active, just point the loop hooks to the right place
            gameLoop.GameRedraw += _canvas.Refresh;
            gameLoop.GameLooped += OnDrawCallback;
            gameLoop.GameUpdate += OnUpdateCallback;
            //gameLoop.DelegateCallback += DelegateStuff;
            //gameLoop.DelegateCallback += gameLoop.Log;

            OnLoad();
            Start();

            Application.Run(_canvas);
        }

        private void DelegateStuff()
        {
            Logger.Info("Delegate Ran in Engine, triggered from invocation.");
        }

        private void Render(object? sender, PaintEventArgs e, ref IList<IShape2D> shapes)
        {
            _renderer.Render(sender, e, shapes);
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
        public virtual void OnLoad()
        {
            Shape2D aShape = new Shape2D(
            new VectorTwo(baseSize, baseSize),
                new VectorTwo(16, 16),
                "Shape");
            aShape.RegisterShape(ref _shapes);
        }

        public virtual void Start()
        {
            _gameLoop.Start();
        }

        public virtual void OnDraw()
        {
            //Debug.WriteLine("OnDraw()");
        }

        public virtual void OnUpdate()
        {
            //Debug.WriteLine("OnUpdate()");
        }

        public void OnDrawCallback(object? sender, GameLoopedEventArgs e)
        {
            OnDraw();
        }

        public void OnUpdateCallback(object? sender, EventArgs e)
        {
            OnUpdate();
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

using GameEngineApp.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public sealed class GameLoop : IGameLoop
    {
        public static int LoopCount = 0;
        public static bool GameLooping = false;
        public event EventHandler<GameLoopedEventArgs>? GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;

        public Thread GameLoopThread { get; set; }


        public int Frames { get; set; } = 0;
        //TODO: Queue of previous timings? 1-N?
        public long PrevFrameTime { get; set; } = DateTime.Now.Ticks;
        //TODO: Move to gameloop, stupid to do here in renderer
        public TimeSpan DeltaTime { get; set; } = TimeSpan.Zero;
        public TimeSpan FPSDeltaTime { get; set; } = TimeSpan.Zero;


        private static readonly GameLoop instance = new GameLoop();

        public static GameLoop Instance
        {
            get { return instance; }
        }

        private GameLoop() 
        {
            GameLoopThread = new Thread(Loop);
        }

        public void Loop()
        {
            while (GameLooping && GameLoopThread.IsAlive)
            {
                //Fire events for having looped
                //OnDraw -> Refresh -> OnUpdate
                //TODO: Change to static invocations or something...multicast delegates are slow
                GameLooped?.Invoke(this, new GameLoopedEventArgs(LoopCount));
                GameRedraw?.Invoke(this, new EventArgs());
                GameUpdate?.Invoke(this, new EventArgs());

                ComputeFrames();

                Logger.Info(String.Format("Loop {0}", LoopCount));
                LoopCount++;
                //TODO: Is our delta time supposed to be dictated in its own system?!  Renderer does now
                Thread.Sleep(16);
            }
        }

        private void ComputeFrames()
        {
            var currFrameTime = DateTime.Now.Ticks;
            DeltaTime = new TimeSpan(currFrameTime - PrevFrameTime);
#if DEBUG //Only compute FPS for DEBUG
            var currFrameRate = (int)((1.0f / DeltaTime.Milliseconds) * 1000.0f);
            if (Frames % currFrameRate <= 5)
            {
                FPSDeltaTime = DeltaTime;
            }
#endif
            PrevFrameTime = currFrameTime;

            Frames++;
        }

        public void Stop()
        {
            GameLooping = false;
            //TODO: Real thread management...
            //GameLoopThread?.Suspend();
        }

        public void Start() 
        {
            GameLooping = true;
            GameLoopThread.Start(); 
        }
    }

    public interface IGameLoop
    {
        Thread GameLoopThread { get; }
        void Loop();
        void Stop();
        void Start();
        public event EventHandler<GameLoopedEventArgs> GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;
    }

    public class GameLoopedEventArgs : EventArgs 
    {
        public int LoopCount { get; protected set; }
        public GameLoopedEventArgs(int loopCount) 
        {
            LoopCount = loopCount;
        }
    }
}

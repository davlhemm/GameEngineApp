using GameEngineApp.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public delegate void SomeDelegateCallback();

    public class GameLoop : IGameLoop
    {
        public static int LoopCount = 0;
        public static bool GameLooping = false;
        public static float FrameRate = 60.0f; //Frames/1000ms
        public static float FrameDensityDiff = 2.5f; // BS Constant to review later
        public event EventHandler<GameLoopedEventArgs>? GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;
        public SomeDelegateCallback DelegateCallback { get; set; }


        public Thread GameLoopThread { get; set; }


        public int Frames { get; set; } = 0;
        //TODO: Queue of previous timings? 1-N?
        public long PrevFrameTime { get; set; } = DateTime.Now.Ticks;
        public TimeSpan DeltaTime { get; set; } = TimeSpan.Zero;
        //TODO: Use more accurate method of delta-time, info being lost w/long instead of float here
        public TimeSpan FPSDeltaTime { get; set; } = TimeSpan.Zero;


        private static readonly GameLoop instance = new GameLoop();

        public static GameLoop Instance
        {
            get { return instance; }
        }

        private GameLoop() 
        {
            DelegateCallback = DefaultCallback;
            GameLoopThread = new Thread(Loop);
        }

        private void DefaultCallback()
        {
            Logger.Info("Default Callback in Loop");
        }

        public void Loop()
        {
            //TODO: Decouple our actual rendering invocations from game/physics updates.
            /// We want these to be able to act independently in case calculations
            /// require several passes before frame actually rendered.
            /// Will allow frame independence for between-frame calcs.
            /// Frame boundaries defined by what system?!
            while (GameLooping && GameLoopThread.IsAlive)
            {
                ComputeFrames();

                //Fire events for having looped
                //OnDraw -> Refresh -> OnUpdate
                //TODO: Change to static invocations or something...multicast delegates are slow
                GameLooped?.Invoke(this, new GameLoopedEventArgs(LoopCount));
                GameRedraw?.Invoke(this, new EventArgs());
                GameUpdate?.Invoke(this, new EventArgs());

#if DEBUG
                DelegateCallback.Invoke();
#endif

                LoopCount++;

                Log();

                Sleep();
            }
        }

        public void Log()
        {
            Logger.Info(String.Format("Loop {0}", LoopCount));
            Logger.WhatThread("GameLoop");
        }

        public virtual void Sleep()
        { 
            float frameSleepTotal = (1000f / FrameRate) - FrameDensityDiff;
            float neededSleep = Math.Max(frameSleepTotal - DeltaTime.Milliseconds, 0);
            Thread.Sleep((int)(frameSleepTotal - neededSleep));
        }

        private void ComputeFrames()
        {
            var currFrameTime = DateTime.Now.Ticks;
            DeltaTime = new TimeSpan(currFrameTime - PrevFrameTime);
#if DEBUG //Only compute FPS for DEBUG
            //var currFrameRate = (int)((1.0f / DeltaTime.Milliseconds) * 1000.0f);
            var FpsUpdateRate = 5f;
            if ((Frames%(FrameRate/FpsUpdateRate)) == 0)
            {
                FPSDeltaTime = DeltaTime;
            }
            Debug.WriteLine("DeltaTime in ms: " + DeltaTime.Milliseconds);
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

        public void SetFrameRate(float frameRate)
        {
            FrameRate = frameRate;
        }
    }

    public interface IGameLoop
    {
        Thread GameLoopThread { get; }
        void Loop();
        void Stop();
        void Start();
        void Sleep();
        void Log();
        void SetFrameRate(float frameRate);
        public event EventHandler<GameLoopedEventArgs> GameLooped;
        public event EventHandler? GameRedraw;
        public event EventHandler? GameUpdate;
        SomeDelegateCallback DelegateCallback { get; set; }
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

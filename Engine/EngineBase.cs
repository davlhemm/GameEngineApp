using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public abstract class EngineBase //: IRenderer
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
            canvas.Paint += renderer.Renderer;
            canvas.FormClosing += StopLoop;
            
            //Thread already active, just point the loop hooks to the right place
            gameLoop.GameRedraw += canvas.RedrawCallback;

            Application.Run(canvas);
        }

        private void StopLoop(object? sender, FormClosingEventArgs e)
        {
            gameLoop.StopLoop();
        }
    }
}

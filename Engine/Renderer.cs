using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngineApp.Engine
{
    public class Canvas : Form
    {
        public Canvas() 
        {
            this.DoubleBuffered = true;
        }

        public void Refresh(object? sender, EventArgs e)
        {
            try
            {
                //Debug.WriteLine("Refresh callback in canvas.");
                var result = BeginInvoke((MethodInvoker)delegate { Refresh(); });
            } 
            catch
            {
                Debug.WriteLine("Refresh fired while control doesn't exist.");
            }
        }
    }

    public class Renderer : RendererBase { }

    public abstract class RendererBase : IRenderer
    {
        //public static int FramesRendered { get; set; } = 0;
        ////TODO: Queue of previous timings? 1-N?
        //public static long PrevFrameTime { get; set; } = DateTime.Now.Ticks;
        ////TODO: Move to gameloop, stupid to do here in renderer
        //public static TimeSpan DeltaTime { get; set; } = TimeSpan.Zero;
        //public static TimeSpan FPSDeltaTime { get; set; } = TimeSpan.Zero;

        //TODO: Manage entities, this is stupid
        private static readonly Pen TestPen = new Pen(Color.White, 8);
        private static readonly SolidBrush TestSolidBrush = new SolidBrush(Color.Cyan);
        private static readonly FontFamily TestFontFam = new FontFamily(GenericFontFamilies.Monospace);
        private static readonly Font TestFont = new Font(TestFontFam, 16.0f, FontStyle.Bold);

        public virtual void Render(object? sender, PaintEventArgs e)
        {

            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            if (true)//FramesRendered % 60 <= 30)
            { 
                graphics.Clear(Color.Black); 
            }

            //Draw all entities
            DrawShapes(ref graphics);
#if DEBUG   //Draw FPS if debugging
            DrawFPS(ref graphics, GameLoop.Instance.FPSDeltaTime);
#endif
        }

        private void DrawShapes(ref Graphics graphics)
        {
            foreach(var shape in GameEngine._shapes)
            {
                graphics.FillRectangle(TestSolidBrush, 
                    shape!.Position!.X, shape!.Position!.Y, 
                    shape!.Scale!.X, shape!.Scale!.Y);
            }
        }

        private void DrawFPS(ref Graphics graphics, TimeSpan deltaTimeSpan)
        {
            var rightBound = graphics.ClipBounds.Right;
            graphics.DrawString(((int)((1.0f / deltaTimeSpan.Milliseconds) * 1000.0f)).ToString() + "fps",
                TestFont,
                new SolidBrush(Color.Blue),
                new PointF(rightBound-32f, 0.0f));
        }
    }

    public interface IRenderer
    {
        public static int FramesRendered { get; set; }
        public static long PrevFrameTime { get; set; }
        public void Render(object? sender, PaintEventArgs e);
    }
}

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

        public override void Refresh()
        {
            base.Refresh();
        }

        public void Refresh(object? sender, EventArgs e)
        {
            try
            {
                //Debug.WriteLine("Refresh callback in canvas.");
                var result = BeginInvoke((MethodInvoker)delegate { Refresh(); });
                //Refresh();
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
        //TODO: Manage with entity-component-system, this is stupid

        //private static readonly Pen TestPen = new Pen(Color.White, 8);
        private static readonly SolidBrush TestSolidBrush = new SolidBrush(Color.Cyan);
        private static readonly FontFamily TestFontFam = new FontFamily(GenericFontFamilies.Monospace);
        private static readonly Font TestFont = new Font(TestFontFam, 16.0f, FontStyle.Bold);

        public virtual void Render(object? sender, PaintEventArgs e)
        {
            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            if (true)
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
            foreach(var shape in EngineBase._shapes)
            {
                graphics.FillRectangle(TestSolidBrush, 
                    shape!.Position!.X, shape!.Position!.Y, 
                    shape!.Scale!.X, shape!.Scale!.Y);
            }
        }

        private void DrawFPS(ref Graphics graphics, TimeSpan deltaTimeSpan)
        {
            var rightBound = graphics.ClipBounds.Right;
            var dataString = ((int)((1.0f / deltaTimeSpan.Milliseconds) * 1000.0f)).ToString() + "fps";
            graphics.DrawString(dataString,
                TestFont,
                new SolidBrush(Color.Blue),
                new PointF(rightBound-(dataString.Length*16f), 0.0f));
        }
    }

    public interface IRenderer
    {
        public void Render(object? sender, PaintEventArgs e);
    }
}

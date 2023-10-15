using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void Redraw(object? sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Redraw callback in canvas.");
                var result = BeginInvoke((MethodInvoker)delegate { Refresh(); });
            } 
            catch
            {
                Debug.WriteLine("Redraw fired while control doesn't exist.");
            }
        }
    }

    public class Renderer : RendererBase { }

    public abstract class RendererBase : IRenderer
    {
        public void Renderer(object? sender, PaintEventArgs e)
        {
            //Get graphics from paint event
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.Black);
            graphics.DrawRectangle(new Pen(Color.White),new Rectangle(new Point(32,32),new Size(32,32)));
            Debug.WriteLine(String.Format("Graphics drawn in renderer: {0}", e.ToString()));
        }
    }

    public interface IRenderer
    {
        public void Renderer(object? sender, PaintEventArgs e);
    }
}

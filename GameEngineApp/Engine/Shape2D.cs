using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class Image2D : Shape2D
    {
        public Image? Image { get; set; } = null;

        private Image2D(): base() { }

        public Image2D(VectorTwo position, VectorTwo scale, string key, string filePath): 
            base(position, scale, key)
        {
            Image = Image.FromFile(filePath);
        }

        public override void DrawEntity(ref Graphics g)
        {
            g.DrawImage(Image?? Image.FromFile("defaultItem.jpg"), Position?.X??0, Position?.Y??0, Scale?.X??0, Scale?.Y??0);
        }
    }

    public class Shape2D : Entity, IShape2D
    {
        private static readonly SolidBrush TestSolidBrush = new SolidBrush(Color.Cyan);

        public VectorTwo? Position { get; set; } = null!;
        public VectorTwo? Scale { get; set; } = null!;

        protected Shape2D(): base("") { }

        public Shape2D(VectorTwo position, VectorTwo scale, string key): base(key)
        {
            Position = position;
            Scale = scale;
            Key = key;
            RegisterEntity();
        }

        public override void RegisterEntity()
        {
            EngineBase.RegisterShape(this);
        }

        public virtual void DrawEntity(ref Graphics g)
        {
            g.FillRectangle(TestSolidBrush,
                Position!.X, Position!.Y,
                Scale!.X, Scale!.Y);
        }

        ~Shape2D() 
        {
            EngineBase.UnregisterShape(this);
        }
    }

    public interface IShape2D : IEntity
    {
        VectorTwo? Position { get; set; }
        VectorTwo? Scale { get; set; }

        void DrawEntity(ref Graphics g);
    }
}

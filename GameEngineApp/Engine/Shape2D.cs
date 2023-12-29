using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public class Shape2D : Entity
    {
        public VectorTwo? Position { get; set; } = null!;
        public VectorTwo? Scale { get; set; } = null!;

        private Shape2D(): base("") { }

        public Shape2D(VectorTwo position, VectorTwo scale, string key): base(key)
        {
            Position = position;
            Scale = scale;
            Key = key;
            RegisterEntity();
        }

        protected override void RegisterEntity()
        {
            EngineBase.RegisterEntity(this);
        }
    }
}

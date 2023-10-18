using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public abstract class Entity
    {
        //Reference key for Dictionaries, etc.?
        public string Key { get; set; }

        protected Entity(string key) 
        {
            Key = key;
        }

        //TODO: This is dumb as hell, need an entity factory to handle this anyway
        protected abstract void RegisterEntity();

        ~Entity() { }
    }


}

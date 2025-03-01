using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Engine
{
    public abstract class Entity : IEntity
    {
        //Reference key for Dictionaries, etc.? GUID?
        public string Key { get; set; }

        protected Entity(string key) 
        {
            Key = key;
        }

        //TODO: This is dumb as hell, need an entity factory to handle this anyway
        public virtual void RegisterEntity(ref List<IEntity> entities, string key)
        {
            // entities.Remove(entities.First(x => x.Key.Equals(key)));
        }

        public virtual void Update(IInputManager inputManager)
        {
            // throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void UnregisterEntity(ref List<IEntity> entities, string key)
        {
            entities.Remove(entities.First(x => x.Key.Equals(key)));
        }

        ~Entity() 
        { 
            Dispose(); 
        }
    }

    public interface IEntity : IDisposable
    {
        string Key { get; }
        void RegisterEntity(ref List<IEntity> entities, string key);
        void UnregisterEntity(ref List<IEntity> entities, string key);
        void Update(IInputManager inputManager);

    }
}

﻿using System;
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
        public abstract void RegisterEntity();

        ~Entity() { }
    }

    public interface IEntity
    {
        string Key { get; }
        void RegisterEntity();

    }
}

using System;
using System.Collections.Generic;

namespace DIContainer
{
    public class Container
    {
        private Dictionary<Type, object> _bindings = new();
        private Container _parent;
        
        public Container(Container parent = null)
        {
            _parent = parent;
        }
        
        public void Bind(object[] objs)
        {
            foreach (var obj in objs)
            {
                _bindings.TryAdd(obj.GetType(), obj);
            }
        }
        
        public void Bind<TImplementation, TInterface>() where TImplementation : TInterface, new()
        {
            var obj = new TImplementation();
            _bindings.TryAdd(typeof(TInterface), obj);
        }
        
        
        public object Resolve(Type type)
        {
            if (_bindings.TryGetValue(type, out var obj))
                return obj;

            if (_parent != null)
                return _parent.Resolve(type);
            
            throw new Exception($"Dependency {type} not found");
        }
    }
}
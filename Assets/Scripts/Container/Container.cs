using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return Instantiate(prefab, Vector3.zero, quaternion.identity, parent);
        }
        
        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            bool isActive = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);
            
            var obj = Object.Instantiate(prefab, position, rotation, parent);
            var monos = obj.GetComponents<MonoBehaviour>();
            
            foreach (var monoBehaviour in monos)
                Inject(monoBehaviour);
            
            prefab.gameObject.SetActive(isActive);
            return obj;
        }

        public void Bind(object[] objs)
        {
            foreach (var obj in objs)
            {
                _bindings.TryAdd(obj.GetType(), obj);
            }
        }
        
        public void BindInstance<TInterface>(TInterface instance)
        {
            var type = typeof(TInterface);

            if (_bindings.ContainsKey(type))
                throw new Exception($"{type} already binded");

            _bindings.Add(type, instance);
        }
        
        public void Bind<TInterface, TImplementation>()
            where TImplementation : TInterface
        {
            var interfaceType = typeof(TInterface);

            if (_bindings.ContainsKey(interfaceType))
                throw new Exception($"{interfaceType} already binded");

            var instance = Create(typeof(TImplementation));

            _bindings.Add(interfaceType, instance);
        }
        
        private object Create(Type type)
        {
            var constructor = type.GetConstructors()[0];
            var parameters = constructor.GetParameters();

            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Resolve(parameters[i].ParameterType);
            }

            return Activator.CreateInstance(type, args);
        }
        
        public object Resolve(Type type)
        {
            if (_bindings.TryGetValue(type, out var obj)) 
                return obj;

            if (_parent != null) 
                return _parent.Resolve(type);

            throw new Exception($"Dependency {type} not found");
        }
        
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
        
        private object[] ResolveParameters(ParameterInfo[] parameters)
        {
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
                args[i] = Resolve(parameters[i].ParameterType);

            return args;
        }
         
        public void Inject(MonoBehaviour mono)
        {
            InjectConstructors(mono.GetType());
            InjectFields(mono);
            InjectProperties(mono);
            InjectMethods(mono);
        }
        
        private void InjectFields(MonoBehaviour mono)
        {
            var fields =
                mono.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (var fieldInfo in fields)
            {
                if (Attribute.IsDefined(fieldInfo, typeof(InjectAttribute)))
                    fieldInfo.SetValue(mono, Resolve(fieldInfo.FieldType));
            }
        }

        private void InjectProperties(MonoBehaviour mono)
        {
            var properties = mono.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(InjectAttribute)))
                    property.SetValue(mono, Resolve(property.PropertyType));
            }
        }

        private object InjectConstructors(Type type)
        {
            var constructors = type.GetConstructors();

            var ctor = constructors
                           .FirstOrDefault(c => Attribute.IsDefined(c, typeof(InjectAttribute))) ?? constructors[0];

            var args = ResolveParameters(ctor.GetParameters());

            return Activator.CreateInstance(type, args);
        }

        private void InjectMethods(MonoBehaviour mono)
        {
            var methods = mono.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(InjectAttribute)))
                    continue;

                var args = ResolveParameters(method.GetParameters());

                method.Invoke(mono, args);
            }
        }
    }
}
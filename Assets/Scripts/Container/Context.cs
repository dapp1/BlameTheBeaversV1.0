using System;
using System.Reflection;
using UnityEngine;

namespace DIContainer
{
    [DefaultExecutionOrder(-1001)]
    public class Context : MonoBehaviour
    {
        public Container Container { get; protected set; }
        
        public void Inject(MonoBehaviour monoBehaviour)
        {
            InjectFields(monoBehaviour);
            InjectProperties(monoBehaviour);
            InjectConstructors(monoBehaviour);
            InjectMethods(monoBehaviour);
        }

        private void InjectFields(MonoBehaviour mono)
        {
            var type = mono.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fields)
            {
                if (Attribute.IsDefined(fieldInfo, typeof(InjectAttribute)))
                {
                    var dependency = Container.Resolve(fieldInfo.FieldType);
                    fieldInfo.SetValue(mono, dependency);
                }
            }
        }

        private void InjectProperties(MonoBehaviour mono)
        {

            var type = mono.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(InjectAttribute)))
                {
                    var dependency = Container.Resolve(property.PropertyType);
                    property.SetValue(mono, dependency);
                }
            }
        }

        private void InjectConstructors(MonoBehaviour mono)
        {
                var type = mono.GetType();
                var constructors = type
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                foreach (var constructorInfo in constructors)
                {
                    if (Attribute.IsDefined(constructorInfo, typeof(InjectAttribute)))
                    {
                        var parameters = constructorInfo.GetParameters();
                        var args = new object[parameters.Length];

                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var paramType = parameters[i].ParameterType;
                            var dependency = Container.Resolve(paramType);
                            args[i] = dependency;
                        }
                    }
                }
        }

        private void InjectMethods(MonoBehaviour type)
        {
            var methods = type.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                foreach (var method in methods)
                {
                    if (Attribute.IsDefined(method, typeof(InjectAttribute)))
                    {
                        var parameters = method.GetParameters();
                        var args = new object[parameters.Length];

                        for (var i = 0; i < parameters.Length; i++)
                        {
                            var paramType = parameters[i].ParameterType;
                            var dependency = Container.Resolve(paramType);
                            args[i] = dependency;
                        }
                    }
                }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SimpleContainer : MonoBehaviour
{
    [Header("MonoBehaviours")]
    [SerializeField] private List<MonoBehaviour> _objects = new List<MonoBehaviour>();

    [Header("Configs")]
    [SerializeField] private List<ScriptableObject> _scriptableObjects = new List<ScriptableObject>();
    
    private readonly Dictionary<Type, object> _instances = new();
    
    private void Awake()
    {
        RegisterMonoBehaviours();
        RegisterScriptableObjects();
        InjectAll();
    }

    private void InjectAll()
    {
        foreach (var obj in _objects)
        {
            if (obj is IInitializable)
                Inject(obj);
        }
    }

    private void RegisterMonoBehaviours()
    {
        foreach (var item in _objects)
        {
            if (item is IInitializable initializable)
            {
                RegisterInstance(initializable);
            }
        }
    }

    private void RegisterScriptableObjects()
    {
        foreach (var so in _scriptableObjects)
        {
            RegisterInstance(so);
        }
    }
    
    private void RegisterInstance(object obj)
    {
        var type = obj.GetType();
        _instances.TryAdd(type, obj);

        foreach (var iface in type.GetInterfaces())
        {
            if(iface != typeof(IInitializable))
                _instances.TryAdd(iface, obj);
        }
    }
    
    private void Inject(object target)
    {
        var methods = target.GetType().GetMethods(
                BindingFlags.Instance | 
                BindingFlags.Public | 
                BindingFlags.NonPublic
            )
            .Where(m => m.Name == "Init")
            .ToArray();

        if (methods.Length == 0)
            return;
        
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;

                if (_instances.TryGetValue(paramType, out var dependency))
                {
                    args[i] = dependency;
                }
                else
                {
                    Debug.LogError($"Dependency {paramType.Name} not found for {target.GetType().Name}");
                    return;
                }
            }

            method.Invoke(target, args);
        }
    }
}

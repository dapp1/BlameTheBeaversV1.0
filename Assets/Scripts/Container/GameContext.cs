using EntityFactory;
using UnityEngine;

namespace DIContainer
{
    [DefaultExecutionOrder(-2000)]
    public class GameContext : Context
    {
        public static GameContext Instance { get; private set; }
        
        [SerializeField] private MonoBehaviour[] _objects;
        
        [Header("Scriptables")]
        [SerializeField] protected ScriptableObject[] _scriptableObjects;
        
        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            DontDestroyOnLoad(this);
        }

        protected override void InstallBindings()
        {
            Container.Bind(_scriptableObjects);
            Container.BindInstance(Container);
            
            foreach (var monoBehaviour in _objects)
            {
                Container.Inject(monoBehaviour);
            }
        }
    }
}
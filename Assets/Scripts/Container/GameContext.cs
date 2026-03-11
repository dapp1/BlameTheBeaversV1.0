using EnemyFactory;
using UnityEngine;

namespace DIContainer
{
    [DefaultExecutionOrder(-1000)]
    public class GameContext : Context
    {
        [SerializeField] private MonoBehaviour[] _objects;
        
        [Header("Scriptables")]
        [SerializeField] protected ScriptableObject[] _scriptableObjects;
        
        protected void Awake()
        {
            DontDestroyOnLoad(this);
            
            Container = new Container();
            Bind();
            
            foreach (var monoBehaviour in _objects)
            {
                Inject(monoBehaviour);
            }
        }

        private void Bind()
        {
            Container.Bind(_scriptableObjects);
            
            Container.Bind<EnemyFactory.EnemyFactory,IEnemyFactory>();
        }
    }
}
using UnityEngine;

namespace DIContainer
{
    [DefaultExecutionOrder(-999)]
    public class SceneContext : Context
    {
        [SerializeField] private MonoBehaviour[] _objects;
        private GameContext _gameContext;
        
        protected void Awake()
        {
            _gameContext = FindObjectOfType<GameContext>();
            
            Container = new Container(_gameContext.Container);

            foreach (var monoBehaviour in _objects)
            {
                Inject(monoBehaviour);
            }
        }
    }
}
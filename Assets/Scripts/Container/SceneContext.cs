using EntityFactory;
using UnityEngine;

namespace DIContainer
{
    [DefaultExecutionOrder(-999)]
    public class SceneContext : Context
    {
        [SerializeField] private MonoBehaviour[] _objects;
        
        private GameContext _gameContext;
        
        protected override void Awake()
        {
            _gameContext = GameContext.Instance;
            Container = new Container(_gameContext.Container);
            InstallBindings();
        }
        
        protected override void InstallBindings()
        {
            Container.BindInstance(Container);
            Container.Bind<IEntityFactory, EntityFactory.EntityFactory>();
            
            foreach (var monoBehaviour in _objects)
            {
                Container.Inject(monoBehaviour);
            }
        }
    }
}
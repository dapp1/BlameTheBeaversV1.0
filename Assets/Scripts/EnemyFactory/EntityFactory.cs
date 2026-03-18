using System.Collections.Generic;
using Configs;
using DIContainer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntityFactory
{ 
    public class EntityFactory : IEntityFactory
    {
        private EntitiesFactoryConfig _config;
        private Dictionary<EntityType, LocalEntityPool> _pools = new Dictionary<EntityType, LocalEntityPool>();
        private Container _container;
        
        [Inject]
        public EntityFactory(EntitiesFactoryConfig config, Container container)
        {
            _config = config;
            _container = container;
        }

        public void InitializeFactory(EntityType type, int maxCount = -1)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                var prefab = _config.GetPrefabByType(type);
                pool = new LocalEntityPool(prefab, _container, maxCount);
                _pools[type] = pool;
            }
        }
        
        public GameObject CreateEntity(EntityType type, Vector2 position, int maxCount = -1)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                var prefab = _config.GetPrefabByType(type);
                pool = new LocalEntityPool(prefab, _container, maxCount, null);
                _pools[type] = pool;
            }

            return pool.Get(position);
        }
    }
    
    public class LocalEntityPool
    {
        private int _maxCount;
        private List<GameObject> _objects;
        private Transform _parent;
        private GameObject _prefab;
        private Container _container;
        
        public LocalEntityPool(GameObject prefab, Container container, int maxCount = -1, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _maxCount = maxCount;
            _objects = new List<GameObject>(_maxCount <= -1 ? 100 : _maxCount);
            _container = container;

            int initialCount = _maxCount <= -1 ? 5 : _maxCount;
            
            if (_parent == null)
            {
                GameObject go = new GameObject($"{prefab.name}_Pool");
                _parent = go.transform;
            }

            for (int i = 0; i < initialCount; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                obj.SetActive(false);
                _objects.Add(obj);
            }
        }

        public GameObject Get(Vector2 position)
        {
            foreach (var obj in _objects)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.transform.position = position;
                    obj.SetActive(true);
                    return obj;
                }
            }
            
            if (_maxCount < 0 || _objects.Count < _maxCount)
            {
                var obj = _container.Instantiate(_prefab, position, Quaternion.identity, _parent);
                _objects.Add(obj);
                return obj;
            }

            return null;
        }

        public void Add(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_parent);
            if (!_objects.Contains(obj))
                _objects.Add(obj);
        }

        public void Remove(GameObject obj)
        {
            _objects.Remove(obj);
            Object.Destroy(obj);
        }
    }
}
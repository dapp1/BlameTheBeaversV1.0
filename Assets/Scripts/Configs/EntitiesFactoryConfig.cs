using System;
using EntityFactory;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public struct EntityPair
    {
        public GameObject Prefab;
        public EntityType Type;
    }

    [CreateAssetMenu(menuName = "Configs/EntityFactory", fileName = "EntityFactoryConfig")]
    public class EntitiesFactoryConfig : ScriptableObject
    {
        [field: SerializeField] public EntityPair[] Entities { get; private set; }

        public GameObject GetPrefabByType(EntityType type)
        {
            foreach (var entityPair in Entities)
            {
                if (entityPair.Type == type)
                    return entityPair.Prefab;
            }

            throw new Exception($"{type} not founded");
        }
    }
}
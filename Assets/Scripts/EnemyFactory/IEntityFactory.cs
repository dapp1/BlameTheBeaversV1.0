using UnityEngine;

namespace EntityFactory
{
    public interface IEntityFactory
    {
        GameObject CreateEntity(EntityType type, Vector2 position, int maxCount);
    }
}
using Configs.Beaver;
using Configs.Charactert;
using Entites;
using UnityEngine;

namespace Character
{
    public class AttackContext
    {
        public IDamagable Target;
        public InventoryItemType ItemType;
        public float Damage;
        public float TargetPositionX;
        public float LookAtX;
    }
    
    public class AttackResolver
    {
        private CharacterConfig _config;

        public AttackResolver(CharacterConfig config)
        {
            _config = config;
        }

        public AttackContext ResolveRoot(RootController root, Vector3 pos)
        {
            var context = new AttackContext();
            context.Target = root;

            context.TargetPositionX = root.transform.position.x < pos.x
                ? root.transform.position.x + 0.6f
                : root.transform.position.x - 0.6f;

            context.LookAtX = root.transform.position.x;

            switch (root.CurrentLevel)
            {
                case 0:
                    context.ItemType = InventoryItemType.Hands;
                    context.Damage = _config.DamageRootByHands;
                    break;

                case 1:
                    context.ItemType = InventoryItemType.Shovel;
                    context.Damage = _config.DamageRootByShovel;
                    break;

                default:
                    context.ItemType = InventoryItemType.Axe;
                    context.Damage = _config.DamageRootByAxe;
                    break;
            }

            return context;
        }
    }
}
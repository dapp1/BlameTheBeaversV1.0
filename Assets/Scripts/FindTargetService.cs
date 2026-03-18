using UnityEngine;

namespace EntityServices
{
    public class FindTargetService : MonoBehaviour
    {
        [SerializeField] private bool _infiniteDistance;
        
        //test
        public Transform GetTarget()
        {
            var player = FindObjectOfType<CharacterController>();
            if (_infiniteDistance)
            {
                return player.transform;
            }
            return null;
        }
    }
}
using System;
using Entites;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewStateMachine.BeaverStates
{
    public class WalkState : IState<StateDataBase>
    {
        public void OnEnter(WalkStateData data)
        {
            throw new NotImplementedException();
        }

        public event Action<StateDataBase> RequestToTransition;

        private Animator _animator;
        private Transform _owner;
        private Transform _target;
        private float _speed;

        public WalkState(Animator animator, Transform owner, float speed)
        {
            _animator = animator;
            _owner = owner;
            _speed = speed;
            _target = Object.FindObjectOfType<CharacterController>().transform;
        }

        public void OnEnter(StateDataBase data)
        {
            if (data is WalkStateData stateData)
            {
                
            }
            
            _animator.Play("Run");
        }

        public void FixedUpdate()
        {
            var direction = _owner.transform.position.x > _target.transform.position.x ? -1 : 1;
            _owner.transform.localScale = new Vector3(-direction, _owner.transform.localScale.y);
            _owner.transform.position += new Vector3(direction * _speed * Time.deltaTime, 0);
            
            if (Mathf.Abs(_owner.position.x - _target.position.x) <= 0.5f &&
                _target.TryGetComponent<IDamagable>(out var damageable))
            {
                RequestToTransition?.Invoke(new AttackStateData(damageable, _target));
            }
        }
        
        public void OnExit()
        {

        }
    }
}
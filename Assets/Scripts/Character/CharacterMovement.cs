using System;
using System.Collections;
using UnityEngine;

namespace Character
{
    public class CharacterMovement
    {
        private GameObject _ownObject;
        private Animator _animator;
        private Transform _renderRoot;
        private float _speed;
        private Action _stopAction;
        private bool _isAutoMove;
        
        public bool IsAutoMove => _isAutoMove;
        
        public CharacterMovement(GameObject ownObject, Animator animator, Transform renderRoot, float speed, Action stopAction)
        {
            _ownObject = ownObject;
            _animator = animator;
            _renderRoot = renderRoot;
            _speed = speed;
            _stopAction = stopAction;
        }

        public void Move()
        {
            var direction = Input.GetAxis("Horizontal");
            direction = direction == 0 ? 0 : direction < 0 ? -1 : 1;
        
            if (direction != 0)
            {
                _stopAction?.Invoke();
                _ownObject.transform.position += new Vector3(direction * _speed, 0);
                _renderRoot.localScale = new Vector3(direction, _renderRoot.localScale.y);
                _animator.SetBool("isMoving", true);
            }
            else if (!IsAutoMove)
                _animator.SetBool("isMoving", false);
        }
        
        public void SetAutoMove(bool value) => _isAutoMove = value;
        
        public IEnumerator MoveToRoutine(Transform target, float threshold, bool canAct)
        {
            yield return MoveToRoutine(target.position.x, threshold, canAct);
        }

        public IEnumerator MoveToRoutine(float targetPositionX, float threshold, bool canAct)
        {
            _animator.SetBool("isMoving", true);
            _isAutoMove = true;

            while (!IsCloseEnoughToTarget(targetPositionX, threshold))
            {
                if (canAct)
                    MakeStepTowardsTarget(targetPositionX);
            
                yield return new WaitForFixedUpdate();
            }
        
            _isAutoMove = false;
            _animator.SetBool("isMoving", false);
        }
        
        private void MakeStepTowardsTarget(float targetPositionX)
        {
            var direction = _renderRoot.position.x > targetPositionX ? -1 : 1;
            _renderRoot.localScale = new Vector3(direction, _renderRoot.localScale.y);
            _ownObject.transform.position += new Vector3(direction * _speed, 0);
        }
        
        private bool IsCloseEnoughToTarget(float targetPositionX, float threshold)
        {
            return Mathf.Abs(_ownObject.transform.position.x - targetPositionX) < threshold;
        }
    }
}
using System;
using Configs.Charactert;
using DIContainer;
using Entites;
using Pixelplacement;
using UnityEngine;

namespace Character
{
    public class BaseCharacterController : BaseEntity, IDamagable
    {
        protected float _speed = 12;
        protected float _jumpForce;
    
        [SerializeField] protected Transform renderRoot;
        [SerializeField] protected InventoryItemDto _currentActiveItem;
        [SerializeField] protected Rigidbody2D _throwAxePrefab;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected AnimationCurve _damageChangeColorCurve;
    
        protected InventoryItemType _currentRoutineItemType;

        protected bool _isGrounded;

        protected Animator _anim;
        protected Rigidbody2D _rb;
    
        protected Action _onActionExecuted;
        protected Coroutine _currentRoutine;
        
        protected bool _canAct = true;

        protected CharacterMovement _movement;
        
        [Inject] protected CharacterConfig _config;
        
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();

            _speed = _config.CharacterSpeed;
            _jumpForce = _config.JumpForce;

            _movement = new CharacterMovement(gameObject, _anim, renderRoot, _config.CharacterSpeed, StopCurrentRoutine);
        }
        
        void FixedUpdate()
        {
            if (!_canAct)
                return;
            
            _movement.Move();

            if (Input.GetKey(KeyCode.Space) && _isGrounded)
            {
                StopCurrentRoutine();
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(new Vector2(0, _jumpForce));
            }
        }
        
        public void TakeDamage(int damage)
        {
            _anim.speed = 0;
            _canAct = false;

            Tween.Value(Color.white, new Color(0.4f, 0.7f, 1, 1), (Color value) => { _renderer.color = value; },
                1 /*GlobalSettings.Instance.FreezeDurationSeconds*/, 0, _damageChangeColorCurve, completeCallback: () =>
                {
                    _anim.speed = 1;
                    _canAct = true;
                });
        }
        
        //Called from animation event
        protected void StopCurrentRoutine()
        {
            if (_currentRoutine != null)
            {
                _anim.Play("Default");
                StopCoroutine(_currentRoutine);
                _currentRoutine = null;
                _movement.SetAutoMove(false);
            }
        }

        protected void OnActionAnimationEvent()
        {
            if (_currentRoutineItemType == InventoryItemType.Hands)
            {
                _onActionExecuted?.Invoke();
            }
            else if (_currentRoutineItemType == _currentActiveItem.Type)
            {
                _onActionExecuted?.Invoke();
                _currentActiveItem.Damage(1);
                if (_currentActiveItem.Durability == 0)
                    StopCurrentRoutine();
            }
            else
            {
                StopCurrentRoutine();
            }
        }
    
        protected void LookAt(float lookAtPositionX)
        {
            var direction = transform.position.x > lookAtPositionX ? -1 : 1;
            renderRoot.localScale = new Vector3(direction, renderRoot.localScale.y);
        }
        
        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("ground"))
            {
                _isGrounded = true;
                _anim.SetBool("isGrounded", true);
            }
        }
    
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("ground"))
            {
                _isGrounded = false;
                _anim.SetBool("isGrounded", false);
            }
        }
    }
}
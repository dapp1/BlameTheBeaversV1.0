using System;
using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Pixelplacement;

public class CharacterController : Singleton<CharacterController>
{
    [SerializeField] 
    private float _speed = 12;

    [SerializeField] 
    private float _jumpForce;
    
    [SerializeField] 
    private Transform renderRoot;
    
    [SerializeField] 
    private InventoryItemDto _currentActiveItem;
    
    [SerializeField]
    private Rigidbody2D _throwAxePrefab;

    private InventoryItemType _currentRoutineItemType;
    
    private bool _isGrounded;
    
    private Animator _anim;
    private Rigidbody2D _rb;
    
    [SerializeField]
    private SpriteRenderer _renderer;
    
    private Action _onActionExecuted;
    private Coroutine _currentRoutine;

    private bool _autoMove;
    private bool _canAct = true;
    
    [SerializeField]
    private AnimationCurve _damageChangeColorCurve;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _speed = GlobalSettings.Instance.CharacterSpeed;
        _jumpForce = GlobalSettings.Instance.JumpForce;
    }
    
    void FixedUpdate()
    {
        if (!_canAct)
            return;
        
        var direction = Input.GetAxis("Horizontal");
        direction = direction == 0 ? 0 : direction < 0 ? -1 : 1;
        
        if (direction != 0)
        {
            StopCurrentRoutine();
            transform.position += new Vector3(direction * _speed, 0);
            renderRoot.localScale = new Vector3(direction, renderRoot.localScale.y);
            _anim.SetBool("isMoving", true);
        }
        else if (_autoMove == false)
            _anim.SetBool("isMoving", false);

        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            StopCurrentRoutine();
            _isGrounded = false;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpForce));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ThrowAxe();
        }
    }

    private void ThrowAxe()
    {
        if (!_isGrounded || _currentActiveItem.Type != InventoryItemType.Axe)
            return;

        var axe = Instantiate(_throwAxePrefab, transform.position, Quaternion.identity);
        axe.velocity = new Vector2(0, 10);
        
        InventoryController.Instance.Remove(_currentActiveItem);
    }

    public void SetActiveItem(InventoryItemDto itemDto)
    {
        StopCurrentRoutine();
        
        _anim.SetInteger("weaponIndex", (int)itemDto.Type);
        _anim.Play("Default");

        _currentActiveItem = itemDto;
    }
    
    private IEnumerator GoToRoutine(Transform target, float threshold)
    {
        _anim.SetBool("isMoving", true);
        _autoMove = true;

        while (!IsCloseEnoughToTarget(target.position.x, threshold))
        {
            if (_canAct)
                MakeStepTowardsTarget(target.position.x);
            
            yield return new WaitForFixedUpdate();
        }
        
        _autoMove = false;
        _anim.SetBool("isMoving", false);
    }

    private IEnumerator GoToRoutine(float targetPositionX, float threshold)
    {
        _anim.SetBool("isMoving", true);
        _autoMove = true;

        while (!IsCloseEnoughToTarget(targetPositionX, threshold))
        {
            if (_canAct)
                MakeStepTowardsTarget(targetPositionX);
            
            yield return new WaitForFixedUpdate();
        }
        
        _autoMove = false;
        _anim.SetBool("isMoving", false);
    }
    
    private void MakeStepTowardsTarget(float targetPositionX)
    {
        var direction = transform.position.x > targetPositionX ? -1 : 1;
        renderRoot.localScale = new Vector3(direction, renderRoot.localScale.y);
        transform.position += new Vector3(direction * _speed, 0);
    }

    private bool IsCloseEnoughToTarget(float targetPositionX, float threshold)
    {
        return Mathf.Abs(transform.position.x - targetPositionX) < threshold;
    }
    
    public void AttackRoot(string actionName, float pivotPosition, float rootPosition, Action onActionExecuted)
    {
        if (!_isGrounded)
            return;
        
        StopCurrentRoutine();
        
        if (actionName == "Shovel")
            _currentRoutine = StartCoroutine(AttackWithShovelRoutine(pivotPosition, rootPosition, onActionExecuted));
        else if (actionName == "Axe")
            _currentRoutine = StartCoroutine(AttackWithAxeRoutine(pivotPosition, rootPosition, onActionExecuted));
        else if (actionName == "Hands")
            _currentRoutine = StartCoroutine(AttackWithHandsRoutine(pivotPosition, rootPosition, onActionExecuted));
    }

    public void PickItem(InventoryItemObject itemObject, Action onPick)
    {
        if (!_isGrounded)
            return;
        
        StopCurrentRoutine();
        
        _currentRoutine = StartCoroutine(PickItemRoutine(itemObject, onPick));
    }

    private IEnumerator PickItemRoutine(InventoryItemObject itemObject, Action onPick)
    {
        yield return GoToRoutine(itemObject.transform, 0.05f);
        onPick?.Invoke();
    }
    
    public void KickBeaver(BeaverController beaver, Action onKick)
    {
        if (!_canAct || !_isGrounded)
            return;
        
        StopCurrentRoutine();
        
        _currentRoutine = StartCoroutine(KickBeaverRoutine(beaver, onKick));
    }

    private IEnumerator KickBeaverRoutine(BeaverController beaver, Action onKick)
    {
        yield return GoToRoutine(beaver.transform, 1f);
    
        LookAt(beaver.transform.position.x);

        _anim.SetBool("isKick", true);
    
        _onActionExecuted = onKick;
    }

    //Called from animation event
    private void OnKickBeaver()
    {
        _anim.SetBool("isKick", false);
        _onActionExecuted.Invoke();
    }

    public void StopCurrentRoutine()
    {
        if (_currentRoutine != null)
        {
            _anim.Play("Default");
            StopCoroutine(_currentRoutine);
            _currentRoutine = null;
            _autoMove = false;
        }
    }
    
    private IEnumerator AttackWithShovelRoutine(float targetPositionX, float lookAtPositionX, Action onActionExecuted)
    {
        if (_currentActiveItem.Type != InventoryItemType.Shovel)
        {
           //Play confusion
           yield break;
        }

        _currentRoutineItemType = InventoryItemType.Shovel;

        yield return GoToRoutine(targetPositionX, 0.05f);
        
        LookAt(lookAtPositionX);
        
        _anim.Play("ActionShovel");
        _onActionExecuted = onActionExecuted;
    }
    
    private IEnumerator AttackWithAxeRoutine(float targetPositionX, float lookAtPositionX, Action onActionExecuted)
    {
        if (_currentActiveItem.Type != InventoryItemType.Axe)
        {
            //Play confusion
            yield break;
        }
        
        _currentRoutineItemType = InventoryItemType.Axe;
        
        yield return GoToRoutine(targetPositionX, 0.05f);

        LookAt(lookAtPositionX);

        _anim.Play("ActionAxe");
        _onActionExecuted = onActionExecuted;
    }
    
    private IEnumerator AttackWithHandsRoutine(float targetPositionX, float lookAtPositionX, Action onActionExecuted)
    {
        _currentRoutineItemType = InventoryItemType.Hands;
        
        yield return GoToRoutine(targetPositionX, 0.05f);
        
        LookAt(lookAtPositionX);
        
        _anim.Play("ActionHands");
        _onActionExecuted = onActionExecuted;
    }

    
    //Called from animation event
    private void OnActionAnimationEvent()
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
    
    private void LookAt(float lookAtPositionX)
    {
        var direction = transform.position.x > lookAtPositionX ? -1 : 1;
        renderRoot.localScale = new Vector3(direction, renderRoot.localScale.y);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
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

    public void Damage()
    {
        _anim.speed = 0;
        _canAct = false;
        
        Tween.Value(Color.white, new Color(0.4f, 0.7f, 1, 1), (Color value) =>
        {
            _renderer.color = value;
        }, GlobalSettings.Instance.FreezeDurationSeconds, 0, _damageChangeColorCurve, completeCallback: () =>
        {
            _anim.speed = 1;
            _canAct = true;
        });
    }
}

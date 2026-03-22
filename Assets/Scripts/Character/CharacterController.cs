using System;
using UnityEngine;
using System.Collections;
using Character;
using EventBusSystem;

public class CharacterController : BaseCharacterController
{
    private AttackResolver _attackResolver;
    
    protected override void Awake()
    {
        base.Awake();
        _attackResolver = new AttackResolver(_config);
        
        EventBus.Subscribe<OnClickRootEvent>(AttackRoot);
        EventBus.Subscribe<OnClickBeaverEvent>(KickBeaver);
        EventBus.Subscribe<OnClickItemEvent>(PickItem);
        EventBus.Subscribe<OnActiveItemChangedEvent>(OnItemChanged);
    }

    private void OnItemChanged(OnActiveItemChangedEvent e)
    {
        StopCurrentRoutine();
        
        _anim.SetInteger("weaponIndex", (int)e.Item.Type);
        _anim.Play("Default");

        _currentActiveItem = e.Item;
    }

    private void PickItem(OnClickItemEvent itemEvent)
    {
        if (!_isGrounded)
            return;
        
        StopCurrentRoutine();
        
        _currentRoutine = StartCoroutine(PickItemRoutine(itemEvent.Item, itemEvent.OnPick));
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

    private IEnumerator PickItemRoutine(InventoryItemObject itemObject, Action onPick)
    {
        yield return _movement.MoveToRoutine(itemObject.transform, 0.05f, _canAct);
        onPick?.Invoke();
    }

    #region Beaver

    public void KickBeaver(OnClickBeaverEvent clickBeaver)
    {
        if (!_canAct || !_isGrounded)
            return;
        
        StopCurrentRoutine();
        
        _currentRoutine = StartCoroutine(KickBeaverRoutine(clickBeaver.Beaver, clickBeaver.OnKick));
    }

    private IEnumerator KickBeaverRoutine(BeaverController beaver, Action onKick)
    {
        yield return _movement.MoveToRoutine(beaver.transform, 0.05f, _canAct);;
    
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

    #endregion

    #region Root
    private IEnumerator AttackRootRoutine(AttackContext context, Action onActionExecuted)
    {
        if (_currentActiveItem.Type != context.ItemType)
            yield break;

        _currentRoutineItemType = context.ItemType;

        yield return _movement.MoveToRoutine(context.TargetPositionX, 0.05f, _canAct);;

        LookAt(context.LookAtX);

        _anim.Play($"Action{context.ItemType}");

        _onActionExecuted = Execute;
        
        void Execute()
        {
            context.Target.TakeDamage((int)context.Damage);
            onActionExecuted?.Invoke();
        }
    }

    private void AttackRoot(OnClickRootEvent clickEvent)
    {
        if (!_isGrounded)
            return;

        StopCurrentRoutine();

        clickEvent.Root.OnDieEvent += StopCurrentRoutine;
        clickEvent.Root.OnLevelChangedEvent += StopCurrentRoutine;

        AttackContext context = _attackResolver.ResolveRoot(clickEvent.Root, transform.position);

        _currentRoutine = StartCoroutine(AttackRootRoutine(context, UnSubscribeRoot));

        void UnSubscribeRoot()
        {
            //clickEvent.Root.OnDieEvent -= StopCurrentRoutine;
            clickEvent.Root.OnLevelChangedEvent -= StopCurrentRoutine;
        }
    }
    #endregion

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnClickRootEvent>(AttackRoot);
        EventBus.Unsubscribe<OnClickBeaverEvent>(KickBeaver);
        EventBus.Unsubscribe<OnClickItemEvent>(PickItem);
        EventBus.Unsubscribe<OnActiveItemChangedEvent>(OnItemChanged);
    }
}
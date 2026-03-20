using System;
using System.Collections.Generic;
using AnimationHelper;
using Configs.Beaver;
using DIContainer;
using Entites;
using EventBusSystem;
using NewStateMachine;
using NewStateMachine.BeaverStates;
using UnityEngine;


public class BeaverController : BaseEntity, IDamagable
{
    [SerializeField] private AnimationEventReceiver _animationEventReceiver;
    [SerializeField] private Animator _animator;
    
    private float _speed = 2f;
    private bool _isGrounded;
    
    private ClickableObject _clickable;

    private BeaverConfig _config;
    private NewStateMachine.StateMachine _stateMachine;

    public event Action<bool> OnGroundChanged;
    
    [Inject]
    private void Construct(BeaverConfig config)
    {
        _config = config;
        _speed = _config.Speed;
        
        var states = new Dictionary<StateType, IState<StateDataBase>>
        {
            { StateType.Idle, new IdleState() },
            { StateType.Walk, new WalkState(_animator, transform, _speed) },
            { StateType.Fly, new BeaverFlyState(_animator) },
            { StateType.Attack, new AttackState(transform, _animator, _animationEventReceiver, _config.Damage, _config.AttackRange) },
            { StateType.Death, new DeathState(_animator, _animationEventReceiver, gameObject) }
        };
        
        _stateMachine = new NewStateMachine.StateMachine(states);
    }

    void Start()
    {
        _clickable = GetComponent<ClickableObject>();

        _clickable.ClickEvent.AddListener(() =>
        {
            EventBus.Publish(new OnClickBeaverEvent(this, Die));
        });
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            _stateMachine.ChangeState(new StateDataBase(StateType.Walk));
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            _stateMachine.ChangeState(new StateDataBase(StateType.Fly));
        }
    }

    private void OnEnable()
    {
        _animator.SetBool("isAlive", true);
        _stateMachine.ChangeState(new StateDataBase(StateType.Fly));
    }

    void FixedUpdate() => _stateMachine?.FixedUpdate();

    private void Die()
    {
        // ----------- CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForBeaver);
        // ----------- CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForBeaver);
        _stateMachine.ChangeState(new StateDataBase(StateType.Death));
    }

    public void TakeDamage(int damage)
    {
        
    }

    private void OnDisable()
    {
        _stateMachine?.StopStateMachine();
    }
}

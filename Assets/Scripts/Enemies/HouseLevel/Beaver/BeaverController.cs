using System;
using System.Collections.Generic;
using AnimationHelper;
using Configs.Beaver;
using DIContainer;
using Entites;
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
    private StateMachine _stateMachine;
    
    [Inject]
    private void Construct(BeaverConfig config)
    {
        _config = config;
        _speed = _config.Speed;
        
        var states = new Dictionary<StateType, IState<StateDataBase>>
        {
            { StateType.Idle, new IdleState() },
            { StateType.Walk, new WalkState(_animator, transform, _speed) },
            { StateType.Attack, new AttackState(transform, _animator, _animationEventReceiver, _config.Damage, _config.AttackRange) },
            { StateType.Death, new DeathState(_animator, _animationEventReceiver, gameObject) }
        };
        
        _stateMachine = new StateMachine(states);
    }
    
    void Start()
    {
        _clickable = GetComponent<ClickableObject>();

        _clickable.ClickEvent.AddListener(() =>
        {
            //_player.KickBeaver(this, Die);
            _stateMachine.ChangeState(new StateDataBase(StateType.Death));
        });
    }

    private void OnEnable()
    {
        SetDefault();
    }

    void Update() => _stateMachine?.FixedUpdate();

    // private void Die()
    // {
    //     // ----------- CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForBeaver);
    //     // ----------- CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForBeaver);
    //     _animator.Play("BeaverDie");
    // }

    public void SetDefault()
    {
        _stateMachine?.ChangeState(new WalkStateData());
    }

    public void TakeDamage(int damage)
    {
        
    }

    private void OnDisable()
    {
        _stateMachine?.StopStateMachine();
    }
}

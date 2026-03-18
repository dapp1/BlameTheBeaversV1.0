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
    
    private float _speed = 2f;
    private bool _isGrounded;
    private Animator _anim;
    private ClickableObject _clickable;

    private BeaverConfig _config;
    private StateMachine _stateMachine;
    
    [Inject]
    private void Construct(BeaverConfig config)
    {
        _config = config;
        _speed = _config.Speed;
    }
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        _clickable = GetComponent<ClickableObject>();
        
        // _clickable.ClickEvent.AddListener(() =>
        // {
        //     _player.KickBeaver(this, Die);
        // });

        var states = new Dictionary<StateType, IState<StateDataBase>>
        {
            { StateType.Idle, new IdleState() },
            { StateType.Walk, new WalkState(_anim, transform, _speed) },
            { StateType.Attack, new AttackState(transform, _anim, _animationEventReceiver, 0) }
        };

        _stateMachine = new StateMachine(states);
    }
    
    void Update()
    {
        _stateMachine.FixedUpdate();
    }

    private void Die()
    {
        // ----------- CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForBeaver);
        // ----------- CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForBeaver);
        _anim.Play("BeaverDie");
    }

    //Called from animation event
    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}

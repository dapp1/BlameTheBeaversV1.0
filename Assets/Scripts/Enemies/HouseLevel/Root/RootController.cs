using System;
using System.Collections;
using Assets.Scripts.Events;
using Configs.Root;
using DIContainer;
using Entites;
using Nora.NEvent;
using EventBusSystem;
using Pixelplacement;
using UnityEngine;
using Random = UnityEngine.Random;

public class RootController : BaseEntity, IDamagable
{
    private int _houseDamage;

    [SerializeField] private AnimationCurve _damageChangeColorCurve;
    [SerializeField] private SpriteRenderer _topRenderer;
    [SerializeField] private SpriteRenderer _bottomRenderer;
    [SerializeField] private GameObject _beaverPrefab;
    
    private float _health;
    private GameObject _beaver;
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    private Animator _anim;
    private ClickableObject _clickable;

    private int _level;
    private bool _canGrow;
    
    private RootConfig _config;

    private NewStateMachine.StateMachine _stateMachine;

    public event Action OnLevelChangedEvent;
    public event Action OnDieEvent;
    
    public int CurrentLevel => _level;

    [Inject]
    private void Construct(RootConfig config)
    {
        _config = config;

        _health = _config.RootInitialHealth;
        _houseDamage = _config.HouseDamage;
    }

    private void OnClick()
    {
        EventBus.Publish(new OnClickRootEvent(this));
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
        _clickable = GetComponent<ClickableObject>();

        _clickable.ClickEvent.AddListener(OnClick);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            _col.isTrigger = true;
            _rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(Grow());
            
            _anim.Play("startGrowing");
        }
    }

    //Корутина роста
    private IEnumerator Grow()
    {
        var growTimeRange = _config.GrowRangeSeconds;
        
        //Пока уровень дерева меньше 4
        while (true){
            //Ждем сколько-то секунд
            yield return new WaitForSeconds(Random.Range(growTimeRange.x, growTimeRange.y));
            //Разрешаем включить анимацию роста
            _anim.SetBool("canGrow", true);
        }
    }
    
    //Called from animation event
    public void OnLevelChanged()
    {
        _level++;
        _anim.SetBool("canGrow", false);

        _health += _config.LevelUpRootHealing;

        OnLevelChangedEvent?.Invoke();
        
        if (_level == 4)
        {
            NEventManager.StartEvent(new HouseDamageEvent(_houseDamage));
            StopAllCoroutines();
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            // --------------- CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForRoot);
            // --------------- CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForRoot);
            Die();
        }
        else if (_level > 0)
        {
            Tween.Value(Color.white, Color.red, (Color value) =>
            {
                _topRenderer.color = value;
                _bottomRenderer.color = value;
            }, 0.2f, 0, _damageChangeColorCurve);
        }
    }

    private void Die()
    {
        OnDieEvent?.Invoke();
        gameObject.SetActive(false);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _col.isTrigger = false;
        OnDieEvent = null;
    }
}

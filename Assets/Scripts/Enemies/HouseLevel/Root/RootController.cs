using System.Collections;
using Assets.Scripts.Events;
using Configs.General;
using Configs.Root;
using DIContainer;
using EntityFactory;
using Nora.NEvent;
using Pixelplacement;
using UnityEngine;

public class RootController : MonoBehaviour 
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
    private bool _isDead;
    
    private RootConfig _config;
    private GeneralConfig _configGeneral;
    private IEntityFactory _entityFactory;
    
    [Inject]
    private void Construct(RootConfig config, GeneralConfig generalConfig, IEntityFactory entityFactory)
    {
        _config = config;
        _configGeneral = generalConfig;
        _entityFactory = entityFactory;
        
        _health = _config.RootInitialHealth;
        _houseDamage = _config.HouseDamage;
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
        _clickable = GetComponent<ClickableObject>();

        _clickable.ClickEvent.AddListener(OnClick);
    }

    private void OnClick()
    {
        var playerPosition = CharacterController.Instance.transform.position;
        var positionX = transform.position.x < playerPosition.x
            ? transform.position.x + 0.6f
            : transform.position.x - 0.6f;
        
        if (_level == 0)
        {
            CharacterController.Instance.AttackRoot("Hands", positionX, transform.position.x, () =>
            {
                GetDamage(_config.DamageByHands);
                
                if (_level > 0 || _isDead)
                    CharacterController.Instance.StopCurrentRoutine();
            });
        }
        else if (_level == 1){
            CharacterController.Instance.AttackRoot("Shovel", positionX, transform.position.x,() =>
            {
                GetDamage(_config.DamageByShovel);
                
                if (_level > 1 || _isDead)
                    CharacterController.Instance.StopCurrentRoutine();
            });
        }
        else if (_level < 5)
        {
            CharacterController.Instance.AttackRoot("Axe",positionX, transform.position.x,() =>
            {
                GetDamage(_config.DamageByAxe);
                
                if (_isDead)
                    CharacterController.Instance.StopCurrentRoutine();
            });
        }
    }

    private void GetDamage(float damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            // --------------- CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForRoot);
            // --------------- CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForRoot);
            
            _isDead = true;
            
            Destroy(gameObject);
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
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            _col.isTrigger = true;
            //Удалить rigidbody (чтоб больше не падал)
            Destroy(_rb);
            //Начать корутину роста
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
        //Повышаем уровень. Запрещаем расти.
        _level++;
        _anim.SetBool("canGrow", false);

        //Лечимся
        _health += _config.LevelUpRootHealing;

        if (_level == 4)
        {
            NEventManager.StartEvent(new HouseDamageEvent(_houseDamage));
            StopAllCoroutines();
            StartCoroutine(BeaverSpawn());
        }
    }

    private IEnumerator BeaverSpawn()
    {
        var spawnRange = _config.BeaversSpawnRangeSeconds;
        
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnRange.x, spawnRange.y));
            // --------------- BeaverSpawnController.Instance.TrySpawnBeaverFromPool(new Vector2(transform.position.x, -3.55f));
            _entityFactory.CreateEntity(EntityType.Beaver, new Vector2(transform.position.x, -3.55f));
        }
    }
}

using System.Collections;
using Assets.Scripts.Events;
using Configs;
using Configs.BeaverShip;
using DIContainer;
using EntityFactory;
using Nora.NEvent;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeaverShip : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationCurve _damageChangeColorCurve;
    [SerializeField] private Transform[] _points;

    private int _health;
    private int _pointIndex;
    private float _defaultSpeed;

    private SpriteRenderer _renderer;
    private IEntityFactory _entityFactory;
    private BeaverShipConfig _config;
    private TempoConfig _configTempo;
    
    [Inject]
    private void Construct(BeaverShipConfig config, TempoConfig configTempo, IEntityFactory entityFactory)
    {
        _config = config;
        _configTempo = configTempo;
        _entityFactory = entityFactory;
        
        _health = _config.Health;
        _defaultSpeed = Random.Range(_config.SpeedRange.x, _config.SpeedRange.y);
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start() => StartCoroutine(RootSpawn());

    private void Update() => MoveTo();

    private void MoveTo()
    {
        int direction = transform.position.x < _points[_pointIndex].position.x ? 1 : -1;
        _animator.SetInteger("direction", direction);
        
        transform.position = Vector2.MoveTowards(
            transform.position, 
            _points[_pointIndex].position, 
            _defaultSpeed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, _points[_pointIndex].position) <= 0.01f)
        {
            _defaultSpeed = Random.Range(_config.SpeedRange.x, _config.SpeedRange.y);
            _pointIndex = Random.Range(0, _points.Length);
        }
    }
    
    private IEnumerator RootSpawn()
    {
        var startRange = _config.RootsSpawnRangeSecondsStart;
        var endRange = _config.RootsSpawnRangeSecondsEnd;
    
        while (true)
        {
            //TODO: Refactoring this
            var currentRangeX = TempoController.Instance.EvaluateFloatByDifficulty(startRange.x, endRange.x);
            var currentRangeY = TempoController.Instance.EvaluateFloatByDifficulty(startRange.y, endRange.y);
    
            yield return new WaitForSeconds(Random.Range(currentRangeX, currentRangeY));
            
            if (Random.Range(0f, 100f) < _config.BeaverChance)
            {
                _entityFactory.CreateEntity(EntityType.Beaver,
                    new Vector2(transform.position.x, transform.position.y - 0.7f), 5);
            }
            else
            {
                _entityFactory.CreateEntity(EntityType.Root,
                    new Vector2(transform.position.x, transform.position.y - 0.7f), -1);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        //TODO: Refactoring this
        if (!col.CompareTag("throwAxe"))
            return;
    
        _health--;
        if (_health <= 0)
        {
            //Play Death
            //Temp end game
            NEventManager.StartEvent(new EndGameEvent(EndGameReason.BobroletIsDead));
        }
        
        Pixelplacement.Tween.Value(Color.white, Color.red, (Color value) =>
        {
            _renderer.color = value;
        }, 0.2f, 0, _damageChangeColorCurve);
    }
}
using System.Collections;
using Assets.Scripts.Events;
using Configs.BeaverShip;
using DIContainer;
using Nora.NEvent;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeaverShip : MonoBehaviour
{
    //TODO: Inject with future container
    [Inject] private BeaverShipConfig _config;
    
    [SerializeField] private Animator _animator;

    [SerializeField] private AnimationCurve _damageChangeColorCurve;
    [SerializeField] private Transform[] _points;

    private int _health;
    private int _pointIndex;
    private float _defaultSpeed;
    
    private SpriteRenderer _renderer;
    
    private void Awake()
    {
        _health = _config.Health;
        _renderer = GetComponent<SpriteRenderer>();
        _defaultSpeed = Random.Range(_config.SpeedRange.x, _config.SpeedRange.y);
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
        var startRange = GlobalSettings.Instance.RootsSpawnRangeSecondsStart;
        var endRange = GlobalSettings.Instance.RootsSpawnRangeSecondsEnd;
    
        while (true)
        {
            //TODO: Refactoring this
            var currentRangeX = TempoController.Instance.EvaluateFloatByDifficulty(startRange.x, endRange.x);
            var currentRangeY = TempoController.Instance.EvaluateFloatByDifficulty(startRange.y, endRange.y);
    
            yield return new WaitForSeconds(Random.Range(currentRangeX, currentRangeY));

            GameObject go;
            
            if (Random.Range(0f, 100f) < _config.BeaverChance)
            {
                go = Instantiate(_config.BeaverPrefab);
                go.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
            }
            else
            {
                go = Instantiate(_config.RootPrefab);
                go.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
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
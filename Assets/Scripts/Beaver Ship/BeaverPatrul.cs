using System;
using System.Collections;
using Assets.Scripts.Events;
using Mono.Cecil;
using Nora.NEvent;
using Pixelplacement;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeaverPatrul : MonoBehaviour
{

    [SerializeField] private GameObject _rootPrefab;

    private float _defaultSpeed;
    [SerializeField] int _hp;
    
    [SerializeField] private GameObject _beaverPrefab;
    [SerializeField] private float _chanceSpawnBeaver;

    [SerializeField] private AnimationCurve _damageChangeColorCurve;
    
    private SpriteRenderer _renderer;
    
    private GameObject _root;
    private GameObject _beaver;
   [SerializeField] private Transform[] _points;
    private int _randomPoint;
    private Animator _anim;
    private int _direction;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        StartCoroutine(RootSpawn());

        _randomPoint = 0;

        _anim = GetComponent<Animator>();

        _defaultSpeed = GlobalSettings.Instance.BobroletDefaultSpeed;
        _hp = GlobalSettings.Instance.BobroletInitialHP;
    }


    void Update()
    {
        if (transform.position.x < _points[_randomPoint].position.x)
        {
            _direction = 1;
            _anim.SetInteger("direction", _direction);
        }
        else
        {
            _direction = -1;
            _anim.SetInteger("direction", _direction);
        }

        transform.position = Vector2.MoveTowards(transform.position, _points[_randomPoint].position, _defaultSpeed * Time.deltaTime);

        var speedRange = GlobalSettings.Instance.BobroletSpeedRange;

        if (transform.position == _points[_randomPoint].position)
        {
            _defaultSpeed = Random.Range(speedRange.x, speedRange.y);
            _randomPoint = Random.Range(0, _points.Length);

        }
    }

    private IEnumerator RootSpawn()
    {
        var startRange = GlobalSettings.Instance.RootsSpawnRangeSecondsStart;
        var endRange = GlobalSettings.Instance.RootsSpawnRangeSecondsEnd;

        while (true)
        {
            var currentRangeX = TempoController.Instance.EvaluateFloatByDifficulty(startRange.x, endRange.x);
            var currentRangeY = TempoController.Instance.EvaluateFloatByDifficulty(startRange.y, endRange.y);

            yield return new WaitForSeconds(Random.Range(currentRangeX, currentRangeY));
            
            if (Random.Range(0f, 100f) < _chanceSpawnBeaver)
            {
                _beaver = Instantiate(_beaverPrefab);
                _beaver.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
            }
            else
            {
                _root = Instantiate(_rootPrefab);
                _root.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("throwAxe"))
            return;

        _hp--;
        if (_hp <= 0)
        {
            //Play Death
            //Temp end game
            NEventManager.StartEvent(new EndGameEvent(EndGameReason.BobroletIsDead));
        }

        Tween.Value(Color.white, Color.red, (Color value) =>
        {
            _renderer.color = value;
        }, 0.2f, 0, _damageChangeColorCurve);
    }

    public IEnumerator BeaverSpawn ()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _chanceSpawnBeaver = 100f;
            if (Random.Range(0f, 100f) < _chanceSpawnBeaver)
            {
                _beaver = Instantiate(_beaverPrefab);
                _beaver.transform.position = new Vector2(transform.position.x, transform.position.y - 0.7f);
            }
        }
    }
}

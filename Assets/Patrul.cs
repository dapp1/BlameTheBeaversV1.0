using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Patrul : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Transform[] moveSpot;
    private int _randomSpot;
    private int _direction;
    private Animator _anim;
    
    void Start()
    {
        _randomSpot = 0;
        _anim = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.x < moveSpot[_randomSpot].position.x)
        {
            _direction = 1;
            _anim.SetInteger("direction", _direction);
        }
        else
        {
            _direction = -1;
            _anim.SetInteger("direction", _direction);
        }

        transform.position = Vector2.MoveTowards(transform.position, moveSpot[_randomSpot].position, _speed * Time.deltaTime);

        if(transform.position == moveSpot[_randomSpot].position)
        {
            _randomSpot = Random.Range(0, moveSpot.Length);
        }
    }
}

using Assets.Scripts.Events;
using Nora.NEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    private Animator _anim;
    private int _health = 100;
    public HealthBarController healthBarController;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        //StartCoroutine(HealthDown());

        NEventManager.Subscribe<HouseDamageEvent>(x =>
        {
            if (_health > 0)
            {
                _health = _health - x.Data.Data;
                healthBarController.SetHealth(_health);
                _anim.SetInteger("HouseHealth", _health);

                if (_health <= 0)
                    NEventManager.StartEvent(new EndGameEvent(EndGameReason.HouseInRuin));
            }
        }
        );
    }


    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Test method to get bar down
    /// </summary>
    /// <returns></returns>
    private IEnumerator HealthDown()
    {
        while (_health > 0)
        {
            yield return new WaitForSeconds(0.5f);
            _health--;
            _anim.SetInteger("HouseHealth", _health);
            healthBarController.SetHealth(_health);
        }
    }
}
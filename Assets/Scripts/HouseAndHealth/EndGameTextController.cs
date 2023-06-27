using Assets.Scripts.Events;
using Nora.NEvent;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameTextController : MonoBehaviour
{
    [SerializeField]
    private bool _enabledEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        if ( _enabledEnd)
        {
            NEventManager.Subscribe<EndGameEvent>(x =>
            {
                Time.timeScale = 0.0f;
                if (x.Data.Data == EndGameReason.HouseInRuin)
                    MenuChanger.Instance.DeathScreen();
                else if (x.Data.Data == EndGameReason.ScoreAchived || x.Data.Data == EndGameReason.BobroletIsDead)
                    MenuChanger.Instance.CongratulationsScreen();
            }
            );
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

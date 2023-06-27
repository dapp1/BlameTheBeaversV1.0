using System.Collections;
using UnityEngine;
using TMPro;
using Nora.NEvent;
using Assets.Scripts.Events;

public class Timer : MonoBehaviour
{
    private int _sec = 0;
    private int _min = 0;
    
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Start()
    {
        StartCoroutine(ITimer());
    }

    private IEnumerator ITimer()
    {
        while (true)
        {
            _sec += 1;
            
            if (_sec == 60)
            {
                _min++;
                _sec = 0;
            }
            
            CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForSecond);

            _text.text = _min.ToString("D2") + ":" + _sec.ToString("D2");
            
            yield return new WaitForSeconds(1);
        }
    }

}

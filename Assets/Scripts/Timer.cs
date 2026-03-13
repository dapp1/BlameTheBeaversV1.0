using System.Threading;
using System.Threading.Tasks;
using Configs.General;
using DIContainer;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _seconds = 0;
    private int _minutes = 0;

    private CancellationTokenSource _ctk;

    //TODO: Change script
    [Inject] private GeneralConfig _config;
    
    private void Start()
    {
        _ = StartTimer();
    }

    private async Task StartTimer()
    {
        while (!_ctk.IsCancellationRequested)
        {
            _seconds++;
        
            if (_seconds == 60)
            {
                _minutes++;
                _seconds = 0;
            }
        
            CoinsAndScoreController.Instance.ChangeScoreValue(_config.ScoreForSecond);
            _text.text = _minutes.ToString("D2") + ":" + _seconds.ToString("D2");
            await Task.Delay(1000);
        }
    }

}

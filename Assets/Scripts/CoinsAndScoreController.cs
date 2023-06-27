using Assets.Scripts.Events;
using Nora.NEvent;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using TMPro;
using UnityEngine;

public class CoinsAndScoreController : Singleton<CoinsAndScoreController>
{
    [SerializeField] TextMeshProUGUI _textCoins;
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] private Animator _anim;

    private int _animationScoreValue = 0;
    public int Score => _animationScoreValue;

    private int _coinsCount;
    private int _scoreCount;
    
    private TweenBase _coinsUpdateTween;
    private TweenBase _scoreUpdateTween;
    
    private void Awake()
    {
        _coinsCount = GlobalSettings.Instance.InitialCoins;
        _textCoins.SetText(_coinsCount.ToString());
    }

    public bool ChangeCoinsValue(int value)
    {
        if (_coinsCount + value < 0)
            return false;
        
        _anim.Play("CoinsChanged");
        
        var oldValue = _coinsCount;
        _coinsCount += value;
   
        if (_coinsUpdateTween?.Status == Tween.TweenStatus.Running)
            _coinsUpdateTween.Stop();

        _coinsUpdateTween = Tween.Value(oldValue, _coinsCount, (val) =>
        {
            _textCoins.SetText(val.ToString());
        }, 1, 0);

        return true;
    }

    public void ChangeScoreValue(int value)
    {
        var oldValue = _scoreCount;
        _scoreCount += value;
        
        if (_scoreUpdateTween?.Status == Tween.TweenStatus.Running)
            _scoreUpdateTween.Stop();

        _scoreUpdateTween = Tween.Value(oldValue, _scoreCount, (val) =>
        {
            _textScore.SetText($"Score: {val}");
            _animationScoreValue = val;
        }, 1, 0);

        if (_animationScoreValue >= GlobalSettings.Instance.NeededScore)
            NEventManager.StartEvent(new EndGameEvent(EndGameReason.ScoreAchived));
    }
}

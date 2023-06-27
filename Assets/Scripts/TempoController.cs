using Pixelplacement;

public class TempoController : Singleton<TempoController>
{
    public float EvaluateFloatByDifficulty(float start, float end)
    {
        var tempoPercent = (float)CoinsAndScoreController.Instance.Score / GlobalSettings.Instance.NeededScore;
        var currentDifficulty = GlobalSettings.Instance.TempoCurve.Evaluate(tempoPercent);
        return end + ((start - end) * (1 - currentDifficulty));
    }
}

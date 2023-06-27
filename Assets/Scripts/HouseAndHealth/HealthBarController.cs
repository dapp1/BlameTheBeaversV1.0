using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private Image _hpBar;

    [SerializeField]
    private TextMeshProUGUI _text;
    
    void Start()
    {
        _hpBar.fillAmount = 1;
    }

    public void SetHealth(int health)
    {
        _hpBar.fillAmount = Mathf.Clamp(health * 0.01f, 0, 1);
        _text.text = $"{health}%";
    }
}

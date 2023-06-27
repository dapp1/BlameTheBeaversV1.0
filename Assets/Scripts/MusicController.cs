using Pixelplacement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : Singleton<MusicController>
{
    [SerializeField] private AudioClip _mainTheme;
    [SerializeField] private AudioClip _menuTheme;

    [SerializeField] private Slider _musicSlider1;
    [SerializeField] private Slider _musicSlider2;
    
    [SerializeField] private AudioMixer _mixer;
    
    private AudioSource _source;

    private static float _musicVolume;
    private static float _musicValue = 0.7f;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        _musicSlider1.onValueChanged.AddListener(ChangeMusicVolume);
        _musicSlider2.onValueChanged.AddListener(ChangeMusicVolume);
    }

    private void Start()
    {
        UpdateMusicVolume();
    }

    public void UpdateMusicVolume()
    {
        ChangeMusicVolume(_musicValue);
    }
    
    private void ChangeMusicVolume(float value)
    {
        _musicValue = value;
        _musicVolume = (_musicValue * 80) - 80;
        
        _musicSlider1.value = value;
        _musicSlider2.value = value;
        
        _mixer.SetFloat ("MusicVolume", _musicVolume);
    }

    public void PlayMainTheme()
    {
        _source.clip = _mainTheme;
        _source.loop = true;
        _source.Play();
    }

    public void PlayMenuTheme()
    {
        _source.clip = _menuTheme;
        _source.loop = true;
        _source.Play();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChanger : Pixelplacement.Singleton<MenuChanger>
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _settingsPause;
    [SerializeField] private GameObject _exit;
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _congratulationsScreen;
    [SerializeField] private GameObject _tutorialScreen;

    private static bool _isTutorialShown = false;

    private bool _isPlaying = false;
    private bool _inSettings = false;
    private bool _inPauseSettings = false;
    private bool _inExit = false;
    public static bool GameIsPaused = false;
    private Blur blur;
    private void Awake()
    {
        Time.timeScale = 0.0f;
        _timer.SetActive(false);
        _HUD.SetActive(false);

        blur = FindObjectOfType<Blur>();
        blur.enabled = true;
    }

    private void Update()
    {
        if (_tutorialScreen.gameObject.activeSelf && Input.anyKeyDown)
        {
            _tutorialScreen.gameObject.SetActive(false);
            _isTutorialShown = true;

            MenuToPlay();
        }

        if (_inSettings == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SettingsToMain();
            }
        }
        else if (_inExit == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NoExit();
            }
        } else if (_inPauseSettings == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
        else
        {
            if (_isPlaying == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (GameIsPaused)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
        }
    }

    public void MenuToPlay()
    {
        _mainMenu.SetActive(false);

        if (_isTutorialShown)
        {
            blur.enabled = false;
            Time.timeScale = 1.0f;
            _timer.SetActive(true);
            _HUD.SetActive(true);
            _isPlaying = true;

            CameraController.Instance.Slide();
            MusicController.Instance.PlayMainTheme();
        }
        else
            _tutorialScreen.gameObject.SetActive(true);
    }

    public void MenuToSettings()
    {
        _inSettings= true;
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        MusicController.Instance.UpdateMusicVolume();
    }

    public void SettingsToMain()
    {
        _inExit = false;
        _inSettings = false;
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
    }

    public void MenuToExit()
    {
        _inExit = true;
        _exit.SetActive(true);
    }

    public void NoExit()
    {
        _inExit = false;
        _inSettings = false;
        _exit.SetActive(false);
    }


    public void YesExit()
    {
        Application.Quit();
        Debug.Log("sdsd");
    }

    public void Resume()
    {
        blur.enabled = false;
        _mainMenu.SetActive(false);
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        _HUD.SetActive(true);
    }

    private void Pause()
    {
        blur.enabled = true;
        _mainMenu.SetActive(false);
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        _HUD.SetActive(false);
    }

    public void PauseSettings()
    {
        _isPlaying= false;
        _inPauseSettings = true;
        _pauseMenuUI.SetActive(false);
        _settingsPause.SetActive(true);
        MusicController.Instance.UpdateMusicVolume();
    }

    public void Back()
    {
        _isPlaying = true;
        _inPauseSettings = false;
        _pauseMenuUI.SetActive(true);
        _settingsPause.SetActive(false);
    }

    public void DeathScreen()
    {
        blur.enabled = true;
        _isPlaying = false;
        Time.timeScale = 0.0f;
        _deathScreen.SetActive(true);
    }

    public void CongratulationsScreen()
    {
        blur.enabled = true;
        _isPlaying = false;
        Time.timeScale = 0.0f;
        _congratulationsScreen.SetActive(true);
    }

    public void Leave()
    {
        // Reload Scene with menu
        ResetGameController.Instance.ReloadScene(true);
    }

    public void Restart()
    {
        // Reload Scene without menu
        ResetGameController.Instance.ReloadScene(false);
    }
}

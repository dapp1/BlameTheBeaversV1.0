using Pixelplacement;
using UnityEngine.SceneManagement;

public class ResetGameController : Singleton<ResetGameController>
{
    private static bool _showMainMenu;
    private static bool _isInitialized;
    public static bool isFirstRun = true;
    
    private void Awake()
    {
        if (!_isInitialized)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _isInitialized = true;
            _showMainMenu = true;
        }
    }

    public void ReloadScene(bool showMainMenu = true)
    {
        _showMainMenu = showMainMenu;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_showMainMenu)
        {
            MenuChanger.Instance.MenuToPlay();
        }
        else 
            MusicController.Instance.PlayMenuTheme();
    }
}

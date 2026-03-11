#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class SceneAutoLoader
{
    private const bool isEnabledAutoLoader = true;
    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets executed.
    static SceneAutoLoader()
    {
        if (isEnabledAutoLoader)
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

    }

    // Play mode change callback handles the scene load/reload.
    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (isEnabledAutoLoader)
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // User pressed play -- autoload master scene.
                PreviousScene = EditorSceneManager.GetActiveScene().path;

                if (PreviousScene.Contains("SplashScreen") == false)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        try
                        {
                            string masterScene = PreviousScene.Replace(EditorSceneManager.GetActiveScene().name,
                                "SplashScreen"); //project relative instead of absolute path
                            EditorSceneManager.OpenScene(masterScene);
                        }
                        catch
                        {
                            Debug.LogError("error: scene not found: " +
                                           EditorSceneManager.GetSceneByBuildIndex(0).path);
                            EditorApplication.isPlaying = false;

                        }
                    }
                    else
                    {
                        // User cancelled the save operation -- cancel play as well.
                        EditorApplication.isPlaying = false;
                    }
                }
            }

            // isPlaying check required because cannot OpenScene while playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // User pressed stop -- reload previous scene.
                if (string.IsNullOrEmpty(PreviousScene) == false)
                {
                    try
                    {
                        EditorSceneManager.OpenScene(PreviousScene);
                    }
                    catch
                    {
                        Debug.LogError(string.Format("error: scene not found: {0}", PreviousScene));
                    }
                }
            }
        }
    }

    private const string cEditorPrefPreviousScene = "SceneAutoLoader.PreviousScene";

    private static string PreviousScene
    {
        get { return EditorPrefs.GetString(cEditorPrefPreviousScene, EditorSceneManager.GetActiveScene().path); }
        set { EditorPrefs.SetString(cEditorPrefPreviousScene, value); }
    }
}
#endif

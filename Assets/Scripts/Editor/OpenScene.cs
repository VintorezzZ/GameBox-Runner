#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenScene
{
    [MenuItem("Open Scene/Launcher", false, 1)]
    public static void OpenLauncher()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
    }
    
    [MenuItem("Open Scene/Client", false, 2)]
    public static void OpenClient()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Client.unity");
    }
    
    [MenuItem("Open Scene/Gameplay", false, 3)]
    public static void OpenServer()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Gameplay.unity");
    }
   
}
#endif
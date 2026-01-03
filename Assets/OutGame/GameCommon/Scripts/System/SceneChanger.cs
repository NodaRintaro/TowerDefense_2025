using UnityEngine.SceneManagement;
using VContainer;

public static class SceneChanger
{
    public static void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

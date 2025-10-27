using UnityEngine.SceneManagement;
using VContainer;

public class SceneChanger
{
    [Inject]
    public SceneChanger() { }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void SceneSwitch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

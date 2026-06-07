using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void SceneSwitch(int classId)
    {
        selectionClass.classSelection = classId;
        SceneManager.LoadScene("Board");
    }
}

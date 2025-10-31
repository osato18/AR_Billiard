using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("Main");  // シーン名で指定
    }
}

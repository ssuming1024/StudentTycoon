using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "PlayGame";

    public void GoMain()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
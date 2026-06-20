using UnityEngine;
using UnityEngine.SceneManagement;

public class BackSceneButton : MonoBehaviour
{
    [SerializeField] private string backSceneName = "PlayGame";

    public void GoBack()
    {
        SceneManager.LoadScene(backSceneName);
    }
}
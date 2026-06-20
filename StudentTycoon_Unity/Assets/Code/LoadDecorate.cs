using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneLoader : MonoBehaviour
{
    public void GoToDecoratePlayer()
    {
        SceneManager.LoadScene("PlayGame");
    }
}
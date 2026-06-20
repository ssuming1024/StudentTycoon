using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    [SerializeField] private string yesSceneName = "Ending_1";
    [SerializeField] private string noSceneName = "Ending_2";

    public void ChooseAnswer(bool isYes)
    {
        if (isYes)
        {
            SceneManager.LoadScene(yesSceneName);
        }
        else
        {
            SceneManager.LoadScene(noSceneName);
        }
    }
}
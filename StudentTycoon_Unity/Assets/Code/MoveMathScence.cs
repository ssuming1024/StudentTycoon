using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveButton : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    public string sceneName = "MathTest";

    public void MoveScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneMoveButton] sceneName이 비어있습니다.");
            return;
        }

        Debug.Log($"[SceneMoveButton] 씬 이동: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
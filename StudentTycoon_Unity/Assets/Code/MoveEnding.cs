using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingJudge : MonoBehaviour
{
    [Header("Ending Scene Names")]
    public string goodEndingSceneName = "Ending_1";
    public string badEndingSceneName = "Ending_2";

    [Header("Average Setting")]
    public float passAverage = 5f;

    [Header("Optional UI")]
    public TMP_Text averageText;
    public TMP_Text resultText;

    private readonly string[] statKeys =
    {
        "Programming",
        "Korean",
        "Math",
        "Society",
        "Art",
        "Health",
        "Science",
        "Music"
    };

    public void CheckEnding()
    {
        int total = 0;

        for (int i = 0; i < statKeys.Length; i++)
        {
            int statValue = PlayerPrefs.GetInt(statKeys[i], 0);
            total += statValue;

            Debug.Log($"{statKeys[i]} Ω∫≈»: {statValue}");
        }

        float average = (float)total / statKeys.Length;

        Debug.Log($"Ω∫≈» √—«’: {total}");
        Debug.Log($"Ω∫≈» ∆Ú±’: {average}");

        if (averageText != null)
        {
            averageText.text = $"∆Ú±’ ¡°ºˆ: {average:F1}";
        }

        if (average >= passAverage)
        {
            if (resultText != null)
            {
                resultText.text = $"∆Ú±’ {average:F1}¡°! Ending_1∑Œ ¿Ãµø";
            }

            SceneManager.LoadScene(goodEndingSceneName);
        }
        else
        {
            if (resultText != null)
            {
                resultText.text = $"∆Ú±’ {average:F1}¡°... Ending_2∑Œ ¿Ãµø";
            }

            SceneManager.LoadScene(badEndingSceneName);
        }
    }
}
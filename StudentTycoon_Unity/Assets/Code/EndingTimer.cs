using UnityEngine;
using UnityEngine.SceneManagement;

public class CareerEndingJudge : MonoBehaviour
{
    [Header("Special Ending Threshold")]
    public int specialEndingScore = 100;

    [Header("Average Ending Setting")]
    public float passAverage = 5f;

    [Header("Normal Ending Scenes")]
    public string goodEndingSceneName = "Ending_1";
    public string badEndingSceneName = "Ending_2";

    [Header("Special Career Ending Scenes")]
    public string programmingEndingSceneName = "Ending_Programming";
    public string koreanEndingSceneName = "Ending_Korean";
    public string mathEndingSceneName = "Ending_Math";
    public string societyEndingSceneName = "Ending_Society";
    public string artEndingSceneName = "Ending_Art";
    public string scienceEndingSceneName = "Ending_Science";
    public string healthEndingSceneName = "Ending_BadmintonPlayer";
    public string musicEndingSceneName = "Ending_Music";

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
        string bestSubject = "";
        int bestScore = -1;
        int total = 0;

        for (int i = 0; i < statKeys.Length; i++)
        {
            string key = statKeys[i];
            int value = PlayerPrefs.GetInt(key, 0);

            total += value;

            if (value > bestScore)
            {
                bestScore = value;
                bestSubject = key;
            }

            Debug.Log($"{key}: {value}");
        }

        float average = (float)total / statKeys.Length;

        Debug.Log($"가장 높은 과목: {bestSubject}, 점수: {bestScore}");
        Debug.Log($"평균 점수: {average}");

        // 1순위: 특수 진로 엔딩
        if (bestScore >= specialEndingScore)
        {
            string specialScene = GetSpecialEndingScene(bestSubject);

            if (!string.IsNullOrEmpty(specialScene))
            {
                Debug.Log($"특수 진로 엔딩 이동: {specialScene}");
                SceneManager.LoadScene(specialScene);
                return;
            }
        }

        // 2순위: 일반 평균 엔딩
        if (average >= passAverage)
        {
            Debug.Log($"평균 {average}점 → 일반 좋은 엔딩");
            SceneManager.LoadScene(goodEndingSceneName);
        }
        else
        {
            Debug.Log($"평균 {average}점 → 일반 나쁜 엔딩");
            SceneManager.LoadScene(badEndingSceneName);
        }
    }

    private string GetSpecialEndingScene(string subject)
    {
        switch (subject)
        {
            case "Programming":
                return programmingEndingSceneName;

            case "Korean":
                return koreanEndingSceneName;

            case "Math":
                return mathEndingSceneName;

            case "Society":
                return societyEndingSceneName;

            case "Art":
                return artEndingSceneName;

            case "Science":
                return scienceEndingSceneName;

            case "Health":
                return healthEndingSceneName;

            case "Music":
                return musicEndingSceneName;

            default:
                return "";
        }
    }
}
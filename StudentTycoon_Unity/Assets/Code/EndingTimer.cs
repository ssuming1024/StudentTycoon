using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimedCareerEndingJudge : MonoBehaviour
{
    public static TimedCareerEndingJudge Instance;

    [Header("Timer Setting")]
    public float timeLimit = 180f; // 3분
    public string timeText;

    [Header("Special Ending Threshold")]
    public int specialEndingScore = 25;

    [Header("Average Ending Setting")]
    public float passAverage = 10f;

    [Header("Normal Ending Scenes")]
    public string goodEndingSceneName = "goodEnding";
    public string badEndingSceneName = "BadEnding";

    [Header("Special Career Ending Scenes")]
    public string programmingEndingSceneName = "ProgramingEnding";
    public string koreanEndingSceneName = "KoreanEnding";
    public string mathEndingSceneName = "MathEnding";
    public string societyEndingSceneName = "SocietyEnding";
    public string artEndingSceneName = "ArtEnding";
    public string scienceEndingSceneName = "ScienceEnding";
    public string healthEndingSceneName = "HealthEnding";
    public string musicEndingSceneName = "MusicEnding";

    private float currentTime;
    private bool ended = false;

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

    private void Awake()
    {
        // 이미 타이머가 있으면 새로 생긴 타이머는 삭제
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬이 바뀌어도 타이머 오브젝트 유지
        DontDestroyOnLoad(gameObject);

        currentTime = timeLimit;
    }

    private void Start()
    {
        UpdateTimerText();
        Debug.Log("[TimedCareerEndingJudge] 타이머 시작");
    }

    private void Update()
    {
        if (ended) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerText();
            CheckEnding();
            return;
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timeText = $"{minutes:00}:{seconds:00}";
    }

    public void CheckEnding()
    {
        if (ended) return;
        ended = true;

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

        Debug.Log($"가장 높은 과목: {bestSubject}");
        Debug.Log($"가장 높은 점수: {bestScore}");
        Debug.Log($"평균 점수: {average}");

        // 1순위: 특수 진로 엔딩
        if (bestScore >= specialEndingScore)
        {
            string specialSceneName = GetSpecialEndingScene(bestSubject);

            if (!string.IsNullOrEmpty(specialSceneName))
            {
                Debug.Log($"특수 진로 엔딩 이동: {specialSceneName}");
                SceneManager.LoadScene(specialSceneName);
                return;
            }
        }

        // 2순위: 평균 엔딩
        if (average >= passAverage)
        {
            Debug.Log($"평균 {average}점 → goodEnding 이동");
            SceneManager.LoadScene(goodEndingSceneName);
        }
        else
        {
            Debug.Log($"평균 {average}점 → BadEnding 이동");
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
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubjectSceneButtonBinder : MonoBehaviour
{
    [SerializeField] private string koreanSceneName = "KoreanTypingTest";
    [SerializeField] private string societySceneName = "SocietyTest";
    [SerializeField] private string healthSceneName = "HealthSpaceTest";
    [SerializeField] private string programmingSceneName = "ProgrammingTest";
    [SerializeField] private string artSceneName = "ArtTest";
    [SerializeField] private string musicSceneName = "MusicTest";
    [SerializeField] private string mathSceneName = "MathTest";
    [SerializeField] private string scienceSceneName = "ScienceTest";

    public TMP_Text timerText;
    TimedCareerEndingJudge timer;

    private void Awake()
    {
        BindButtons();


    }

    private void Start()
    {
        timer = FindFirstObjectByType<TimedCareerEndingJudge>();
    }

    private void Update()
    {
        timerText.text = timer.timeText;
    }

    private void BindButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            string subjectName = GetButtonText(button);
            string sceneName = GetSceneName(subjectName);

            if (string.IsNullOrWhiteSpace(sceneName))
            {
                continue;
            }

            string capturedSceneName = sceneName;
            button.onClick.AddListener(() => SceneManager.LoadScene(capturedSceneName));
        }
    }

    private string GetButtonText(Button button)
    {
        TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>(true);
        if (texts == null || texts.Length == 0)
        {
            return string.Empty;
        }

        return texts[0].text.Trim();
    }

    private string GetSceneName(string subjectName)
    {
        switch (subjectName)
        {
            case "국어":
                return koreanSceneName;
            case "사회":
                return societySceneName;
            case "체육":
                return healthSceneName;
            case "프로그래밍":
                return programmingSceneName;
            case "미술":
                return artSceneName;
            case "음악":
                return musicSceneName;
            case "수학":
                return mathSceneName;
            case "과학":
                return scienceSceneName;
            default:
                return string.Empty;
        }
    }
}

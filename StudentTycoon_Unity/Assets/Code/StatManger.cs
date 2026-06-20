using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    [Header("바 표시 기준값")]
    [SerializeField] private int barMaxValue = 20;

    [Header("능력치 값")]
    [SerializeField] private int programming = 0;
    [SerializeField] private int korean = 0;
    [SerializeField] private int math = 0;
    [SerializeField] private int society = 0;
    [SerializeField] private int art = 0;
    [SerializeField] private int health = 0;
    [SerializeField] private int science = 0;
    [SerializeField] private int music = 0;

    [Header("능력치 바 UI")]
    [SerializeField] private Slider programmingBar;
    [SerializeField] private Slider koreanBar;
    [SerializeField] private Slider mathBar;
    [SerializeField] private Slider societyBar;
    [SerializeField] private Slider artBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider scienceBar;
    [SerializeField] private Slider musicBar;

    [Header("능력치 숫자 UI")]
    [SerializeField] private TMP_Text programmingText;
    [SerializeField] private TMP_Text koreanText;
    [SerializeField] private TMP_Text mathText;
    [SerializeField] private TMP_Text societyText;
    [SerializeField] private TMP_Text artText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scienceText;
    [SerializeField] private TMP_Text musicText;

    [Header("증가 결과 표시 Text")]
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        SetSliderMaxValue();
        LoadSavedStats();

        if (PlayerPrefs.GetInt("LastQuizCorrect", 0) == 1)
        {
            StartCoroutine(ShowIncreaseAnimation());
        }
        else
        {
            UpdateUI();
        }
    }

    private void SetSliderMaxValue()
    {
        SetSlider(programmingBar);
        SetSlider(koreanBar);
        SetSlider(mathBar);
        SetSlider(societyBar);
        SetSlider(artBar);
        SetSlider(healthBar);
        SetSlider(scienceBar);
        SetSlider(musicBar);
    }

    private void SetSlider(Slider slider)
    {
        if (slider == null)
        {
            return;
        }

        slider.minValue = 0;
        slider.maxValue = barMaxValue;
        slider.wholeNumbers = false;
    }

    private void LoadSavedStats()
    {
        programming = PlayerPrefs.GetInt("Programming", 0);
        korean = PlayerPrefs.GetInt("Korean", 0);
        math = PlayerPrefs.GetInt("Math", 0);
        society = PlayerPrefs.GetInt("Society", 0);
        art = PlayerPrefs.GetInt("Art", 0);
        health = PlayerPrefs.GetInt("Health", 0);
        science = PlayerPrefs.GetInt("Science", 0);
        music = PlayerPrefs.GetInt("Music", 0);
    }

    private IEnumerator ShowIncreaseAnimation()
    {
        string subject = PlayerPrefs.GetString("LastSubject", "");
        int beforeStat = PlayerPrefs.GetInt("LastBeforeStat", 0);
        int addValue = PlayerPrefs.GetInt("LastAddValue", 0);
        int afterStat = PlayerPrefs.GetInt("LastAfterStat", 0);

        SetStatValue(subject, beforeStat);
        UpdateUI();

        if (resultText != null)
        {
            resultText.text = GetKoreanSubjectName(subject) + " +" + addValue + "  (" + beforeStat + " → " + afterStat + ")";
        }

        yield return new WaitForSeconds(0.3f);

        float timer = 0f;
        float duration = 0.8f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            int currentValue = Mathf.RoundToInt(Mathf.Lerp(beforeStat, afterStat, t));

            SetStatValue(subject, currentValue);
            UpdateUI();

            yield return null;
        }

        SetStatValue(subject, afterStat);
        UpdateUI();

        PlayerPrefs.DeleteKey("LastQuizCorrect");
        PlayerPrefs.DeleteKey("LastSubject");
        PlayerPrefs.DeleteKey("LastBeforeStat");
        PlayerPrefs.DeleteKey("LastAddValue");
        PlayerPrefs.DeleteKey("LastAfterStat");
        PlayerPrefs.Save();
    }

    private void SetStatValue(string subject, int value)
    {
        switch (subject)
        {
            case "Programming":
                programming = value;
                break;
            case "Korean":
                korean = value;
                break;
            case "Math":
                math = value;
                break;
            case "Society":
                society = value;
                break;
            case "Art":
                art = value;
                break;
            case "Health":
                health = value;
                break;
            case "Science":
                science = value;
                break;
            case "Music":
                music = value;
                break;
        }
    }

    private string GetKoreanSubjectName(string subject)
    {
        switch (subject)
        {
            case "Programming":
                return "프로그래밍";
            case "Korean":
                return "국어";
            case "Math":
                return "수학";
            case "Society":
                return "사회";
            case "Art":
                return "미술";
            case "Health":
                return "체육";
            case "Science":
                return "과학";
            case "Music":
                return "음악";
            default:
                return "능력치";
        }
    }

    private void UpdateUI()
    {
        UpdateStat(programmingBar, programmingText, programming);
        UpdateStat(koreanBar, koreanText, korean);
        UpdateStat(mathBar, mathText, math);
        UpdateStat(societyBar, societyText, society);
        UpdateStat(artBar, artText, art);
        UpdateStat(healthBar, healthText, health);
        UpdateStat(scienceBar, scienceText, science);
        UpdateStat(musicBar, musicText, music);
    }

    private void UpdateStat(Slider slider, TMP_Text text, int value)
    {
        if (slider != null)
        {
            slider.maxValue = Mathf.Max(barMaxValue, value);
            slider.value = value;
        }

        if (text != null)
        {
            text.text = value.ToString();
        }
    }
}
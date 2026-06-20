using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    [Header("남은 스탯")]
    [SerializeField] private int remainStat = 35;

    [Header("각 능력치 최대치")]
    [SerializeField] private int maxEachStat = 10;

    [Header("능력치 값")]
    [SerializeField] private int programming = 0;
    [SerializeField] private int korean = 0;
    [SerializeField] private int math = 0;
    [SerializeField] private int society = 0;
    [SerializeField] private int art = 0;
    [SerializeField] private int health = 0;
    [SerializeField] private int science = 0;
    [SerializeField] private int music = 0;

    [Header("남은 스탯 UI")]
    [SerializeField] private Slider remainStatBar;
    [SerializeField] private TMP_Text remainStatText;

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

    private void Start()
    {
        SetSliderMaxValue();
        UpdateUI();
    }

    private void SetSliderMaxValue()
    {
        remainStatBar.maxValue = 35;
        remainStatBar.minValue = 0;

        programmingBar.maxValue = maxEachStat;
        koreanBar.maxValue = maxEachStat;
        mathBar.maxValue = maxEachStat;
        societyBar.maxValue = maxEachStat;
        artBar.maxValue = maxEachStat;
        healthBar.maxValue = maxEachStat;
        scienceBar.maxValue = maxEachStat;
        musicBar.maxValue = maxEachStat;
    }

    public void AddProgrammingStat()
    {
        if (remainStat > 0 && programming < maxEachStat)
        {
            programming++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddKoreanStat()
    {
        if (remainStat > 0 && korean < maxEachStat)
        {
            korean++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddMathStat()
    {
        if (remainStat > 0 && math < maxEachStat)
        {
            math++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddSocietyStat()
    {
        if (remainStat > 0 && society < maxEachStat)
        {
            society++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddArtStat()
    {
        if (remainStat > 0 && art < maxEachStat)
        {
            art++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddHealthStat()
    {
        if (remainStat > 0 && health < maxEachStat)
        {
            health++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddScienceStat()
    {
        if (remainStat > 0 && science < maxEachStat)
        {
            science++;
            remainStat--;
            UpdateUI();
        }
    }

    public void AddMusicStat()
    {
        if (remainStat > 0 && music < maxEachStat)
        {
            music++;
            remainStat--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        remainStatBar.value = remainStat;
        remainStatText.text = "최대 스탯 : " + remainStat;

        programmingBar.value = programming;
        koreanBar.value = korean;
        mathBar.value = math;
        societyBar.value = society;
        artBar.value = art;
        healthBar.value = health;
        scienceBar.value = science;
        musicBar.value = music;

        programmingText.text = programming.ToString();
        koreanText.text = korean.ToString();
        mathText.text = math.ToString();
        societyText.text = society.ToString();
        artText.text = art.ToString();
        healthText.text = health.ToString();
        scienceText.text = science.ToString();
        musicText.text = music.ToString();
    }
}
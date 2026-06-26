using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public class KoreanTypingGameManager : MonoBehaviour
{
    [SerializeField] private string statKey = "Korean";
    [SerializeField] private string statSceneName = "StatUp";
    [SerializeField] private string wrongSceneName = "WrongAnswer";
    [Header("난이도 설정")]
    [InspectorName("성공에 필요한 문장 수")]
    [SerializeField] private int requiredClearCount = 2;
    [InspectorName("제한 시간")]
    [SerializeField] private float timeLimit = 45f;
    [SerializeField] private int maxStatValue = 20;
    [InspectorName("입력창 자동 선택")]
    [SerializeField] private bool keepInputFocused = true;

    [Header("글꼴")]
    [SerializeField] private TMP_FontAsset messageFont;

    [Header("Inspector에서 직접 바꿀 타자 문장")]
    [Tooltip("여기에 짧은 문장을 추가하면 그 문장 중 하나가 랜덤으로 나옵니다.")]
    [InspectorName("타자 문장 목록")]
    [SerializeField]
    private string[] typingSentences =
    {
        "나는 공부한다",
        "책을 읽는다",
        "오늘은 맑다",
        "학교에 간다",
        "친구와 논다"
    };

    private TMP_Text timerText;
    private TMP_Text progressText;
    private TMP_Text targetText;
    private TMP_Text hintText;
    private TMP_Text resultText;
    private TMP_InputField inputField;
    private int clearCount;
    private int currentSentenceIndex;
    private float remainingTime;
    private bool isFinished;

    private void Start()
    {
        EnsureEventSystem();
        EnsureTypingSentences();

        requiredClearCount = Mathf.Max(1, requiredClearCount);
        remainingTime = Mathf.Max(10f, timeLimit);
        currentSentenceIndex = Random.Range(0, typingSentences.Length);
        CreateUi();
        ShowCurrentSentence();
        UpdateUi();
        StartCoroutine(FocusInputNextFrame());
    }

    private void Update()
    {
        if (isFinished)
        {
            return;
        }

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            FinishGame(false);
            return;
        }

        UpdateUi();
        KeepTypingInputFocused();
    }

    private void CreateUi()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        Image background = CreateImage(canvasObject.transform, "Background", new Color(0.78f, 0.70f, 0.68f, 1f), Vector2.zero, Vector2.one, Vector2.zero);
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        CreateText(canvasObject.transform, "Title", "국어 타자 연습", 58, new Vector2(0f, 190f));
        timerText = CreateText(canvasObject.transform, "TimerText", string.Empty, 32, new Vector2(0f, 125f));
        progressText = CreateText(canvasObject.transform, "ProgressText", string.Empty, 30, new Vector2(0f, 80f));
        targetText = CreateText(canvasObject.transform, "TargetText", string.Empty, 38, new Vector2(0f, 15f));
        hintText = CreateText(canvasObject.transform, "HintText", "마우스를 누르지 않고 바로 입력해도 됩니다", 22, new Vector2(0f, -35f));
        inputField = CreateInputField(canvasObject.transform, new Vector2(0f, -95f));
        resultText = CreateText(canvasObject.transform, "ResultText", string.Empty, 30, new Vector2(0f, -175f));
    }

    private Image CreateImage(Transform parent, string objectName, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition)
    {
        GameObject imageObject = new GameObject(objectName);
        imageObject.transform.SetParent(parent, false);

        Image image = imageObject.AddComponent<Image>();
        image.color = color;

        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = anchoredPosition;

        return image;
    }

    private TMP_Text CreateText(Transform parent, string objectName, string text, float fontSize, Vector2 anchoredPosition)
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        TMP_Text tmpText = textObject.AddComponent<TextMeshProUGUI>();
        if (messageFont != null)
        {
            tmpText.font = messageFont;
        }

        tmpText.text = text;
        tmpText.fontSize = fontSize;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = new Color(0.18f, 0.16f, 0.16f, 1f);

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(780f, 80f);

        return tmpText;
    }

    private TMP_InputField CreateInputField(Transform parent, Vector2 anchoredPosition)
    {
        GameObject inputObject = new GameObject("TypingInput");
        inputObject.transform.SetParent(parent, false);

        Image image = inputObject.AddComponent<Image>();
        image.color = Color.white;

        TMP_InputField input = inputObject.AddComponent<TMP_InputField>();
        input.lineType = TMP_InputField.LineType.SingleLine;

        RectTransform inputRect = inputObject.GetComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0.5f, 0.5f);
        inputRect.anchorMax = new Vector2(0.5f, 0.5f);
        inputRect.anchoredPosition = anchoredPosition;
        inputRect.sizeDelta = new Vector2(520f, 60f);

        TMP_Text textComponent = CreateInputText(inputObject.transform, "Text", string.Empty, new Color(0.16f, 0.14f, 0.14f, 1f));
        TMP_Text placeholder = CreateInputText(inputObject.transform, "Placeholder", "문장을 입력하세요", new Color(0.4f, 0.36f, 0.36f, 0.55f));

        input.textViewport = inputRect;
        input.textComponent = textComponent;
        input.placeholder = placeholder;
        input.onValueChanged.AddListener(CheckTyping);

        return input;
    }

    private TMP_Text CreateInputText(Transform parent, string objectName, string text, Color color)
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        TMP_Text tmpText = textObject.AddComponent<TextMeshProUGUI>();
        if (messageFont != null)
        {
            tmpText.font = messageFont;
        }

        tmpText.text = text;
        tmpText.fontSize = 28;
        tmpText.alignment = TextAlignmentOptions.MidlineLeft;
        tmpText.color = color;

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = new Vector2(18f, 6f);
        rectTransform.offsetMax = new Vector2(-18f, -6f);

        return tmpText;
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();

#if ENABLE_INPUT_SYSTEM
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
#elif ENABLE_LEGACY_INPUT_MANAGER
        eventSystemObject.AddComponent<StandaloneInputModule>();
#endif
    }

    private void EnsureTypingSentences()
    {
        if (typingSentences != null && typingSentences.Length > 0)
        {
            return;
        }

        typingSentences = new[]
        {
            "나는 공부한다",
            "책을 읽는다",
            "오늘은 맑다"
        };
    }

    private IEnumerator FocusInputNextFrame()
    {
        yield return null;
        ActivateTypingInput();
    }

    private void KeepTypingInputFocused()
    {
        if (!keepInputFocused || inputField == null || isFinished || !inputField.interactable)
        {
            return;
        }

        if (inputField.isFocused)
        {
            return;
        }

        ActivateTypingInput();
    }

    private void ActivateTypingInput()
    {
        if (inputField == null || isFinished || !inputField.interactable)
        {
            return;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        }

        inputField.Select();
        inputField.ActivateInputField();
    }

    private void CheckTyping(string inputValue)
    {
        if (isFinished || typingSentences == null || typingSentences.Length == 0)
        {
            return;
        }

        if (Normalize(inputValue) != Normalize(typingSentences[currentSentenceIndex]))
        {
            return;
        }

        clearCount++;
        if (clearCount >= requiredClearCount)
        {
            FinishGame(true);
            return;
        }

        currentSentenceIndex = typingSentences.Length == 1
            ? 0
            : (currentSentenceIndex + Random.Range(1, typingSentences.Length)) % typingSentences.Length;
        inputField.text = string.Empty;
        ActivateTypingInput();
        ShowCurrentSentence();
        UpdateUi();
    }

    private string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    private void ShowCurrentSentence()
    {
        if (targetText != null && typingSentences != null && typingSentences.Length > 0)
        {
            targetText.text = typingSentences[currentSentenceIndex];
        }
    }

    private void UpdateUi()
    {
        if (timerText != null)
        {
            timerText.text = "남은 시간 " + Mathf.CeilToInt(remainingTime) + "초";
        }

        if (progressText != null)
        {
            progressText.text = clearCount + " / " + requiredClearCount;
        }
    }

    private void FinishGame(bool success)
    {
        isFinished = true;
        UpdateUi();

        if (inputField != null)
        {
            inputField.interactable = false;
        }

        if (success)
        {
            AddStat();
            if (resultText != null)
            {
                resultText.text = "성공!";
            }

            StartCoroutine(MoveSceneAfterDelay(statSceneName));
        }
        else
        {
            PlayerPrefs.SetInt("LastQuizCorrect", 0);
            PlayerPrefs.Save();

            if (resultText != null)
            {
                resultText.text = "시간 초과";
            }

            StartCoroutine(MoveSceneAfterDelay(wrongSceneName));
        }
    }

    private void AddStat()
    {
        int statLimit = maxStatValue > 0 ? maxStatValue : 20;
        int beforeStat = Mathf.Clamp(PlayerPrefs.GetInt(statKey, 0), 0, statLimit);
        int randomAddValue = Random.Range(1, 6);
        int afterStat = Mathf.Clamp(beforeStat + randomAddValue, 0, statLimit);
        int addValue = afterStat - beforeStat;

        PlayerPrefs.SetInt(statKey, afterStat);
        PlayerPrefs.SetInt("LastQuizCorrect", 1);
        PlayerPrefs.SetString("LastSubject", statKey);
        PlayerPrefs.SetInt("LastBeforeStat", beforeStat);
        PlayerPrefs.SetInt("LastAddValue", addValue);
        PlayerPrefs.SetInt("LastAfterStat", afterStat);
        PlayerPrefs.Save();
    }

    private IEnumerator MoveSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}

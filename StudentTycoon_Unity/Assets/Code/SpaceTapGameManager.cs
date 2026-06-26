using System.Collections;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceTapGameManager : MonoBehaviour
{
    [SerializeField] private string statKey = "Health";
    [SerializeField] private string statSceneName = "StatUp";
    [SerializeField] private string wrongSceneName = "WrongAnswer";
    [SerializeField] private int targetPressCount = 30;
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private int maxStatValue = 20;
    [SerializeField] private TMP_FontAsset messageFont;

    private TMP_Text timerText;
    private TMP_Text countText;
    private TMP_Text resultText;
    private int pressCount;
    private float remainingTime;
    private bool isFinished;

    private void Start()
    {
        remainingTime = timeLimit;
        CreateUi();
        UpdateUi();
    }

    private void Update()
    {
        if (isFinished)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (WasSpacePressed())
        {
            pressCount++;
        }

        if (pressCount >= targetPressCount)
        {
            FinishGame(true);
            return;
        }

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            FinishGame(false);
            return;
        }

        UpdateUi();
    }

    private bool WasSpacePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.Space);
#else
        return false;
#endif
    }

    private void CreateUi()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        Image background = CreateImage(canvasObject.transform, "Background", new Color(0.72f, 0.84f, 0.88f, 1f), Vector2.zero, Vector2.one, Vector2.zero);
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        CreateText(canvasObject.transform, "Title", "체육", 64, new Vector2(0f, 180f));
        CreateText(canvasObject.transform, "Guide", "시간 안에 스페이스바를 빠르게 누르세요", 32, new Vector2(0f, 110f));
        timerText = CreateText(canvasObject.transform, "TimerText", string.Empty, 36, new Vector2(0f, 35f));
        countText = CreateText(canvasObject.transform, "CountText", string.Empty, 48, new Vector2(0f, -35f));
        resultText = CreateText(canvasObject.transform, "ResultText", string.Empty, 32, new Vector2(0f, -115f));
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
        rectTransform.sizeDelta = new Vector2(760f, 90f);

        return tmpText;
    }

    private void UpdateUi()
    {
        if (timerText != null)
        {
            timerText.text = "남은 시간 " + Mathf.CeilToInt(remainingTime) + "초";
        }

        if (countText != null)
        {
            countText.text = pressCount + " / " + targetPressCount;
        }
    }

    private void FinishGame(bool success)
    {
        isFinished = true;
        UpdateUi();

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
                resultText.text = "실패";
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

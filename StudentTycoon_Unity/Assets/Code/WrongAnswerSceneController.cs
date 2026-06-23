using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WrongAnswerSceneController : MonoBehaviour
{
    [SerializeField] private string returnSceneName = "StatUp_UI";
    [SerializeField] private float autoReturnDelay = 1.5f;
    [SerializeField] private TMP_FontAsset messageFont;

    private void Start()
    {
        CreateWrongAnswerUi();
        StartCoroutine(ReturnAfterDelay());
    }

    private void CreateWrongAnswerUi()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject backgroundObject = new GameObject("Background");
        backgroundObject.transform.SetParent(canvasObject.transform, false);
        Image background = backgroundObject.AddComponent<Image>();
        background.color = new Color(0.78f, 0.70f, 0.68f, 1f);
        RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        TMP_Text messageText = CreateText(canvasObject.transform, "WrongAnswerText", "틀렸습니다", 72, new Vector2(0f, 55f));
        messageText.color = new Color(0.18f, 0.16f, 0.16f, 1f);
        messageText.fontStyle = FontStyles.Bold;

        TMP_Text guideText = CreateText(canvasObject.transform, "ReturnGuideText", "잠시 후 돌아갑니다", 30, new Vector2(0f, -55f));
        guideText.color = new Color(0.32f, 0.28f, 0.27f, 1f);
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
        tmpText.enableAutoSizing = true;
        tmpText.fontSizeMin = 18f;
        tmpText.fontSizeMax = fontSize;

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(700f, 110f);

        return tmpText;
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(autoReturnDelay);
        SceneManager.LoadScene(returnSceneName);
    }
}

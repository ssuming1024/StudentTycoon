using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class OXQuizManager : MonoBehaviour
{
    public enum Subject
    {
        Programming,
        Korean,
        Math,
        Society,
        Art,
        Health,
        Science,
        Music
    }

    public List<string> questionList = new List<string>();
    public List<bool> answerList = new List<bool>();


    public List<string> inputQuestionList = new List<string>();
    public List<string> inputAnswerList = new List<string>();
    string answer = string.Empty;

    public TMP_Text questionText;

    public TMP_InputField answerInput;

    [SerializeField] private GameObject oButtonObject;
    [SerializeField] private GameObject xButtonObject;
    [SerializeField] private GameObject submitButtonObject;

    [Header("문제 과목")]
    [SerializeField] private Subject subject = Subject.Math;

    [Header("정답 설정")]
    [SerializeField] private bool correctAnswerIsO = true;

    [Header("결과 텍스트")]
    [SerializeField] private TMP_Text resultText;

    [Header("정답이면 이동할 스탯 화면 씬 이름")]
    [SerializeField] private string statSceneName = "DecoratePlayer";

    [Header("오답이면 이동할 화면 씬 이름")]
    [SerializeField] private string mainSceneName = "WrongAnswer";

    [Header("능력치 최대값")]
    [SerializeField] private int maxStatValue = 20;

    private bool isAnswered = false;
    private bool isInputQuestion = false;

    private void Awake()
    {
        if (answerInput != null)
        {
            answerInput.onSubmit.AddListener(_ => SubmitAnswer());
        }
    }

    public void ChooseO()
    {
        if (isInputQuestion)
        {
            return;
        }

        CheckAnswer(true);
    }

    public void ChooseX()
    {
        if (isInputQuestion)
        {
            return;
        }

        CheckAnswer(false);
    }

    public void SubmitAnswer()
    {
        if (isAnswered || !isInputQuestion || answerInput == null)
        {
            return;
        }

        bool isCorrect = NormalizeAnswer(answerInput.text) == NormalizeAnswer(answer);
        HandleAnswer(isCorrect);
    }

    public void MakeQuestion()
    {
        isAnswered = false;
        answer = string.Empty;

        if (resultText != null)
        {
            resultText.text = string.Empty;
        }

        if (answerInput != null)
        {
            answerInput.text = string.Empty;
        }

        bool canMakeOXQuestion = HasOXQuestion();
        bool canMakeInputQuestion = HasInputQuestion();

        if (!canMakeOXQuestion && !canMakeInputQuestion)
        {
            if (questionText != null)
            {
                questionText.text = "문제 데이터가 없습니다.";
            }

            SetInputMode(false);
            return;
        }

        bool useInputQuestion = canMakeInputQuestion && (!canMakeOXQuestion || Random.Range(0, 2) == 1);
        if (useInputQuestion)
        {
            MakeInputQuestion();
        }
        else
        {
            MakeOXQuestion();
        }
    }

    private void Start()
    {
        MakeQuestion();
    }

    private void CheckAnswer(bool playerAnswerIsO)
    {
        HandleAnswer(playerAnswerIsO == correctAnswerIsO);
    }

    private void MakeOXQuestion()
    {
        int qIndex = GetRandomOXQuestionIndex();

        if (questionText != null)
        {
            questionText.text = questionList[qIndex];
        }

        correctAnswerIsO = answerList[qIndex];
        SetInputMode(false);
    }

    private void MakeInputQuestion()
    {
        int qInputIndex = GetRandomInputQuestionIndex();

        if (questionText != null)
        {
            questionText.text = inputQuestionList[qInputIndex];
        }

        answer = inputAnswerList[qInputIndex];
        SetInputMode(true);

        if (answerInput != null)
        {
            answerInput.Select();
            answerInput.ActivateInputField();
        }
    }

    private int GetRandomOXQuestionIndex()
    {
        List<int> validIndexes = new List<int>();
        int count = Mathf.Min(questionList.Count, answerList.Count);

        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(questionList[i]))
            {
                validIndexes.Add(i);
            }
        }

        return validIndexes[Random.Range(0, validIndexes.Count)];
    }

    private int GetRandomInputQuestionIndex()
    {
        List<int> validIndexes = new List<int>();
        int count = Mathf.Min(inputQuestionList.Count, inputAnswerList.Count);

        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(inputQuestionList[i]) && !string.IsNullOrWhiteSpace(inputAnswerList[i]))
            {
                validIndexes.Add(i);
            }
        }

        return validIndexes[Random.Range(0, validIndexes.Count)];
    }

    private bool HasOXQuestion()
    {
        int count = Mathf.Min(questionList.Count, answerList.Count);

        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(questionList[i]))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasInputQuestion()
    {
        int count = Mathf.Min(inputQuestionList.Count, inputAnswerList.Count);

        for (int i = 0; i < count; i++)
        {
            if (!string.IsNullOrWhiteSpace(inputQuestionList[i]) && !string.IsNullOrWhiteSpace(inputAnswerList[i]))
            {
                return true;
            }
        }

        return false;
    }

    private void SetInputMode(bool showInput)
    {
        isInputQuestion = showInput;

        if (answerInput != null)
        {
            answerInput.gameObject.SetActive(showInput);
        }

        if (submitButtonObject != null)
        {
            submitButtonObject.SetActive(showInput);
        }

        if (oButtonObject != null)
        {
            oButtonObject.SetActive(!showInput);
        }

        if (xButtonObject != null)
        {
            xButtonObject.SetActive(!showInput);
        }
    }

    private string NormalizeAnswer(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().Replace(" ", string.Empty).ToLowerInvariant();
    }

    private void HandleAnswer(bool isCorrect)
    {
        if (isAnswered)
        {
            return;
        }

        isAnswered = true;

        if (isCorrect)
        {
            string statKey = subject.ToString();

            int statLimit = maxStatValue > 0 ? maxStatValue : 20;
            int beforeStat = Mathf.Clamp(PlayerPrefs.GetInt(statKey, 0), 0, statLimit);
            int randomAddValue = Random.Range(1, 6); // 1~5 랜덤
            int afterStat = Mathf.Clamp(beforeStat + randomAddValue, 0, statLimit);
            int addValue = afterStat - beforeStat;

            PlayerPrefs.SetInt(statKey, afterStat);

            // 스탯 화면에서 0 → 2처럼 올라가는 걸 보여주기 위한 기록
            PlayerPrefs.SetInt("LastQuizCorrect", 1);
            PlayerPrefs.SetString("LastSubject", statKey);
            PlayerPrefs.SetInt("LastBeforeStat", beforeStat);
            PlayerPrefs.SetInt("LastAddValue", addValue);
            PlayerPrefs.SetInt("LastAfterStat", afterStat);

            PlayerPrefs.Save();

            if (resultText != null)
            {
                resultText.text = "정답!";
            }

            StartCoroutine(MoveSceneAfterDelay(statSceneName));
        }
        else
        {
            PlayerPrefs.SetInt("LastQuizCorrect", 0);
            PlayerPrefs.Save();

            if (resultText != null)
            {
                resultText.text = "틀렸어요";
            }

            StartCoroutine(MoveSceneAfterDelay(mainSceneName));
        }
    }

    private IEnumerator MoveSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}

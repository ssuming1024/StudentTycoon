using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class OXQuizManager : MonoBehaviour
{
    public enum QuestionType
    {
        OX,
        Input
    }

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

    [System.Serializable]
    public class QuizQuestion
    {
        [InspectorName("문제 종류")]
        [Tooltip("OX 문제면 OX, 직접 글자를 입력하게 만들 문제면 Input을 고르세요.")]
        public QuestionType questionType = QuestionType.OX;

        [TextArea(2, 4)]
        [InspectorName("문제 내용")]
        [Tooltip("게임 화면에 보여줄 문제 문장입니다.")]
        public string questionText;

        [InspectorName("OX 정답이 O인가?")]
        [Tooltip("OX 문제일 때만 사용합니다. O가 정답이면 체크, X가 정답이면 체크를 끄세요.")]
        public bool oxAnswerIsO = true;

        [InspectorName("입력 정답")]
        [Tooltip("Input 문제일 때만 사용합니다. 플레이어가 입력해야 하는 정답입니다.")]
        public string inputAnswer;
    }

    [Header("Inspector에서 직접 만들 문제")]
    [InspectorName("아래 문제 목록 사용")]
    [Tooltip("켜져 있으면 아래 문제 목록을 먼저 사용합니다. 목록이 비어 있으면 기존 방식 문제를 사용합니다.")]
    [SerializeField] private bool useInspectorQuestionList = true;
    [InspectorName("문제 목록")]
    [Tooltip("Size를 늘린 뒤 Element를 펼쳐서 문제를 직접 추가하세요.")]
    public List<QuizQuestion> quizQuestions = new List<QuizQuestion>();

    [Header("기존 방식 문제 목록 - 비워도 됨")]
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
            answerInput.onValueChanged.AddListener(value => WarmUpFont(answerInput.textComponent, value));
            WarmUpFont(answerInput.placeholder as TMP_Text);
            WarmUpFont(answerInput.textComponent);
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

        bool isCorrect = IsCorrectInputAnswer(answerInput.text, answer);
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

        if (useInspectorQuestionList && HasInspectorQuestion())
        {
            MakeInspectorQuestion();
            return;
        }

        bool canMakeOXQuestion = HasOXQuestion();
        bool canMakeInputQuestion = HasInputQuestion();

        if (!canMakeOXQuestion && !canMakeInputQuestion)
        {
            if (questionText != null)
            {
                SetQuestionText("문제 데이터가 없습니다.");
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

    private void MakeInspectorQuestion()
    {
        int questionIndex = GetRandomInspectorQuestionIndex();
        QuizQuestion selectedQuestion = quizQuestions[questionIndex];

        if (questionText != null)
        {
            SetQuestionText(selectedQuestion.questionText);
        }

        if (selectedQuestion.questionType == QuestionType.Input)
        {
            answer = selectedQuestion.inputAnswer;
            SetInputMode(true);

            if (answerInput != null)
            {
                answerInput.Select();
                answerInput.ActivateInputField();
            }
        }
        else
        {
            correctAnswerIsO = selectedQuestion.oxAnswerIsO;
            SetInputMode(false);
        }
    }

    private void MakeOXQuestion()
    {
        int qIndex = GetRandomOXQuestionIndex();

        if (questionText != null)
        {
            SetQuestionText(questionList[qIndex]);
        }

        correctAnswerIsO = answerList[qIndex];
        SetInputMode(false);
    }

    private void MakeInputQuestion()
    {
        int qInputIndex = GetRandomInputQuestionIndex();

        if (questionText != null)
        {
            SetQuestionText(inputQuestionList[qInputIndex]);
        }

        answer = inputAnswerList[qInputIndex];
        SetInputMode(true);

        if (answerInput != null)
        {
            answerInput.Select();
            answerInput.ActivateInputField();
        }
    }

    private int GetRandomInspectorQuestionIndex()
    {
        List<int> validIndexes = new List<int>();

        for (int i = 0; i < quizQuestions.Count; i++)
        {
            if (IsValidInspectorQuestion(quizQuestions[i]))
            {
                validIndexes.Add(i);
            }
        }

        return validIndexes[Random.Range(0, validIndexes.Count)];
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

    private bool HasInspectorQuestion()
    {
        if (quizQuestions == null)
        {
            return false;
        }

        for (int i = 0; i < quizQuestions.Count; i++)
        {
            if (IsValidInspectorQuestion(quizQuestions[i]))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsValidInspectorQuestion(QuizQuestion question)
    {
        if (question == null || string.IsNullOrWhiteSpace(question.questionText))
        {
            return false;
        }

        return question.questionType == QuestionType.OX
            || !string.IsNullOrWhiteSpace(question.inputAnswer);
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

    private void SetQuestionText(string text)
    {
        WarmUpFont(questionText, text);
        questionText.text = text;
    }

    private void WarmUpFont(TMP_Text targetText)
    {
        if (targetText != null)
        {
            WarmUpFont(targetText, targetText.text);
        }
    }

    private void WarmUpFont(TMP_Text targetText, string text)
    {
        if (targetText == null || targetText.font == null || string.IsNullOrEmpty(text))
        {
            return;
        }

        targetText.font.TryAddCharacters(text, out string missingCharacters);
        if (!string.IsNullOrEmpty(missingCharacters))
        {
            Debug.LogWarning("TMP font is missing characters: " + ToUnicodeList(missingCharacters));
        }
    }

    private string ToUnicodeList(string characters)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < characters.Length; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append("U+");
            builder.Append(((int)characters[i]).ToString("X4"));
        }

        return builder.ToString();
    }

    private bool IsCorrectInputAnswer(string playerAnswer, string correctAnswers)
    {
        string normalizedPlayerAnswer = NormalizeAnswer(playerAnswer);
        if (string.IsNullOrEmpty(normalizedPlayerAnswer))
        {
            return false;
        }

        string[] answers = correctAnswers.Split('/', ',', '|', ';', '\n');
        for (int i = 0; i < answers.Length; i++)
        {
            if (normalizedPlayerAnswer == NormalizeAnswer(answers[i]))
            {
                return true;
            }
        }

        return false;
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

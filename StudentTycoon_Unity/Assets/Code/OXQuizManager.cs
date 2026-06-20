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

    [Header("문제 과목")]
    [SerializeField] private Subject subject = Subject.Math;

    [Header("정답 설정")]
    [SerializeField] private bool correctAnswerIsO = true;

    [Header("결과 텍스트")]
    [SerializeField] private TMP_Text resultText;

    [Header("정답이면 이동할 스탯 화면 씬 이름")]
    [SerializeField] private string statSceneName = "DecoratePlayer";

    [Header("오답이면 이동할 메인 화면 씬 이름")]
    [SerializeField] private string mainSceneName = "PlayGame";

    private bool isAnswered = false;

    public void ChooseO()
    {
        CheckAnswer(true);
    }

    public void ChooseX()
    {
        CheckAnswer(false);
    }

    public void SubmitAnswer()
    {
        if(answer == answerInput.text)
        {
            //정답처리
        }
        else
        {
            // 오답처리
        }
    }

    public void MakeQuestion()
    {
        int typeIndex = Random.Range(0, 2);
        if (typeIndex == 0)
        {
            // OX 퀴즈

            int qIndex = Random.Range(0, questionList.Count);
            questionText.text = questionList[qIndex];
            correctAnswerIsO = answerList[qIndex];
        }
        else
        {
            // 주관식 퀴즈
            int qInputIndex = Random.Range(0, inputQuestionList.Count);
            questionText.text = inputQuestionList[qInputIndex];
            answer = inputAnswerList[qInputIndex];
        }
    }

    private void Start()
    {
        MakeQuestion();
    }

    private void CheckAnswer(bool playerAnswerIsO)
    {
        if (isAnswered)
        {
            return;
        }

        isAnswered = true;

        if (playerAnswerIsO == correctAnswerIsO)
        {
            string statKey = subject.ToString();

            int beforeStat = PlayerPrefs.GetInt(statKey, 0);
            int addValue = Random.Range(1, 6); // 1~5 랜덤
            int afterStat = beforeStat + addValue;

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
                resultText.text = "틀렸다!";
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
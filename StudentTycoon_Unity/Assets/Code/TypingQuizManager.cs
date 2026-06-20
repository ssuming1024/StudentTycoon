using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypingQuizManager : MonoBehaviour
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
    [Header("문제 과목")]
    [SerializeField] private Subject subject = Subject.Math;

    public TMP_Text questionField;
    public TMP_InputField answerField;

    [Header("정답이면 이동할 스탯 화면 씬 이름")]
    [SerializeField] private string statSceneName = "DecoratePlayer";

    [Header("오답이면 이동할 메인 화면 씬 이름")]
    [SerializeField] private string mainSceneName = "PlayGame";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("CheckAnswer", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckAnswer()
    {
        if(answerField.text == questionField.text)
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

            StartCoroutine(MoveSceneAfterDelay(statSceneName));
        }
        else
        {
            
            PlayerPrefs.SetInt("LastQuizCorrect", 0);
            PlayerPrefs.Save();

            StartCoroutine(MoveSceneAfterDelay(mainSceneName));
        }
    }
    
    private IEnumerator MoveSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}

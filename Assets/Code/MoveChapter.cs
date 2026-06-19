using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // 이 줄이 반드시 있어야 합니다!

public class parkchan : MonoBehaviour
{
    private bool isPlayerNear = false;

    void Update()
    {
        // Keyboard.current를 사용하면 입력 시스템 설정 오류와 상관없이 키보드 값을 직접 읽습니다.
        if (isPlayerNear && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("씬 이동 시도!");
            SceneManager.LoadScene("URP2DSceneTemplate");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 태그 검사 없이 무엇이든 닿으면 로그를 찍습니다.
        Debug.Log("충돌 발생! 부딪힌 물체 이름: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("플레이어 태그 확인 완료!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
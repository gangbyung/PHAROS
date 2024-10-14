using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // 싱글톤 패턴
    public static SceneTransition instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            instance = this; // 인스턴스를 현재 객체로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 삭제되지 않도록 설정
        }
    }

    // 대화 종료 후 씬 변경
    public void ChangeSceneAfterDialogue()
    {
        // NPC의 bool 값을 확인하여 씬 전환 결정
        if (DialogueManager.instance.isNpc0)
        {
            // isNpc0이 true일 경우 씬 1로 이동
            SceneManager.LoadScene("Scene1");
        }
        else if (DialogueManager.instance.isNpc1)
        {
            // isNpc1이 true일 경우 씬 2로 이동
            SceneManager.LoadScene("Scene2");
        }
        else
        {
            // 그 외의 경우 기본 씬으로 이동
            SceneManager.LoadScene("DefaultScene");
        }
    }
}

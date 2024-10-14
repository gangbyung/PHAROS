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
    public void EndScene()
    {
        SceneManager.LoadScene("99.GameOver");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("0.GameTitle");
    }
    public void Scene10()
    {
        SceneManager.LoadScene("10.part1");
    }
    // 대화 종료 후 씬 변경
    public void ChangeSceneAfterDialogue()
    {
        // NPC의 bool 값을 확인하여 씬 전환 결정
        if (DialogueManager.instance.isNpc0)
        {
            // isNpc0이 true일 경우 씬 1로 이동
            SceneManager.LoadScene("40.BadEndPart1");
        }
        else if (DialogueManager.instance.isNpc1)
        {
            // isNpc1이 true일 경우 씬 2로 이동
            SceneManager.LoadScene("42.NomalEndPart");
        }
        else
        {
            // 그 외의 경우 기본 씬으로 이동
            SceneManager.LoadScene("43.HappyEndPart");
        }
    }
}

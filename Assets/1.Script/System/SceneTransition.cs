using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // �̱��� ����
    public static SceneTransition instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            instance = this; // �ν��Ͻ��� ���� ��ü�� ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �������� �ʵ��� ����
        }
    }

    // ��ȭ ���� �� �� ����
    public void ChangeSceneAfterDialogue()
    {
        // NPC�� bool ���� Ȯ���Ͽ� �� ��ȯ ����
        if (DialogueManager.instance.isNpc0)
        {
            // isNpc0�� true�� ��� �� 1�� �̵�
            SceneManager.LoadScene("40.BadEndPart1");
        }
        else if (DialogueManager.instance.isNpc1)
        {
            // isNpc1�� true�� ��� �� 2�� �̵�
            SceneManager.LoadScene("42.NomalEndPart");
        }
        else
        {
            // �� ���� ��� �⺻ ������ �̵�
            SceneManager.LoadScene("43.HappyEndPart");
        }
    }
}

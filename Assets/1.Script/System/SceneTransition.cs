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
    // ��ȭ ���� �� �� ����
    
}

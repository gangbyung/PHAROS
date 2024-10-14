using UnityEngine;

public class SingletonDestroyer : MonoBehaviour
{
    void Start()
    {
        // Ư�� ���� ���ڸ��� �̱��� ������Ʈ�� ����
        DestroySingleton("GameManager");
        DestroySingleton("SceneTransition");
        DestroySingleton("DialogueManager");
        DestroySingleton("SoundManager");
        // �ʿ��� �߰� �̱��� ������Ʈ�� ������ ���⿡ �߰�
    }

    private void DestroySingleton(string objectName)
    {
        GameObject singleton = GameObject.Find(objectName);
        if (singleton != null)
        {
            Destroy(singleton);
        }
    }
}

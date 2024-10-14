using UnityEngine;

public class SingletonDestroyer : MonoBehaviour
{
    void Start()
    {
        // 특정 씬에 들어가자마자 싱글톤 오브젝트를 삭제
        DestroySingleton("GameManager");
        DestroySingleton("SceneTransition");
        DestroySingleton("DialogueManager");
        DestroySingleton("SoundManager");
        // 필요한 추가 싱글톤 오브젝트가 있으면 여기에 추가
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

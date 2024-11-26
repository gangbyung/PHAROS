using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;  // 대화 관리 스크립트의 참조
    public int dialogueId;  // 현재 NPC의 대화 ID
    public GameObject parentObject;
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때만 대화 창 실행
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueId);
        }
    }
    public void DeleteMyParent()
    {
        if (transform.parent != null)
        {
            parentObject = transform.parent.gameObject; // 상위 오브젝트 참조
            Destroy(parentObject); // 상위 오브젝트 삭제
            Debug.Log($"상위 오브젝트 '{parentObject.name}'가 삭제되었습니다.");
        }
        else
        {
            Debug.LogWarning("상위 오브젝트가 없습니다.");
        }
    }
}

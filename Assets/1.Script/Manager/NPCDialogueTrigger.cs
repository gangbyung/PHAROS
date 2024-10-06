using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;  // 대화 관리 스크립트의 참조
    public int dialogueId;  // 현재 NPC의 대화 ID

    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때만 대화 창 실행
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueId);
        }
    }
}

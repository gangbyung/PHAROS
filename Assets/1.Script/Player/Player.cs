using UnityEngine;

public class Player : MonoBehaviour
{
    public DialogueManager dialogueManager; // DialogueManager 참조

    private void OnTriggerEnter(Collider other)
    {
        // NPC 태그를 가진 오브젝트와 충돌했는지 확인
        if (other.CompareTag("NPC"))
        {
            // NPC 오브젝트의 이름을 가져와 대사 출력
            string npcName = other.gameObject.name;
            dialogueManager.StartDialogue(npcName);
        }
    }
}

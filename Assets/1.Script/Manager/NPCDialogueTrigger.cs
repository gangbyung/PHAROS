using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;  // ��ȭ ���� ��ũ��Ʈ�� ����
    public int dialogueId;  // ���� NPC�� ��ȭ ID

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ���� ��ȭ â ����
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueId);
        }
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    public DialogueManager dialogueManager; // DialogueManager ����

    private void OnTriggerEnter(Collider other)
    {
        // NPC �±׸� ���� ������Ʈ�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("NPC"))
        {
            // NPC ������Ʈ�� �̸��� ������ ��� ���
            string npcName = other.gameObject.name;
            dialogueManager.StartDialogue(npcName);
        }
    }
}

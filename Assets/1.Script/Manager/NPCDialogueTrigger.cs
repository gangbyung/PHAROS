using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;  // ��ȭ ���� ��ũ��Ʈ�� ����
    public int dialogueId;  // ���� NPC�� ��ȭ ID
    public GameObject parentObject;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ���� ��ȭ â ����
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueId);
            parentObject = transform.parent.gameObject;
            Debug.Log($"{parentObject}�� ������");
            Destroy(parentObject);
        }
    }
}

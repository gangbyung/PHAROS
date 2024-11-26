using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;  // ��ȭ ���� ��ũ��Ʈ�� ����
    public int dialogueId;  // ���� NPC�� ��ȭ ID
    public GameObject parentObject;
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ���� ��ȭ â ����
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueId);
        }
    }
    public void DeleteMyParent()
    {
        if (transform.parent != null)
        {
            parentObject = transform.parent.gameObject; // ���� ������Ʈ ����
            Destroy(parentObject); // ���� ������Ʈ ����
            Debug.Log($"���� ������Ʈ '{parentObject.name}'�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("���� ������Ʈ�� �����ϴ�.");
        }
    }
}

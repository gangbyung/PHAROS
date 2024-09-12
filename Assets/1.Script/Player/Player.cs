using UnityEngine;

public class Player : MonoBehaviour
{
    public TalkManager talkManager;
    public GameManager manager;
    GameObject scanObject;

    private void Update()
    {
        Next();
    }

    public void Next()
    {
        if (Input.GetMouseButtonDown(0) && scanObject != null)
        {
            manager.NextTalk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            scanObject = other.gameObject;
            manager.Action(scanObject); // NPC���� ��ȣ�ۿ� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC") && scanObject != null)
        {
            scanObject = null;
        }
    }
}

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
            // ��ȭ�� �������� ���ο� ���� �ٸ��� ����
            if (!talkManager.IsTalkFinished())
            {
                manager.NextTalk();
                talkManager.NextTalk(scanObject);
            }
            else
            {
                // ��ȭ ���� �� NPC ����
                talkManager.NextTalk(scanObject);
                // ��ȭ ���� �ʱ�ȭ
                talkManager.ResetTalk();
                scanObject = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        scanObject = other.gameObject;
        if (other.CompareTag("NPC") && scanObject != null)
        {
            manager.Action(scanObject);
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

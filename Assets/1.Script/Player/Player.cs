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
            // 대화가 끝났는지 여부에 따라 다르게 동작
            if (!talkManager.IsTalkFinished())
            {
                manager.NextTalk();
                talkManager.NextTalk(scanObject);
            }
            else
            {
                // 대화 종료 및 NPC 삭제
                talkManager.NextTalk(scanObject);
                // 대화 상태 초기화
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

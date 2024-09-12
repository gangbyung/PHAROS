using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Image portraitImg;
    public Text talkTextUI;
    public Text nameTextUI;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    public int nameIndex;

    private static GameManager _instance;
    private static readonly object _lock = new object();

    public static GameManager Instance // 싱글톤 패턴
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<GameManager>();

                        if (_instance == null)
                        {
                            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
                        }
                        else
                        {
                            DontDestroyOnLoad(_instance.gameObject);
                        }
                    }
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }

        if (talkManager == null)
        {
            Debug.LogError("TalkManager가 설정되지 않았습니다. TalkManager가 있는지 확인하세요.");
        }
    }

    public void Action(GameObject scanObj) // 오브젝트 스캔
    {
        if (scanObj == null)
        {
            Debug.LogError("scanObj가 null입니다!");
            return;
        }

        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogError("스캔된 오브젝트에 ObjectData 컴포넌트가 없습니다!");
            return;
        }

        talkIndex = 0; // 대화 시작 시 인덱스 초기화
        nameIndex = 0;
        Talk(objData.id, objData.isNpc);
        talkPanel.SetActive(isAction);
    }

    public void NextTalk()
    {
        if (scanObject == null) return;

        ObjectData objData = scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogError("스캔된 오브젝트에 ObjectData 컴포넌트가 없습니다!");
            return;
        }

        Talk(objData.id, objData.isNpc);
    }

    public void EndTalk()
    {
        isAction = false;
        talkIndex = 0;
        nameIndex = 0;
        talkPanel.SetActive(false);

        // NPC 오브젝트 삭제
        if (scanObject != null)
        {
            Destroy(scanObject);
            scanObject = null;
        }
    }

    public void Talk(int id, bool isNpc) // 대사 내보내기
    {
        if (talkManager == null)
        {
            Debug.LogError("TalkManager 인스턴스가 null입니다.");
            return;
        }

        string talkData = talkManager.GetTalk(id, talkIndex);
        string nameData = talkManager.GetName(id, nameIndex);

        if (talkData == null) // 더 이상 대사가 없을 때
        {
            EndTalk(); // 대화 종료 처리
            return;
        }

        if (isNpc)
        {
            talkTextUI.text = talkData.Split(':')[0];
            nameTextUI.text = nameData.Split('&')[0];

            int portraitIndex;
            if (int.TryParse(talkData.Split(':')[1], out portraitIndex))
            {
                portraitImg.sprite = talkManager.GetPortrait(id, portraitIndex);
                portraitImg.color = new Color(1, 1, 1, 1);
            }
            else
            {
                Debug.LogWarning("초상화 인덱스를 변환할 수 없습니다.");
            }
        }
        else
        {
            talkTextUI.text = talkData;
            nameTextUI.text = nameData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        talkIndex++; // 다음 대사를 위해 인덱스를 증가시킴
        nameIndex++;

        isAction = true;
    }
}

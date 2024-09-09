using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    TalkManager talkManager;
    public GameObject talkPanel;
    public Image portraiImg;
    public Text talkText;
    public Text NameText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    public int NameIndex;

    public GameObject TalkManager;

    private static GameManager _instance;
    private static readonly object _lock = new object();

    public static GameManager Instance //�̱��� ����
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
        if(talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
        }
        if (talkManager == null)
        {
            Debug.LogError("TalkManager�� �������� �ʾҽ��ϴ�. TalkManager�� �ִ��� Ȯ���ϼ���.");
        }
    }
    public void Action(GameObject scanObj) // ������Ʈ ��ĵ
    {
        if (scanObj == null)
        {
            Debug.LogError("scanObj is null!");
            return;
        }

        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogError("ObjectData component is missing on the scanned object!");
            return;
        }

        Talk(objData.id, objData.isNpc);
        talkPanel.SetActive(isAction);
    }
    public void NextTalk()
    {
        ObjectData objData = scanObject.GetComponent<ObjectData>();

        Talk(objData.id, objData.isNpc);
        talkPanel.SetActive(isAction);
    }

    public void Talk(int id, bool isNpc) //��� ��������
    {
        if (talkManager != null)
        {
            // talkManager �ν��Ͻ��� null�� �ƴϸ�, �ش� �޼��带 ȣ���մϴ�.
            string talkText = talkManager.GetTalk(id, 0);

        }
        else
        {
            Debug.LogError("TalkManager �ν��Ͻ��� null�Դϴ�.");
        }

        string talkData = talkManager.GetTalk(id, talkIndex);

        string NameData = talkManager.GetName(id, NameIndex);

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            NameIndex = 0;
            return;
        }
        if (isNpc)
        {
            talkText.text = talkData.Split(':')[0];
            NameText.text = NameData.Split('&')[0];

            portraiImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraiImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;
            NameText.text = NameData;

            portraiImg.color = new Color(1, 1, 1, 0);
        }
        isAction = true;
        talkIndex++;
        NameIndex++;
    }
}

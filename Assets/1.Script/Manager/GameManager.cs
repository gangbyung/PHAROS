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

    public static GameManager Instance // �̱��� ����
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
                            Debug.LogError("GameManager �ν��Ͻ��� ã�� �� �����ϴ�.");
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
            Debug.LogError("TalkManager�� �������� �ʾҽ��ϴ�. TalkManager�� �ִ��� Ȯ���ϼ���.");
        }
    }

    public void Action(GameObject scanObj) // ������Ʈ ��ĵ
    {
        if (scanObj == null)
        {
            Debug.LogError("scanObj�� null�Դϴ�!");
            return;
        }

        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();

        if (objData == null)
        {
            Debug.LogError("��ĵ�� ������Ʈ�� ObjectData ������Ʈ�� �����ϴ�!");
            return;
        }

        talkIndex = 0; // ��ȭ ���� �� �ε��� �ʱ�ȭ
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
            Debug.LogError("��ĵ�� ������Ʈ�� ObjectData ������Ʈ�� �����ϴ�!");
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

        // NPC ������Ʈ ����
        if (scanObject != null)
        {
            Destroy(scanObject);
            scanObject = null;
        }
    }

    public void Talk(int id, bool isNpc) // ��� ��������
    {
        if (talkManager == null)
        {
            Debug.LogError("TalkManager �ν��Ͻ��� null�Դϴ�.");
            return;
        }

        string talkData = talkManager.GetTalk(id, talkIndex);
        string nameData = talkManager.GetName(id, nameIndex);

        if (talkData == null) // �� �̻� ��簡 ���� ��
        {
            EndTalk(); // ��ȭ ���� ó��
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
                Debug.LogWarning("�ʻ�ȭ �ε����� ��ȯ�� �� �����ϴ�.");
            }
        }
        else
        {
            talkTextUI.text = talkData;
            nameTextUI.text = nameData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        talkIndex++; // ���� ��縦 ���� �ε����� ������Ŵ
        nameIndex++;

        isAction = true;
    }
}

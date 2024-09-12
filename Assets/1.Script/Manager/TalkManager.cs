using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public GameObject choiceUI;
    public Button choiceButton1;
    public Button choiceButton2;
    public TextMeshProUGUI choiceButton1Text;
    public TextMeshProUGUI choiceButton2Text;

    public GameObject talkPanel;

    private Action<int> onChoiceMade;

    Dictionary<int, string[]> talkData;
    Dictionary<int, string[]> NameData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    public Sprite[] player_portraitArr;


    public static TalkManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            talkData = new Dictionary<int, string[]>();
            NameData = new Dictionary<int, string[]>();
            portraitData = new Dictionary<int, Sprite>();
            GenerateData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void GenerateData()
    {
        talkData.Add(1000, new string[]
        {
            "���1:0",
            "���2:1"
        });
        NameData.Add(1000, new string[] { "�̸�1&0", "�̸�2&1"});


        portraitData.Add(1000 + 0, portraitArr[1]); // 1000
        portraitData.Add(1000 + 1, portraitArr[1]); // 1001
    }

    public string GetTalk(int id, int talkIndex)
    {
        // Ű�� �����ϴ��� Ȯ��
        if (talkData.ContainsKey(id))
        {
            if (talkIndex < talkData[id].Length)
            {
                return talkData[id][talkIndex];
            }
            else
            {
                Debug.LogWarning("{id} {talkIndex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("'{id}'talkData�� �������� ����");
            return null;
        }
    }

    public string GetName(int id, int NameIndex)
    {
        // Ű�� �����ϴ��� Ȯ��
        if (NameData.ContainsKey(id))
        {
            if (NameIndex < NameData[id].Length)
            {
                return NameData[id][NameIndex];
            }
            else
            {
                Debug.LogWarning($"Ű '{id}'�� ���� ��ȿ���� ���� �̸� �ε���: {NameIndex}");
                talkPanel.SetActive(false);
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"Ű '{id}'�� NameData�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        int key = id + portraitIndex;

        // Ű�� �����ϴ��� Ȯ��
        if (portraitData.ContainsKey(key))
        {
            Sprite portrait = portraitData[key];

            
            if (id == 1000 && portraitIndex == 1)
            {
                
                ShowChoiceUI("���󰣴�", "������ ���ư���", (choice) =>
                {
                    if (choice == 1)
                    {
                        if (talkPanel.activeSelf)
                        {
                            GameManager.Instance.NextTalk();
                        }
                    }
                    else if (choice == 2)
                    {
                        
                    }
                });
            
            
            }

            return portrait; // ���� ��ȯ
        }
        else
        {
            Debug.LogWarning($"Ű '{key}'�� portraitData�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

    // ���� UI�� ���̰� �ϰ�, ��ư�� �ؽ�Ʈ�� �ݹ��� ����
    void ShowChoiceUI(string choice1Text, string choice2Text, Action<int> callback)
    {
        if (choiceUI != null)
        {
            choiceButton1Text.text = choice1Text;
            choiceButton2Text.text = choice2Text;

            onChoiceMade = callback;

            choiceButton1.onClick.RemoveAllListeners();
            choiceButton2.onClick.RemoveAllListeners();

            choiceButton1.onClick.AddListener(() => OnChoiceButtonClicked(1));
            choiceButton2.onClick.AddListener(() => OnChoiceButtonClicked(2));

            choiceUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Choice UI is not assigned!");
        }
    }

    void HideChoiceUI()
    {
        if (choiceUI != null)
        {
            choiceUI.SetActive(false);
        }
    }

    void OnChoiceButtonClicked(int choice)
    {
        HideChoiceUI();

        if (onChoiceMade != null)
        {
            onChoiceMade(choice);
        }
    }
}

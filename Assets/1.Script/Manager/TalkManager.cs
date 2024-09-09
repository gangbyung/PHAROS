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

    public bool isTalkFinished = false;
    private GameObject currentNpc;
    private bool waitingForFinalInput = false;

    public static TalkManager Instance { get; private set; }

    void Awake()
    {
        // ��ųʸ� �ʱ�ȭ
        talkData = new Dictionary<int, string[]>();
        NameData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();

        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[]
        {
        "���1:0",
        "���2:1",
        });
        NameData.Add(1000, new string[] { "�̸�1&0", "�̸�2&1" });

        if (portraitArr.Length > 1)
        {
            // portraitData�� Ű�� �ùٸ��� �߰�
            portraitData.Add(1000 + 0, portraitArr[1]); // 1000
            portraitData.Add(1000 + 1, portraitArr[1]); // 1001
                                                        // �ʿ��� ��� �ٸ� Ű�� ���� �߰��մϴ�.
                                                        // ��: portraitData.Add(1000 + 2, portraitArr[1]); // 1002
        }
        else
        {
            Debug.LogWarning("portraitArr�� ����� ��Ұ� �����ϴ�.");
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        int key = id + portraitIndex;

        // Debug.Log�� ����Ͽ� ���� ��ųʸ� ���¿� key�� Ȯ��
        Debug.Log($"Trying to get portrait with key: {key}");

        if (portraitData != null && portraitData.ContainsKey(key))
        {
            Sprite portrait = portraitData[key];

            if (id == 4000 && portraitIndex == 7)
            {
                // NPC Ȱ��ȭ �Լ�
            }
            else if (id == 5000 && portraitIndex == 7)
            {
                ShowChoiceUI("���󰣴�", "������ ���ư���", (choice) =>
                {
                    if (choice == 1)
                    {
                        // ���� 1 ����
                    }
                    else if (choice == 2)
                    {
                        // ���� 2 ����
                    }
                });
            }
            return portrait;
        }
        else
        {
            Debug.LogWarning($"Ű '{key}'�� portraitData�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

    public void NextTalk(GameObject npc)
    {
        if (waitingForFinalInput)
        {
            EndTalk(npc);
            waitingForFinalInput = false; // ���� �ʱ�ȭ
        }
        else
        {
            // ��ȭ�� �����ٰ� ������ ��
            if (!isTalkFinished)
            {
                isTalkFinished = true;
                currentNpc = npc; // ���� ��ȭ ���� NPC ����
                Debug.Log("��ȭ�� �������ϴ�. �� �� �� �����ּ���.");
                waitingForFinalInput = true; // ���� �Է� ��� ���·� ����
            }
        }
    }

    public void EndTalk(GameObject npc)
    {
        if (npc != null)
        {
            Destroy(npc);
            GameManager.Instance.NextTalk();
            Debug.Log("NPC ������Ʈ�� �����Ǿ����ϴ�.");
        }
        ResetTalk();
    }

    public bool IsTalkFinished()
    {
        return isTalkFinished;
    }

    public void ResetTalk()
    {
        isTalkFinished = false;
        waitingForFinalInput = false; // ���� �ʱ�ȭ
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkData != null && talkData.ContainsKey(id))
        {
            if (talkIndex < talkData[id].Length)
            {
                return talkData[id][talkIndex];
            }
            else
            {
                Debug.LogWarning($"Ű '{id}'�� ���� ��ȿ���� ���� ��ȭ �ε���: {talkIndex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"Ű '{id}'�� talkData�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

    public string GetName(int id, int NameIndex)
    {
        if (NameData != null && NameData.ContainsKey(id))
        {
            if (NameIndex < NameData[id].Length)
            {
                return NameData[id][NameIndex];
            }
            else
            {
                Debug.LogWarning($"Ű '{id}'�� ���� ��ȿ���� ���� �̸� �ε���: {NameIndex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"Ű '{id}'�� NameData�� �������� �ʽ��ϴ�.");
            return null;
        }
    }

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

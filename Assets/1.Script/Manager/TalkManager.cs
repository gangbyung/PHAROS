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
        // 딕셔너리 초기화
        talkData = new Dictionary<int, string[]>();
        NameData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();

        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[]
        {
        "대사1:0",
        "대사2:1",
        });
        NameData.Add(1000, new string[] { "이름1&0", "이름2&1" });

        if (portraitArr.Length > 1)
        {
            // portraitData에 키를 올바르게 추가
            portraitData.Add(1000 + 0, portraitArr[1]); // 1000
            portraitData.Add(1000 + 1, portraitArr[1]); // 1001
                                                        // 필요한 경우 다른 키와 값을 추가합니다.
                                                        // 예: portraitData.Add(1000 + 2, portraitArr[1]); // 1002
        }
        else
        {
            Debug.LogWarning("portraitArr에 충분한 요소가 없습니다.");
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        int key = id + portraitIndex;

        // Debug.Log를 사용하여 현재 딕셔너리 상태와 key를 확인
        Debug.Log($"Trying to get portrait with key: {key}");

        if (portraitData != null && portraitData.ContainsKey(key))
        {
            Sprite portrait = portraitData[key];

            if (id == 4000 && portraitIndex == 7)
            {
                // NPC 활성화 함수
            }
            else if (id == 5000 && portraitIndex == 7)
            {
                ShowChoiceUI("따라간다", "집으로 돌아간다", (choice) =>
                {
                    if (choice == 1)
                    {
                        // 선택 1 로직
                    }
                    else if (choice == 2)
                    {
                        // 선택 2 로직
                    }
                });
            }
            return portrait;
        }
        else
        {
            Debug.LogWarning($"키 '{key}'가 portraitData에 존재하지 않습니다.");
            return null;
        }
    }

    public void NextTalk(GameObject npc)
    {
        if (waitingForFinalInput)
        {
            EndTalk(npc);
            waitingForFinalInput = false; // 상태 초기화
        }
        else
        {
            // 대화가 끝났다고 가정할 때
            if (!isTalkFinished)
            {
                isTalkFinished = true;
                currentNpc = npc; // 현재 대화 중인 NPC 저장
                Debug.Log("대화가 끝났습니다. 한 번 더 눌러주세요.");
                waitingForFinalInput = true; // 최종 입력 대기 상태로 변경
            }
        }
    }

    public void EndTalk(GameObject npc)
    {
        if (npc != null)
        {
            Destroy(npc);
            GameManager.Instance.NextTalk();
            Debug.Log("NPC 오브젝트가 삭제되었습니다.");
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
        waitingForFinalInput = false; // 상태 초기화
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
                Debug.LogWarning($"키 '{id}'에 대한 유효하지 않은 대화 인덱스: {talkIndex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"키 '{id}'가 talkData에 존재하지 않습니다.");
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
                Debug.LogWarning($"키 '{id}'에 대한 유효하지 않은 이름 인덱스: {NameIndex}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"키 '{id}'가 NameData에 존재하지 않습니다.");
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

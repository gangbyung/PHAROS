using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Image imageComponent;
    public Button choiceButton1;
    public Button choiceButton2;
    public TextMeshProUGUI choiceButtonText1;
    public TextMeshProUGUI choiceButtonText2;

    public Sprite[] npcSprites; // NPC 스프라이트 배열

    private Dictionary<int, string[]> talkData = new Dictionary<int, string[]>();
    private Dictionary<int, string[]> nameData = new Dictionary<int, string[]>();
    private Dictionary<string, int> npcNameToID = new Dictionary<string, int>();

    private int currentDialogueIndex = 0;
    private int currentDialogueID;
    private bool isChoiceActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        InitializeDialogueData();
        InitializeNameData();
        InitializeNpcNameToID();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && !isChoiceActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextDialogue();
        }
    }

    void InitializeDialogueData()
    {
        talkData.Add(1000, new string[] {
            "어유, 간만에 산책나왔더니만 계속 날씨가 이모양이네 ...:0",
            "발전소에서 뭔가 생긴 이후부터 하늘이 기분나쁘게 먹구름만 가득하지 뭐야:1",
            "그러고 보니 총각, 옛날에 저기서 일한다고 했었던 것 같은데, 뭔지 알어?:2",
            "아마 발전소 폭발이랑 연관은 없을 거에요.:3",
            "그래? 그럼 뭐 그렇게 큰 일도 아니구만.:4",
            "아, 조금있으면 장마철이기도 했구만,:5",
            "'...상황이 어떻게 되고 있는지는 몰라도 호우로 방사능 물질이 근방에 유출되면 큰일이 날텐데':6",
            ":7" // 공백 대사
        });

        talkData.Add(2000, new string[] {
            "어서 오세요! 여기는 우리 마을의 중요한 곳입니다.:0",
            "이곳은 예전부터 중요한 자원으로 여겨졌죠.:1",
            "무엇을 도와드릴까요?:2",
            "이곳의 자원은 고갈되어 가고 있습니다.:3",
            "무엇인가 좋은 방안이 있을까요?:4",
            "저희가 가진 정보로는 도움이 될까요?:5",
            "기대해 주세요, 좋은 소식이 있을지도 모르죠.:6",
            ":7" // 공백 대사
        });

        talkData.Add(3000, new string[] {
            "이곳은 상당히 위험한 곳입니다.:0",
            "많은 사람들이 이곳을 피하고 있죠.:1",
            "하지만 이곳에서 해결책을 찾는다면 좋겠군요.:2",
            ":3" // 공백 대사
        });
    }

    void InitializeNameData()
    {
        nameData.Add(1000, new string[] { "선캡아줌마&0", "선캡아줌마&1", "선캡아줌마&2", "주인공&3", "선캡아줌마&4", "선캡아줌마&5", "주인공&6" });
        nameData.Add(2000, new string[] { "마을 주민&0", "마을 주민&1", "마을 주민&2", "마을 주민&3", "마을 주민&4", "마을 주민&5", "마을 주민&6" });
        nameData.Add(3000, new string[] { "위험한 사람&0", "위험한 사람&1", "위험한 사람&2" });
    }

    void InitializeNpcNameToID()
    {
        npcNameToID.Add("NPC1", 1000);
        npcNameToID.Add("NPC2", 2000);
        npcNameToID.Add("NPC3", 3000);
    }

    public void StartDialogue(string npcName)
    {
        if (npcNameToID.TryGetValue(npcName, out int dialogueID))
        {
            if (talkData.ContainsKey(dialogueID))
            {
                currentDialogueID = dialogueID;
                currentDialogueIndex = 0;
                dialoguePanel.SetActive(true);
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning($"'{dialogueID}'에 해당하는 대사가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"'{npcName}'에 해당하는 대사 ID가 없습니다.");
        }
    }

    void DisplayCurrentDialogue()
    {
        if (currentDialogueIndex < talkData[currentDialogueID].Length)
        {
            string dialogueLine = talkData[currentDialogueID][currentDialogueIndex];
            if (string.IsNullOrEmpty(dialogueLine))
            {
                // 공백 대사 처리
                currentDialogueIndex++;
                DisplayCurrentDialogue();
                return;
            }

            string[] parts = dialogueLine.Split(':');
            dialogueText.text = parts[0];
            int speakerIndex = int.Parse(parts[1]);

            if (nameData[currentDialogueID].Length > speakerIndex)
            {
                speakerNameText.text = nameData[currentDialogueID][speakerIndex];
            }
            else
            {
                speakerNameText.text = "알 수 없는 인물";
            }

            if (speakerIndex >= 0 && speakerIndex < npcSprites.Length)
            {
                imageComponent.sprite = npcSprites[speakerIndex];
            }
            else
            {
                imageComponent.sprite = null;
            }

            currentDialogueIndex++;

            if (currentDialogueIndex >= talkData[currentDialogueID].Length)
            {
                dialoguePanel.SetActive(false);
                imageComponent.sprite = null;
                isChoiceActive = false;
                UpdateChoiceButtons();
            }
        }
        else
        {
            Debug.Log("모든 대사를 출력했습니다.");
            dialoguePanel.SetActive(false);
            imageComponent.sprite = null;
        }
    }

    void DisplayNextDialogue()
    {
        if (currentDialogueIndex < talkData[currentDialogueID].Length)
        {
            DisplayCurrentDialogue();
        }
    }

    void UpdateChoiceButtons()
    {
        if (talkData[currentDialogueID].Length > 2)
        {
            isChoiceActive = true;

            if (talkData[currentDialogueID].Length > 1)
            {
                choiceButton1.gameObject.SetActive(true);
                choiceButtonText1.text = "선택지 1"; // 실제 선택지 텍스트로 변경
                choiceButton1.onClick.RemoveAllListeners();
                choiceButton1.onClick.AddListener(() => OnChoiceSelected(2001)); // 실제로는 다음 대사 ID로 대체
            }
            else
            {
                choiceButton1.gameObject.SetActive(false);
            }

            if (talkData[currentDialogueID].Length > 2)
            {
                choiceButton2.gameObject.SetActive(true);
                choiceButtonText2.text = "선택지 2"; // 실제 선택지 텍스트로 변경
                choiceButton2.onClick.RemoveAllListeners();
                choiceButton2.onClick.AddListener(() => OnChoiceSelected(2002)); // 실제로는 다음 대사 ID로 대체
            }
            else
            {
                choiceButton2.gameObject.SetActive(false);
            }
        }
        else
        {
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);
            isChoiceActive = false;
        }
    }

    void OnChoiceSelected(int nextDialogueID)
    {
        if (talkData.ContainsKey(nextDialogueID))
        {
            currentDialogueID = nextDialogueID;
            currentDialogueIndex = 0;
            DisplayCurrentDialogue();
        }
        else
        {
            Debug.LogWarning($"'{nextDialogueID}'에 해당하는 대사가 없습니다.");
            isChoiceActive = false;
        }
    }
}

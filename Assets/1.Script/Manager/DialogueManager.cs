using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static DialogueManager instance;

    // 일반 대화 UI 요소
    public GameObject normalDialoguePanel; // 일반 대화창 패널
    public TextMeshProUGUI npcNameTextNormal; // 일반 대화창의 NPC 이름 텍스트
    public TextMeshProUGUI dialogueTextNormal; // 일반 대화창의 대사 텍스트
    public Image npcImageNormal; // 일반 대화창의 NPC 이미지

    // 선택지 대화 UI 요소
    public GameObject choiceDialoguePanel; // 선택지 대화창 패널
    public TextMeshProUGUI npcNameTextChoice; // 선택지 대화창의 NPC 이름 텍스트
    public TextMeshProUGUI dialogueTextChoice; // 선택지 대화창의 대사 텍스트
    public Image npcImageChoice; // 선택지 대화창의 NPC 이미지

    public Button choiceButton1; // 첫 번째 선택지 버튼
    public Button choiceButton2; // 두 번째 선택지 버튼
    public Button choiceButton3; // 세 번째 선택지 버튼

    // 대화 데이터
    private Dictionary<int, DialogueData> dialogues; // 대화 데이터를 담고 있는 딕셔너리
    private int currentDialogueId = 0; // 현재 대화 ID
    private int currentLineIndex = 0; // 현재 대사 줄 인덱스

    private bool isDialogueActive = false; // 대화 활성화 여부
    private bool waitingForChoice = false; // 선택지 대기 상태 여부

    // 점수 변수
    private int score;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            instance = this; // 인스턴스를 현재 객체로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 삭제되지 않도록 설정
        }
    }

    void Start()
    {
        // 대화 데이터 초기화
        dialogues = new Dictionary<int, DialogueData>
        {
            {
                0, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("안녕! 너의 선택은 뭐야?", "npc1", Resources.Load<Sprite>("npc1_image"), true, new string[] { "1번 선택", "2번 선택", "3번 선택" }, new int[] { 1, 2, 3 })
                    }
                )
            },
            {
                1, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("1번이구나, 좋은 선택이야!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                2, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("2번이구나, 괜찮은 선택이야!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                3, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("3번을 선택했구나, 멋진 선택이야!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            }
        };

        // 초기에는 모든 대화창을 숨김
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(false);
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);

        // 점수 초기화
        score = 0;
    }

    void Update()
    {
        // 마우스 클릭 시 다음 대사로 진행
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            // 선택지를 기다리는 중이라면 진행하지 않음
            if (!waitingForChoice)
            {
                ShowNextLine();
            }
        }
    }

    // 특정 대화 ID로 대화를 시작
    public void ShowDialogue(int dialogueId)
    {
        currentDialogueId = dialogueId; // 현재 대화 ID 설정
        currentLineIndex = 0; // 대사 줄 인덱스 초기화
        isDialogueActive = true; // 대화 활성화 상태 설정

        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            // 일반 대화 UI 업데이트
            npcNameTextNormal.text = dialogueData.npcName;
            dialogueTextNormal.text = "";
            npcImageNormal.sprite = null;

            // 선택지 대화 UI 업데이트
            npcNameTextChoice.text = dialogueData.npcName;
            dialogueTextChoice.text = "";
            npcImageChoice.sprite = null;

            // 첫 번째 대사 줄 표시
            ShowNextLine();
        }
    }

    // 다음 대사 줄을 표시
    public void ShowNextLine()
    {
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            if (currentLineIndex < dialogueData.lines.Length)
            {
                DialogueLine line = dialogueData.lines[currentLineIndex];

                if (line.hasChoices && line.choices != null)
                {
                    // 일반 대화창 숨기고 선택지 대화창 표시
                    normalDialoguePanel.SetActive(false);
                    choiceDialoguePanel.SetActive(true);

                    // 선택지 대화 텍스트 업데이트
                    dialogueTextChoice.text = line.text;

                    // 선택지 대화창의 NPC 이미지 업데이트
                    npcImageChoice.sprite = line.npcImage;

                    // 선택지 버튼에 텍스트 설정
                    choiceButton1.gameObject.SetActive(true);
                    choiceButton2.gameObject.SetActive(true);
                    choiceButton3.gameObject.SetActive(true);

                    choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[0];
                    choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[1];
                    choiceButton3.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[2];

                    // 선택지 버튼에 리스너 추가
                    choiceButton1.onClick.RemoveAllListeners();
                    choiceButton2.onClick.RemoveAllListeners();
                    choiceButton3.onClick.RemoveAllListeners();
                    choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
                    choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
                    choiceButton3.onClick.AddListener(() => OnChoiceSelected(2));

                    // 선택지를 기다리는 상태로 변경
                    waitingForChoice = true;
                }
                else
                {
                    // 일반 대화창 표시하고 선택지 대화창 숨기기
                    normalDialoguePanel.SetActive(true);
                    choiceDialoguePanel.SetActive(false);

                    // 일반 대화 텍스트 업데이트
                    dialogueTextNormal.text = line.text;

                    // 일반 대화창의 NPC 이미지 업데이트
                    npcImageNormal.sprite = line.npcImage;

                    // 선택지 버튼 숨기기
                    choiceButton1.gameObject.SetActive(false);
                    choiceButton2.gameObject.SetActive(false);
                    choiceButton3.gameObject.SetActive(false);

                    // 다음 대사 줄로 진행
                    currentLineIndex++;
                }
            }
            else
            {
                // 더 이상 대사가 없으면 대화 종료
                EndDialogue();
            }
        }
    }

    // 선택지가 선택되었을 때 처리
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"플레이어가 선택을 했습니다: {choiceIndex}");

        // 선택지 버튼 숨기기
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);

        // 선택이 완료되었으므로 선택지 대기 상태 해제
        waitingForChoice = false;

        // 선택에 따른 다음 대화 ID 설정 및 대사 줄 초기화
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            if (line.hasChoices && line.nextDialogueIds != null && choiceIndex < line.nextDialogueIds.Length)
            {
                int nextDialogueId = line.nextDialogueIds[choiceIndex];

                // 선택지에 따른 점수 누적
                switch (choiceIndex)
                {
                    case 0: // 첫 번째 선택지
                        score += 10; // 점수 추가 (예: 10점)
                        break;
                    case 1: // 두 번째 선택지
                        score += 20; // 점수 추가 (예: 20점)
                        break;
                    case 2: // 세 번째 선택지
                        score += 30; // 점수 추가 (예: 30점)
                        break;
                }
                Debug.Log($"현재 점수: {score}");

                // 다음 대화 ID 설정
                currentDialogueId = nextDialogueId;
                currentLineIndex = 0; // 다음 대화의 첫 번째 줄로 초기화
                ShowNextLine(); // 다음 대사를 보여줌
            }
        }
    }

    // 대화 종료 처리
    void EndDialogue()
    {
        isDialogueActive = false; // 대화 비활성화
        normalDialoguePanel.SetActive(false); // 일반 대화창 숨기기
        choiceDialoguePanel.SetActive(false); // 선택지 대화창 숨기기
        Debug.Log("대화가 종료되었습니다.");
    }

    // 대화 데이터 구조체
    [System.Serializable]
    public struct DialogueData
    {
        public string npcName; // NPC 이름
        public DialogueLine[] lines; // 대사 줄 배열

        public DialogueData(string npcName, DialogueLine[] lines)
        {
            this.npcName = npcName;
            this.lines = lines;
        }
    }

    // 대사 줄 구조체
    [System.Serializable]
    public struct DialogueLine
    {
        public string text; // 대사 텍스트
        public string npcName; // NPC 이름
        public Sprite npcImage; // NPC 이미지
        public bool hasChoices; // 선택지가 있는지 여부
        public string[] choices; // 선택지 배열
        public int[] nextDialogueIds; // 다음 대화 ID 배열

        public DialogueLine(string text, string npcName, Sprite npcImage, bool hasChoices = false, string[] choices = null, int[] nextDialogueIds = null)
        {
            this.text = text;
            this.npcName = npcName;
            this.npcImage = npcImage;
            this.hasChoices = hasChoices;
            this.choices = choices;
            this.nextDialogueIds = nextDialogueIds;
        }
    }
}

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
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("헤헤, 아린이 배 타고 한국에 처음 도착했던 거 기억나?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("그때 일본이랑 달라서 완전 신기했지!", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("부모님도 한국 땅 밟자마자 엄청 좋아하셨잖아?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("아린이도 그때 '여기가 진짜 내 고향인가?' 하고 슬슬 느꼈을 것 같은데, 맞지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "1번 선택: 과거를 되돌아보며 차분히 대답한다",
                                "2번 선택: 미소 지으며 긍정적으로 대답한다",
                                "3번 선택: 조용히 고개를 끄덕인다"
                            },
                            new int[] { 1, 2, 3 },
                            new int[] {50,20,30}
                        )
                    }
                )
            },
            {
                1, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("그래... 그때의 기억은 항상 나에게 특별해. 어렸을 때 느꼈던 그 감정들이 지금도 나를 지탱해주는 것 같아.", "아름이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("고마워. 넌 항상 그때의 나를 기억해주는구나.", "아름이", Resources.Load<Sprite>("npc1_image"))
                    }
                )

            },
            {
                2, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("그래... 그때의 기억은 항상 나에게 특별해. 어렸을 때 느꼈던 그 감정들이 지금도 나를 지탱해주는 것 같아.", "아름이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("고마워. 넌 항상 그때의 나를 기억해주는구나.", "아름이", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                3, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("그래... 그때의 기억은 항상 나에게 특별해. 어렸을 때 느꼈던 그 감정들이 지금도 나를 지탱해주는 것 같아.", "아름이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("고마워. 넌 항상 그때의 나를 기억해주는구나.", "아름이", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            }
            // 추가 대화 데이터...
        };

        // 초기에는 모든 대화창을 숨김
        HideAllDialoguePanels();

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
            // 대화 UI 초기화
            InitializeDialogueUI(dialogueData);

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
                    // 선택지 대화 UI로 전환
                    ShowChoiceDialogue(line);
                }
                else
                {
                    // 일반 대화 UI로 전환
                    ShowNormalDialogue(line);
                    currentLineIndex++; // 다음 대사 줄로 진행
                }
            }
            else
            {
                // 더 이상 대사가 없으면 대화 종료
                EndDialogue();
            }
        }
    }

    // 일반 대화 UI 업데이트
    private void ShowNormalDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(true);
        choiceDialoguePanel.SetActive(false);

        dialogueTextNormal.text = line.text;
        npcNameTextNormal.text = line.npcName;
        npcImageNormal.sprite = line.npcImage;

        // 선택지 버튼 숨기기
        HideChoiceButtons();
    }

    // 선택지 대화 UI 업데이트
    private void ShowChoiceDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(true);

        dialogueTextChoice.text = line.text;
        npcNameTextChoice.text = line.npcName;
        npcImageChoice.sprite = line.npcImage;

        // 선택지 버튼 업데이트
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);
        choiceButton3.gameObject.SetActive(true);

        choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[0];
        choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[1];
        choiceButton3.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[2];

        // 선택지 버튼 리스너 추가
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();
        choiceButton3.onClick.RemoveAllListeners();
        choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
        choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
        choiceButton3.onClick.AddListener(() => OnChoiceSelected(2));

        waitingForChoice = true; // 선택지를 기다리는 상태로 변경
    }

    // 선택지가 선택되었을 때 처리
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"플레이어가 선택을 했습니다: {choiceIndex}");

        HideChoiceButtons();
        waitingForChoice = false; // 선택지 대기 상태 해제

        // 현재 대화 ID에서 대화 데이터를 가져오기
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            if (line.hasChoices && line.nextDialogueIds != null && choiceIndex < line.nextDialogueIds.Length)
            {
                // 점수 업데이트
                UpdateScore(choiceIndex);

                int nextDialogueId = line.nextDialogueIds[choiceIndex];
                currentDialogueId = nextDialogueId;
                currentLineIndex = 0; // 다음 대화의 첫 번째 줄로 초기화

                ShowNextLine(); // 다음 대사를 보여줌
            }
        }
    }


    // 점수 업데이트
    private void UpdateScore(int choiceIndex)
    {
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];

            if (line.scores != null && choiceIndex < line.scores.Length)
            {
                int pointsToAdd = line.scores[choiceIndex]; // 선택지에 해당하는 점수 가져오기
                score += pointsToAdd; // 점수 업데이트
                Debug.Log($"선택지: {choiceIndex}, 추가 점수: {pointsToAdd}, 현재 점수: {score}"); // 디버깅 로그
            }
            else
            {
                Debug.LogWarning("선택지에 대한 점수가 없습니다.");
            }
        }
    }


    // 대화 종료 처리
    private void EndDialogue()
    {
        isDialogueActive = false; // 대화 비활성화
        HideAllDialoguePanels(); // 모든 대화창 숨김
        Debug.Log("대화가 종료되었습니다.");
    }

    // 대화 UI 초기화
    private void InitializeDialogueUI(DialogueData dialogueData)
    {
        // 일반 대화창과 선택지 대화창 모두 숨김
        HideAllDialoguePanels();

        npcNameTextNormal.text = dialogueData.npcName;
        npcImageNormal.sprite = dialogueData.npcImage;
    }

    // 모든 대화창 숨김
    private void HideAllDialoguePanels()
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(false);
    }

    // 선택지 버튼 숨기기
    private void HideChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
    }
}

// 대화 데이터 클래스
[System.Serializable]
public class DialogueData
{
    public string npcName; // NPC 이름
    public Sprite npcImage;
    public DialogueLine[] lines; // 대사 줄

    public DialogueData(string npcName, DialogueLine[] lines)
    {
        this.npcName = npcName;
        this.lines = lines;
    }
}

// 대사 줄 클래스
[System.Serializable]
public class DialogueLine
{
    public string text;
    public string npcName;
    public Sprite npcImage;
    public bool hasChoices;
    public string[] choices;
    public int[] nextDialogueIds;
    public int[] scores; // 점수 배열 추가

    public DialogueLine(string text, string npcName, Sprite npcImage, bool hasChoices = false,
        string[] choices = null, int[] nextDialogueIds = null, int[] scores = null)
    {
        this.text = text;
        this.npcName = npcName;
        this.npcImage = npcImage;
        this.hasChoices = hasChoices;
        this.choices = choices;
        this.nextDialogueIds = nextDialogueIds;
        this.scores = scores; // 점수 배열 초기화
    }
}


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

    public GameObject choiceButtonPrefab; // 선택지 버튼 프리팹
    public Transform choiceContainer; // 선택지 버튼을 넣을 Scroll View의 Content

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
                                "새로운 환경을 탐험하기로 결심하기",
                                "낮선 환경에 부모님 곁을 떠나지 않기"
                            },
                            new int[] { 1, 2},
                            new int[] {3,1}
                        )
                    }
                )
            },
            {
                1, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("그래 한번 탐험해 보자!", "아름이", Resources.Load<Sprite>("npc1_image"))
                    }
                )

            },
            {
                2, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("난 부모님과 함께 있을래..", "아름이", Resources.Load<Sprite>("npc1_image")),
                        
                    }
                )
            },
            {
                3, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이는 일본에서 태어나서 그런지 한국말 배우는 게 좀 힘들었겠지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("한국에 오자마자 동네 아이들이랑 놀고 싶었을 텐데, 말이 안 통해서 답답했을 거야", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("부모님이랑 할머니가 집에서 한국말 가르쳐주긴 했지만, 그래도 아린이는 여전히 일본어가 더 편했을 거잖아.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "더 열심히 한국어를 배우기로 결심하기",
                                "일본어와 한국어를 섞어가며 대화하기",
                                "긴장해서 아무 말도 하지 않기"
                            },
                            new int[] {4,5,6},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                4, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("더 열심히 공부해야지!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                5, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("아직은 좀 미숙하지만 노력해야지", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                6, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("긴장해서 아무말도 못할거같아..", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
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

    }

    // 선택지 대화 UI 업데이트
    // 선택지 대화 UI 업데이트
    private void ShowChoiceDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(true);

        dialogueTextChoice.text = line.text;
        npcNameTextChoice.text = line.npcName;
        npcImageChoice.sprite = line.npcImage;

        // 기존 선택지 버튼 제거
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // 선택지 버튼 생성 시 애니메이션 설정
        // 선택지 버튼 생성 시 애니메이션 실행
        for (int i = 0; i < line.choices.Length; i++)
        {
            GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[i];

            // 버튼에 있는 Image 컴포넌트를 찾아서 Animator도 찾기
            Image buttonImage = choiceButton.GetComponentInChildren<Image>();  // Image 컴포넌트 가져오기
            Animator animator = choiceButton.GetComponentInChildren<Animator>();  // Animator 컴포넌트 가져오기

            // 버튼을 클릭했을 때 처리
            int choiceIndex = i; // Closure 문제 방지
            choiceButton.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        }



        waitingForChoice = true; // 선택지를 기다리는 상태로 변경
    }


    // 선택지가 선택되었을 때 처리
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"플레이어가 선택을 했습니다: {choiceIndex}");

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


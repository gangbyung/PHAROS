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


    public NPCDialogueTrigger nPCDialogueTrigger;


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
        }
    }
    void Start()
    {
        nPCDialogueTrigger = GetComponent<NPCDialogueTrigger>();
        // 대화 데이터 초기화
        dialogues = new Dictionary<int, DialogueData>
        {
            {
                
                0, new DialogueData( 
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("헤헤, 아린이 배 타고 한국에 처음 도착했던 거 기억나?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그때 일본이랑 달라서 완전 신기했지!", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("부모님도 한국 땅 밟자마자 엄청 좋아하셨잖아?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("아린이도 그때 '여기가 진짜 내 고향인가?' 하고 슬슬 느꼈을 것 같은데, 맞지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "새로운 환경을 탐험하기로 결심하기",
                                "낮선 환경에 부모님 곁을 떠나지 않기"
                            },
                            new int[] {8,9},
                            new int[] {3,1}
                        )
                    }
                )
            },
            {
                1, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이는 일본에서 태어나서 그런지 한국말 배우는 게 좀 힘들었겠지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("한국에 오자마자 동네 아이들이랑 놀고 싶었을 텐데, 말이 안 통해서 답답했을 거야", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("부모님이랑 할머니가 집에서 한국말 가르쳐주긴 했지만, 그래도 아린이는 여전히 일본어가 더 편했을 거잖아.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "더 열심히 한국어를 배우기로 결심하기",
                                "일본어와 한국어를 섞어가며 대화하기",
                                "긴장해서 아무 말도 하지 않기"
                            },
                            new int[] {10,11,12},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                2, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이는 일본에서 먹던 음식이 많이 그리웠을 거야.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("한국 전통 음식들이 좀 낯설고 맛도 강했잖아.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "한국 음식의 새로운 맛을 즐기기",
                                "부모님의 일본 음식을 먹으며 위로 받기",
                                "몸이 작아져서 달달한 음식만 먹는 상상하기"
                            },
                            new int[] {13,14,15},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                3, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("너가 가장 아끼던 일본어 책들, 한국으로 가져왔잖아?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("근데 한국에서는 일본어 책 보는 게 별로 좋지 않다는 얘기를 들었대.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그래서 부모님이 너한테 일본어 책들은 치우고 한국어 책을 읽으라고 했는데, 너가 얼마나 아쉬웠겠어.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "한국어 책에서의 재미를 찾기",
                                "일본어 책들의 추억을 소중히 간직하기",
                                "몸이 작아져서 책 속으로 들어가는 상상하기"
                            },
                            new int[] {16,17,18},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                4, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("너가 한국 학교에 처음 간 날, 진짜 낮설고 무서웠을 거야, 안 그래?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("교실에서 자기소개할 때 일본식 이름을 말했더니, 선생님이랑 친구들이 조금 당황한 눈치였대.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그래서 너는 부모님이랑 한국식 이름을 고민하기 시작했지.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "한국식 이름을 정하기",
                                "계속해서 일본식 이름을 사용하기",
                                "자신이 작아졌다 느끼며, 소심하게 행동하기"
                            },
                            new int[] {19,20,21},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                5, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("음, 아린이는 일본에서 태어났거든.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("근데 주변 사람들은 다들 아린이를 한국인 이라고 생각해.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("뭐, 아린이는 별로 신경 안 쓰는 것 같긴 한데, 그래도 가끔은 자기가 좀 이방인처럼 느껴지나 봐.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "한국 문화에 더 많이 참여하기",
                                "자신이 일본 문화를 소중히 여기기"
                            },
                            new int[] {22,23},
                            new int[] {3,1}
                        )
                    }
                )
            },
            {
                6, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이네 가족이 한국으로 돌아왔을 때, 주변에 조금 못마땅하게 보는 사람들이 있더라구.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("'왜 일본에서 살았냐'는 말도 나왔고, 아린이 부모님은 그걸 설명하느라 좀 힘들었대.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("아린이도 그 시선 때문에 좀 슬펐겠지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "친절하게 행동하기",
                                "더 당당하게 굴기",
                                "집 안에 틀어박히기"
                            },
                            new int[] {24,25,26},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                7, new DialogueData(
                    "고양이 가면을 쓴 남자아이",
                    new DialogueLine[] {
                        new DialogueLine("시간이 지나면서 아리이는 한국에서 한 소녀랑 친해졌어.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그 친구는 아린이가 일본에서 왔다고 해서 별다른 편견도 없었고, 그냥 자연스럽게 다가와줬거든.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그래서 둘은 서로의 문화를 존중하면서 우정을 쌓아갔어.", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "아린이의 모습을 한 아름이", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "친구의 도움으로 한국 생활에 적응하기",
                                "서로의 가족과 더 가까워지기"
                            },
                            new int[] {27,28},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                8, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("그래 한번 탐험해 보자!", "아름이", Resources.Load<Sprite>("npc1_image"))
                    }
                )

            },
            {
                9, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("난 부모님과 함께 있을래..", "아름이", Resources.Load<Sprite>("npc1_image")),
                        
                    }
                )
            },
            {
                10, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("더 열심히 공부해야지!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                11, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("아직은 좀 미숙하지만 노력해야지", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                12, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("긴장해서 아무말도 못할거같아..", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                13, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("새로운 맛을 즐기자!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                14, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("부모님의 음식을 먹으며 위로받아야겠어", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                15, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("몸이 작아져 달달한 음식만 먹고싶어", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                16, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("한국어 책에서 재미를 찾자!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                17, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("일본어 책들의 추억을 소중히 간직할래", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                18, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("몸이 작아져서 책속으로 들어가고싶어", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                19, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("한국식 이름을 정해보자!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                20, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("계속 일본어 이름을 사용할래", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                21, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("소심하게 행동할래..", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                22, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("한국 문화에 더많이 참여할래!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                23, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("나의 일본 문화를 소중히 여길래", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                24, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("친절하게 행동해야지!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                25, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("더 당당하게 굴자!", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                26, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("집 안에 틀어박힐래..", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                27, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("친구의 도움을 받아 한국 생활에 적응해야겠어", "아름이", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                28, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("서로의 가족과 더 가까워질래", "아름이", Resources.Load<Sprite>("npc1_image")),

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
        nPCDialogueTrigger = FindObjectOfType<NPCDialogueTrigger>();
        nPCDialogueTrigger.DeleteMyParent();
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


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
    public Image npcImage22;

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

    public GameObject StartObj;
    // 점수 변수
    public int score;

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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("헤헤, 아린이 배 타고 한국에 처음 도착했던 거 기억나?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"),Resources.Load<Sprite>("a2")),
                        new DialogueLine("그때 일본이랑 달라서 완전 신기했지!", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"),Resources.Load<Sprite>("a2")),
                        new DialogueLine("부모님도 한국 땅 밟자마자 엄청 좋아하셨잖아?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("아린이도 그때 '여기가 진짜 내 고향인가?' 하고 슬슬 느꼈을 것 같은데, 맞지?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"),Resources.Load<Sprite>("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이는 일본에서 태어나서 그런지 한국말 배우는 게 좀 힘들었겠지?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("한국에 오자마자 동네 아이들이랑 놀고 싶었을 텐데, 말이 안 통해서 답답했을 거야", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("부모님이랑 할머니가 집에서 한국말 가르쳐주긴 했지만, 그래도 아린이는 여전히 일본어가 더 편했을 거잖아.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이는 일본에서 먹던 음식이 많이 그리웠을 거야.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("한국 전통 음식들이 좀 낯설고 맛도 강했잖아.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("너가 가장 아끼던 일본어 책들, 한국으로 가져왔잖아?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("근데 한국에서는 일본어 책 보는 게 별로 좋지 않다는 얘기를 들었대.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그래서 부모님이 너한테 일본어 책들은 치우고 한국어 책을 읽으라고 했는데, 너가 얼마나 아쉬웠겠어.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("너가 한국 학교에 처음 간 날, 진짜 낮설고 무서웠을 거야, 안 그래?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("교실에서 자기소개할 때 일본식 이름을 말했더니, 선생님이랑 친구들이 조금 당황한 눈치였대.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그래서 너는 부모님이랑 한국식 이름을 고민하기 시작했지.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("음, 아린이는 일본에서 태어났거든.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("근데 주변 사람들은 다들 아린이를 한국인 이라고 생각해.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("뭐, 아린이는 별로 신경 안 쓰는 것 같긴 한데, 그래도 가끔은 자기가 좀 이방인처럼 느껴지나 봐.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("아린이네 가족이 한국으로 돌아왔을 때, 주변에 조금 못마땅하게 보는 사람들이 있더라구.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("'왜 일본에서 살았냐'는 말도 나왔고, 아린이 부모님은 그걸 설명하느라 좀 힘들었대.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("아린이도 그 시선 때문에 좀 슬펐겠지?", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "고양이 가면을 쓴 아이",
                    new DialogueLine[] {
                        new DialogueLine("시간이 지나면서 아리이는 한국에서 한 소녀랑 친해졌어.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그 친구는 아린이가 일본에서 왔다고 해서 별다른 편견도 없었고, 그냥 자연스럽게 다가와줬거든.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그래서 둘은 서로의 문화를 존중하면서 우정을 쌓아갔어.", "고양이 가면을 쓴 아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
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
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("그래 한번 탐험해 보자!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1"))
                    }
                )

            },
            {
                9, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("난 부모님과 함께 있을래..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),
                        
                    }
                )
            },
            {
                10, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("더 열심히 공부해야지!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                11, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("아직은 좀 미숙하지만 노력해야지", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                12, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("긴장해서 아무말도 못할거같아..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                13, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("새로운 맛을 즐기자!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                14, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("부모님의 음식을 먹으며 위로받아야겠어", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                15, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("몸이 작아져 달달한 음식만 먹고싶어", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                16, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("한국어 책에서 재미를 찾자!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                17, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("일본어 책들의 추억을 소중히 간직할래", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                18, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("몸이 작아져서 책속으로 들어가고싶어", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                19, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("한국식 이름을 정해보자!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                20, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("계속 일본어 이름을 사용할래", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                21, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("소심하게 행동할래..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                22, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("한국 문화에 더많이 참여할래!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                23, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("나의 일본 문화를 소중히 여길래", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                24, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("친절하게 행동해야지!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                25, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("더 당당하게 굴자!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                26, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("집 안에 틀어박힐래..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                27, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("친구의 도움을 받아 한국 생활에 적응해야겠어", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                28, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("서로의 가족과 더 가까워질래", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                29, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린아, 전쟁이 터져서 네가 가족과 집을 떠나 피난길에 올랐어.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("밤낮없이 산길을 걷고, 폭격 소리가 계속 들리는데, 가족을 지키면서도 짐을 챙겨야 한다면 어떻게 할 것 같아?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "짐을 버리기",
                                "짐과 가족 모두 챙기기"
                            },
                            new int[] {37,38},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                37, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("짐을 버리고 가족을 지킬래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                38, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("짐과 가족 모두 챙길수 있어", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                30, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁 때문에 집안 생계를 네가 책임져야 하는 상황이야.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("매일 장터에서 일하거나 물건을 팔아야 하는데, 한 끼를 겨우 먹을 정도라면 넌 어떻게 할래?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "열심히 일하며 버티기",
                                "다른 생존 방법을 찾기"
                            },
                            new int[] {39,40},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                39, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("열심히 일하면서 끝까지 버틸거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                40, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("다른 생존방법을 찾을거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                31, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린아, 전쟁 중 가족과 헤어져서 어디 있는지 알 수 없게 됐어.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("캠프를 돌아다니며 사람들에게 물어볼 수도 있지만, 다시 만날 가능성이 희박할 때 넌 어떤 선택을 할 것 같아?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "끝까지 찾기",
                                "가족이 올 때까지 기다리기",
                                "혼자 살아가는걸 다짐하기"
                            },
                            new int[] {41,42,43},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                41, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("가족을 끝까지 찾을거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                42, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("가족이 다시 올때까지 기다릴꺼야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                43, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("혼자 살아가는것을 다짐할거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                32, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린, 길을 가다 군인을 마주쳤어.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("어떤 군인은 음식을 줄 수도 있지만, 또 어떤 군인은 위협할 수도 있는 상황이라면 넌 어떻게 할래?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "숨기",
                                "거리를 유지하며 지켜보기",
                                "조심스럽게 접근하기"
                            },
                            new int[] {44,45,46},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                44, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("숨어서 기다릴래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                45, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("거리를 유지하고 무엇을 하는지 지켜볼거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                46, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("조심스럽게 접근할래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                33, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린아, 전쟁으로 학교가 문을 닫아 공부를 할 수 없게 됐어.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("책도 연필도 없는 상황에서 너라면 이 시간을 어떻게 보낼 것 같아?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "친구들과 공부 방법을 찾기",
                                "생존 기술을 배우기"
                            },
                            new int[] {47,48},
                            new int[] {1,3}
                        )
                    }
                )
            },
            {
                47, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("친구들과 공부 방법을 찾아 공부할거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                48, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("생존 기술을 배워 일단 살아남을거야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                34, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린아, 전쟁 때문에 부모님이 부재하거나 부상을 입어서 네가 동생들을 챙겨야 하는 상황이야.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("너도 어린 나이인데, 동생들을 먹이고 재우는 책임을 맡아야 한다면 어떻게 할래?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "동생들을 끝까지 지키기",
                                "도움을 받을 방법을 찾기",
                                "동생들에게도 역할을 나누기"
                            },
                            new int[] {49,50,51},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                49, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("내 동생들을 끝까지 지켜야지", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                50, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("도움을 받을 방법을 찾으러 갈꺼야", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                51, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("동생들에게 역할을 나누어 책임을 나눌래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                35, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린, 네 눈앞에서 전투가 벌어져 총성과 폭격 소리가 가득한 상황이야", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("집이 무너지고 길거리가 폐허가 됐을 때 넌 어떻게 행동할 것 같아?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "숨기",
                                "주변 사람들과 협력하기"
                            },
                            new int[] {52,53},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                52, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("당장 숨어야지", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                53, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("주변 사람들과 협력할래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                36, new DialogueData(
                    "고양이 가면을 쓴 소년",
                    new DialogueLine[] {
                        new DialogueLine("아린아, 전쟁으로 가족이나 친구를 잃게 됐어.", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("매일 악옴을 꾸고 작은 소리에도 놀라게 되는 상황에서 평범한 삶을 되찾기 위해 넌 어떤 선택을 할래?", "고양이 가면을 쓴 소년", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "상처를 마주하고 받아들이기",
                                "새로운 사람을 만나기",
                                "새로운 곳으로 떠나기"
                            },
                            new int[] {54,55,56},
                            new int[] {3,1,2}
                        )
                    }
                )
            },
            {
                54, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("상처를 마주하고 받아들여 이겨낼래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                55, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("새로운 사람을 만날래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                56, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("새로운 곳으로 떠날래", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                57, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁 중에 가족을 잃은 사람들 많잖아", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그래서 가족을 다시 찾는 일은 정말 중요한 일이었지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그러면, 아린은 가족을 찾기위해 어디부터 시작할 거야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "부모님 고향으로 가기",
                                "친척을 찾아보기"
                            },
                            new int[] {65,66},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                65, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("부모님의 고향으로 갈꺼야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                66, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("친척을 찾아볼꺼야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
            58, new DialogueData(
                "고양이 가면을 쓴 남자",
                new DialogueLine[] {
                        new DialogueLine("아린, 전쟁을 겪고 나면, 정신적으로 고통을 안고 살아가는 사람들이 많아.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그 당시에는 제대로 된 치료를 받기 힘들었겠지만, 그래도 조금씩 국복하려는 사람들이 있었지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("아린은 어떻게 그 고통을 이겨내려고 할래?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "일상으로 돌아가기",
                                "전문적인 상담을 받기",
                                "친구들과 이야기하기"
                            },
                            new int[] {67,68,69},
                            new int[] {1,3,2}
                        )
                    }
                )
            },
            {
                67, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("일상으로 돌아갈래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                68, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("전문적인 상담을 받아 치료할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                69, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("친구들과 이야기하며 해소할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                59, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁 중 다친 사람들은 정말 힘들었지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("치료를 제대로 받기도 어려운 상황에서 고통을 견뎌야 했으니까.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("부상 치료를 어떻게 할까?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "민간 자원을 찾기",
                                "집에서 치료하기"
                            },
                            new int[] {70,71},
                            new int[] {3,2}
                        )
                    }
                )
            },
            {
                70, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("민간 자원을 찾으러 갈래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                71, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("집에서 스스로 치료할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                60, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 피난을 떠난 사람들은 전쟁 후 새로운 땅에서 삶을 다시 시작해야 했지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("낮선 환경에서 살아가는 것도 쉽지 않았을 거야.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그럼, 아린은 새로운 곳에서 어떻게 시작할 거야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "일자리를 찾기",
                                "사람들과 친해지기",
                                "피난민 캠프에 참여하기"
                            },
                            new int[] {72,73,74},
                            new int[] {1,2,3}
                        )
                    }
                )
            },
            {
                72, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("일자리를 찾으러 가야지", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                73, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("사람들과 친해질래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                74, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("피난민 캠프레 참여할꺼야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                61, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁을 겪은 사람들은 그 기억을 쉽게 잊지 못하잖아?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("다시 평화로운 일상으로 돌아가는 게 얼마나 힘들었을까.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("아린은 그 기억을 어떻게 극복할 생각이야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "새로운 목표를 세우기",
                                "가족과 시간을 보내기",
                                "그 경험을 기록하기"
                            },
                            new int[] {75,76,77},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                75, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("새로운 목표를 세우고 실천할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                76, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("가족과 시간을 보낼꺼야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                77, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("그 경험을 기록할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                62, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁 후 경제 상황은 정말 어려웠지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("사람들은 각자 생계를 위해서 다양한 방법으로 재건에 힘썼어.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그럼, 아린은 어떻게 경제적 재건을 시작할 생각이야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "대도시로 이주하기",
                                "농업에 종사하기",
                                "작은 사업을 시작하기"
                            },
                            new int[] {78,79,80},
                            new int[] {3,1,2}
                        )
                    }
                )
            },
            {
                78, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("대도시로 이주할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                79, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("농업에 종사할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                80, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("작은 사업을 시작할꺼야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                63, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁 후, 사람들은 서로 도우며 살아가려고 했지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("고난을 함께 나누는 공동체 정신이 강해졌고, 사회를 복구하는 데 많은 사람들이 힘을 보탰어.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("아린은 그 일에 어떻게 협력할 생각이야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "이웃과 힘을 합치기",
                                "복구 활동에 참여하기"
                            },
                            new int[] {81,82},
                            new int[] {2,3}
                        )
                    }
                )
            },
            {
                81, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("이웃과 힘을 합쳐야지", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                82, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("복구활동에 참여할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                64, new DialogueData(
                    "고양이 가면을 쓴 남자",
                    new DialogueLine[] {
                        new DialogueLine("아린, 전쟁의 상처를 극복하고 나면, 새로운 희망을 찾아가야 하지.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그때는 미래에 대한 불안도 컸지만, 희망을 품고 사는 사람들도 많았어.", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("이린은 어떻게 희망을 찾을 거야?", "고양이 가면을 쓴 남자", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("그런 상황 속에 있었다면 나는...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "새로운 기회를 찾기",
                                "사회에 기여하기"
                            },
                            new int[] {83,84},
                            new int[] {2,3}
                        )
                    }
                )
            },
            {
                83, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("새로운 기회를 찾을거야", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                84, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("사회에 기여할래", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                85, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("할머니! 할머니! 오늘도 옛날이야기 해주세요!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("아이구 우리 똥강아지, 할미가 해주는 옛날 이야기가 그렇게 좋아?", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("네! 해주세요", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("허허..할미가 말이야 딱 우리 아름이 만할 때 옆나라 일본에서 왔단다", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("처음에는 많이 무서웠어. 길거리를 걸을때도 조심조심 걸었단다.", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그러다가 몸에서 연기가 나는 하얀 호랑이가 휙! 지나가는거야!", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("호기심 많았던 할미는 호랑이를 쫓아갔단다. 그 뒤로..", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("'어..? 뭐가 지나갔어! 연기가 났었는데'", "아름이", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("'자세히 보고싶어!'", "아름이", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("(호랑이를 쫓아 숲으로 들어간다)", "아름이", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        
                    }
                )
            },
            {
                86, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("아린아! 아린아! 뭐해? 그동안 뭐하고 지냈어? 엄청 오랜만이다!", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("아린..? 그게 누구야? 나는 아름이야!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("무슨소리야? 아름이는 누구야? 너는 아린이잖아", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("아름이가 나야!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("너는 아린이잖아! 에이.. 어쨌든 호랑이 쫓아가는거지?", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("내가 도와줄께!", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("왜..?", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("그야 아린이는 나의 친구니까!", "고양이 가면을 쓴 남자아이", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("아름이라니까.. 쨌든 고마워!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                    }
                )
            },
            {
                87, new DialogueData(
                    "아름이",
                    new DialogueLine[] {
                        new DialogueLine("할머니! 할머니!!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("우리 강아지 무슨일이야?", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("저! 연기나는 하얀 호랑이를 봤어요! 그리고 신기한데 갔어요! 그리고 거기서 고양이 가면을 쓴 친구도 만났는데..", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("응응", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("근데 그 친구가 아린이라 불었어요! 난 아름이인데!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그치 우리 강아지는 아름이지~", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그리고 또 여러 장소에 갔어요! 막 무섭지만.. 신기했어요!!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그랬구나~", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("근데 그 장소들이 하나같이 할머니가 해준 옛날 이야기랑 똑같았어요! 내가 할머니가 됐었어요!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그거참 신기한 일이구나", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("네! 그리고 그리고..", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("(꼬르륵..)", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("할머니..나 배고파요..", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("허허..그래그래, 일단 할미가 저녁해놨으니 어서 먹자", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("그 다음에 더 이야기 해주련?", "할머니", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("네!", "아름이", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),

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
        npcImage22.sprite = line.npcImage2;
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
        StartObj.SetActive(true);
    }

    // 대화 UI 초기화
    private void InitializeDialogueUI(DialogueData dialogueData)
    {
        // 일반 대화창과 선택지 대화창 모두 숨김
        HideAllDialoguePanels();

        npcNameTextNormal.text = dialogueData.npcName;
        npcImageNormal.sprite = dialogueData.npcImage;
        npcImage22.sprite = dialogueData.npcImage2;

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
    public Sprite npcImage2;
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
    public Sprite npcImage2;
    public bool hasChoices;
    public string[] choices;
    public int[] nextDialogueIds;
    public int[] scores; // 점수 배열 추가

    public DialogueLine(string text, string npcName, Sprite npcImage, Sprite npcImage2, bool hasChoices = false,
        string[] choices = null, int[] nextDialogueIds = null, int[] scores = null)
    {
        this.text = text;
        this.npcName = npcName;
        this.npcImage = npcImage;
        this.npcImage2 = npcImage2;
        this.hasChoices = hasChoices;
        this.choices = choices;
        this.nextDialogueIds = nextDialogueIds;
        this.scores = scores; // 점수 배열 초기화
    }
}


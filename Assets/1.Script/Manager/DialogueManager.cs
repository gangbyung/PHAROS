using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static DialogueManager instance;

    // �Ϲ� ��ȭ UI ���
    public GameObject normalDialoguePanel; // �Ϲ� ��ȭâ �г�
    public TextMeshProUGUI npcNameTextNormal; // �Ϲ� ��ȭâ�� NPC �̸� �ؽ�Ʈ
    public TextMeshProUGUI dialogueTextNormal; // �Ϲ� ��ȭâ�� ��� �ؽ�Ʈ
    public Image npcImageNormal; // �Ϲ� ��ȭâ�� NPC �̹���
    public Image npcImage22;

    // ������ ��ȭ UI ���
    public GameObject choiceDialoguePanel; // ������ ��ȭâ �г�
    public TextMeshProUGUI npcNameTextChoice; // ������ ��ȭâ�� NPC �̸� �ؽ�Ʈ
    public TextMeshProUGUI dialogueTextChoice; // ������ ��ȭâ�� ��� �ؽ�Ʈ
    public Image npcImageChoice; // ������ ��ȭâ�� NPC �̹���

    public GameObject choiceButtonPrefab; // ������ ��ư ������
    public Transform choiceContainer; // ������ ��ư�� ���� Scroll View�� Content

    // ��ȭ ������
    private Dictionary<int, DialogueData> dialogues; // ��ȭ �����͸� ��� �ִ� ��ųʸ�
    private int currentDialogueId = 0; // ���� ��ȭ ID
    private int currentLineIndex = 0; // ���� ��� �� �ε���

    private bool isDialogueActive = false; // ��ȭ Ȱ��ȭ ����
    private bool waitingForChoice = false; // ������ ��� ���� ����


    public NPCDialogueTrigger nPCDialogueTrigger;

    public GameObject StartObj;
    // ���� ����
    public int score;

    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            instance = this; // �ν��Ͻ��� ���� ��ü�� ����
        }
    }
    void Start()
    {
        nPCDialogueTrigger = GetComponent<NPCDialogueTrigger>();
        // ��ȭ ������ �ʱ�ȭ
        dialogues = new Dictionary<int, DialogueData>
        {
            {
                
                0, new DialogueData( 
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("����, �Ƹ��� �� Ÿ�� �ѱ��� ó�� �����ߴ� �� ��ﳪ?", "����� ������ �� ����", Resources.Load<Sprite>("b1"),Resources.Load<Sprite>("a2")),
                        new DialogueLine("�׶� �Ϻ��̶� �޶� ���� �ű�����!", "����� ������ �� ����", Resources.Load<Sprite>("b1"),Resources.Load<Sprite>("a2")),
                        new DialogueLine("�θ�Ե� �ѱ� �� ���ڸ��� ��û �����ϼ��ݾ�?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�Ƹ��̵� �׶� '���Ⱑ ��¥ �� �����ΰ�?' �ϰ� ���� ������ �� ������, ����?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"),Resources.Load<Sprite>("a2"), true,
                            new string[] {
                                "���ο� ȯ���� Ž���ϱ�� ����ϱ�",
                                "���� ȯ�濡 �θ�� ���� ������ �ʱ�"
                            },
                            new int[] {8,9},
                            new int[] {3,1}
                        )
                    }
                )
            },
            {
                1, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̴� �Ϻ����� �¾�� �׷��� �ѱ��� ���� �� �� ���������?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�ѱ��� ���ڸ��� ���� ���̵��̶� ��� �;��� �ٵ�, ���� �� ���ؼ� ������� �ž�", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�θ���̶� �ҸӴϰ� ������ �ѱ��� �������ֱ� ������, �׷��� �Ƹ��̴� ������ �Ϻ�� �� ������ ���ݾ�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "�� ������ �ѱ�� ����� ����ϱ�",
                                "�Ϻ���� �ѱ�� ����� ��ȭ�ϱ�",
                                "�����ؼ� �ƹ� ���� ���� �ʱ�"
                            },
                            new int[] {10,11,12},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                2, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̴� �Ϻ����� �Դ� ������ ���� �׸����� �ž�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�ѱ� ���� ���ĵ��� �� ������ ���� �����ݾ�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "�ѱ� ������ ���ο� ���� ����",
                                "�θ���� �Ϻ� ������ ������ ���� �ޱ�",
                                "���� �۾����� �޴��� ���ĸ� �Դ� ����ϱ�"
                            },
                            new int[] {13,14,15},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                3, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�ʰ� ���� �Ƴ��� �Ϻ��� å��, �ѱ����� �������ݾ�?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�ٵ� �ѱ������� �Ϻ��� å ���� �� ���� ���� �ʴٴ� ��⸦ �����.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷��� �θ���� ������ �Ϻ��� å���� ġ��� �ѱ��� å�� ������� �ߴµ�, �ʰ� �󸶳� �ƽ����ھ�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "�ѱ��� å������ ��̸� ã��",
                                "�Ϻ��� å���� �߾��� ������ �����ϱ�",
                                "���� �۾����� å ������ ���� ����ϱ�"
                            },
                            new int[] {16,17,18},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                4, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�ʰ� �ѱ� �б��� ó�� �� ��, ��¥ ������ �������� �ž�, �� �׷�?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("���ǿ��� �ڱ�Ұ��� �� �Ϻ��� �̸��� ���ߴ���, �������̶� ģ������ ���� ��Ȳ�� ��ġ����.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷��� �ʴ� �θ���̶� �ѱ��� �̸��� ����ϱ� ��������.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "�ѱ��� �̸��� ���ϱ�",
                                "����ؼ� �Ϻ��� �̸��� ����ϱ�",
                                "�ڽ��� �۾����� ������, �ҽ��ϰ� �ൿ�ϱ�"
                            },
                            new int[] {19,20,21},
                            new int[] {3,2,1}
                        )
                    }
                )
            },
            {
                5, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("��, �Ƹ��̴� �Ϻ����� �¾�ŵ�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�ٵ� �ֺ� ������� �ٵ� �Ƹ��̸� �ѱ��� �̶�� ������.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("��, �Ƹ��̴� ���� �Ű� �� ���� �� ���� �ѵ�, �׷��� ������ �ڱⰡ �� �̹���ó�� �������� ��.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "�ѱ� ��ȭ�� �� ���� �����ϱ�",
                                "�ڽ��� �Ϻ� ��ȭ�� ������ �����"
                            },
                            new int[] {22,23},
                            new int[] {3,1}
                        )
                    }
                )
            },
            {
                6, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̳� ������ �ѱ����� ���ƿ��� ��, �ֺ��� ���� �������ϰ� ���� ������� �ִ���.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("'�� �Ϻ����� ��ҳ�'�� ���� ���԰�, �Ƹ��� �θ���� �װ� �����ϴ��� �� �������.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�Ƹ��̵� �� �ü� ������ �� �������?", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "ģ���ϰ� �ൿ�ϱ�",
                                "�� ����ϰ� ����",
                                "�� �ȿ� Ʋ�������"
                            },
                            new int[] {24,25,26},
                            new int[] {3,2,0}
                        )
                    }
                )
            },
            {
                7, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�ð��� �����鼭 �Ƹ��̴� �ѱ����� �� �ҳ�� ģ������.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�� ģ���� �Ƹ��̰� �Ϻ����� �Դٰ� �ؼ� ���ٸ� ��ߵ� ������, �׳� �ڿ������� �ٰ�����ŵ�.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷��� ���� ������ ��ȭ�� �����ϸ鼭 ������ �׾ư���.", "����� ������ �� ����", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a2"), Resources.Load < Sprite >("a2"), true,
                            new string[] {
                                "ģ���� �������� �ѱ� ��Ȱ�� �����ϱ�",
                                "������ ������ �� ���������"
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
                        new DialogueLine("�׷� �ѹ� Ž���� ����!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1"))
                    }
                )

            },
            {
                9, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�� �θ�԰� �Բ� ������..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),
                        
                    }
                )
            },
            {
                10, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�� ������ �����ؾ���!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                11, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ �� �̼������� ����ؾ���", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                12, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�����ؼ� �ƹ����� ���ҰŰ���..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                13, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���ο� ���� �����!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                14, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�θ���� ������ ������ ���ι޾ƾ߰ھ�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                15, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� �۾��� �޴��� ���ĸ� �԰�;�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                16, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ��� å���� ��̸� ã��!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                17, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�Ϻ��� å���� �߾��� ������ �����ҷ�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                18, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� �۾����� å������ ����;�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                19, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ��� �̸��� ���غ���!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                20, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("��� �Ϻ��� �̸��� ����ҷ�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                21, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ҽ��ϰ� �ൿ�ҷ�..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                22, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ� ��ȭ�� ������ �����ҷ�!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                23, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� �Ϻ� ��ȭ�� ������ ���淡", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                24, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("ģ���ϰ� �ൿ�ؾ���!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                25, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�� ����ϰ� ����!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                26, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�� �ȿ� Ʋ�������..", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                27, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("ģ���� ������ �޾� �ѱ� ��Ȱ�� �����ؾ߰ھ�", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                28, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ ������ �� ���������", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a1")),

                    }
                )
            },
            {
                29, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���, ������ ������ �װ� ������ ���� ���� �ǳ��濡 �ö���.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�㳷���� ����� �Ȱ�, ���� �Ҹ��� ��� �鸮�µ�, ������ ��Ű�鼭�� ���� ì�ܾ� �Ѵٸ� ��� �� �� ����?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "���� ������",
                                "���� ���� ��� ì���"
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
                        new DialogueLine("���� ������ ������ ��ų��", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                38, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� ���� ��� ì��� �־�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                30, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� ������ ���� ���踦 �װ� å������ �ϴ� ��Ȳ�̾�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("���� ���Ϳ��� ���ϰų� ������ �Ⱦƾ� �ϴµ�, �� ���� �ܿ� ���� ������� �� ��� �ҷ�?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "������ ���ϸ� ��Ƽ��",
                                "�ٸ� ���� ����� ã��"
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
                        new DialogueLine("������ ���ϸ鼭 ������ ��ƿ�ž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                40, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ٸ� ��������� ã���ž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                31, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���, ���� �� ������ ������� ��� �ִ��� �� �� ���� �ƾ�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("ķ���� ���ƴٴϸ� ����鿡�� ��� ���� ������, �ٽ� ���� ���ɼ��� ����� �� �� � ������ �� �� ����?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "������ ã��",
                                "������ �� ������ ��ٸ���",
                                "ȥ�� ��ư��°� �����ϱ�"
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
                        new DialogueLine("������ ������ ã���ž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                42, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ �ٽ� �ö����� ��ٸ�����", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                43, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("ȥ�� ��ư��°��� �����Ұž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                32, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� ���� ������ �����ƾ�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("� ������ ������ �� ���� ������, �� � ������ ������ ���� �ִ� ��Ȳ�̶�� �� ��� �ҷ�?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "����",
                                "�Ÿ��� �����ϸ� ���Ѻ���",
                                "���ɽ����� �����ϱ�"
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
                        new DialogueLine("��� ��ٸ���", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                45, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�Ÿ��� �����ϰ� ������ �ϴ��� ���Ѻ��ž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                46, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���ɽ����� �����ҷ�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                33, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���, �������� �б��� ���� �ݾ� ���θ� �� �� ���� �ƾ�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("å�� ���ʵ� ���� ��Ȳ���� �ʶ�� �� �ð��� ��� ���� �� ����?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "ģ����� ���� ����� ã��",
                                "���� ����� ����"
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
                        new DialogueLine("ģ����� ���� ����� ã�� �����Ұž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                48, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� ����� ��� �ϴ� ��Ƴ����ž�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                34, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���, ���� ������ �θ���� �����ϰų� �λ��� �Ծ �װ� �������� ì�ܾ� �ϴ� ��Ȳ�̾�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�ʵ� � �����ε�, �������� ���̰� ���� å���� �þƾ� �Ѵٸ� ��� �ҷ�?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "�������� ������ ��Ű��",
                                "������ ���� ����� ã��",
                                "�����鿡�Ե� ������ ������"
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
                        new DialogueLine("�� �������� ������ ���Ѿ���", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                50, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ ���� ����� ã���� ������", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                51, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�����鿡�� ������ ������ å���� ������", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                35, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, �� ���տ��� ������ ������ �Ѽ��� ���� �Ҹ��� ������ ��Ȳ�̾�", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("���� �������� ��Ÿ��� ���㰡 ���� �� �� ��� �ൿ�� �� ����?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "����",
                                "�ֺ� ������ �����ϱ�"
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
                        new DialogueLine("���� �������", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                53, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ֺ� ������ �����ҷ�", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                36, new DialogueData(
                    "����� ������ �� �ҳ�",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���, �������� �����̳� ģ���� �Ұ� �ƾ�.", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("���� �ǿ��� �ٰ� ���� �Ҹ����� ���� �Ǵ� ��Ȳ���� ����� ���� ��ã�� ���� �� � ������ �ҷ�?", "����� ������ �� �ҳ�", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a3")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a3"), Resources.Load < Sprite >("a3"), true,
                            new string[] {
                                "��ó�� �����ϰ� �޾Ƶ��̱�",
                                "���ο� ����� ������",
                                "���ο� ������ ������"
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
                        new DialogueLine("��ó�� �����ϰ� �޾Ƶ鿩 �̰ܳ���", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                55, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���ο� ����� ������", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                56, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���ο� ������ ������", "???", Resources.Load<Sprite>("b2"), Resources.Load < Sprite >("a4")),

                    }
                )
            },
            {
                57, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� �߿� ������ ���� ����� ���ݾ�", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷��� ������ �ٽ� ã�� ���� ���� �߿��� ���̾���.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷���, �Ƹ��� ������ ã������ ������ ������ �ž�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "�θ�� �������� ����",
                                "ģô�� ã�ƺ���"
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
                        new DialogueLine("�θ���� �������� ������", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                66, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("ģô�� ã�ƺ�����", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
            58, new DialogueData(
                "����� ������ �� ����",
                new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ������ �ް� ����, ���������� ������ �Ȱ� ��ư��� ������� ����.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�� ��ÿ��� ����� �� ġ�Ḧ �ޱ� �����������, �׷��� ���ݾ� �����Ϸ��� ������� �־���.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�Ƹ��� ��� �� ������ �̰ܳ����� �ҷ�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "�ϻ����� ���ư���",
                                "�������� ����� �ޱ�",
                                "ģ����� �̾߱��ϱ�"
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
                        new DialogueLine("�ϻ����� ���ư���", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                68, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�������� ����� �޾� ġ���ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                69, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("ģ����� �̾߱��ϸ� �ؼ��ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                59, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� �� ��ģ ������� ���� �������.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("ġ�Ḧ ����� �ޱ⵵ ����� ��Ȳ���� ������ �ߵ��� �����ϱ�.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�λ� ġ�Ḧ ��� �ұ�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "�ΰ� �ڿ��� ã��",
                                "������ ġ���ϱ�"
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
                        new DialogueLine("�ΰ� �ڿ��� ã���� ����", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                71, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ ������ ġ���ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                60, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, �ǳ��� ���� ������� ���� �� ���ο� ������ ���� �ٽ� �����ؾ� ����.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("���� ȯ�濡�� ��ư��� �͵� ���� �ʾ��� �ž�.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷�, �Ƹ��� ���ο� ������ ��� ������ �ž�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "���ڸ��� ã��",
                                "������ ģ������",
                                "�ǳ��� ķ���� �����ϱ�"
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
                        new DialogueLine("���ڸ��� ã���� ������", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                73, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ ģ������", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                74, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�ǳ��� ķ���� �����Ҳ���", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                61, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ������ ���� ������� �� ����� ���� ���� �����ݾ�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�ٽ� ��ȭ�ο� �ϻ����� ���ư��� �� �󸶳� ���������.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�Ƹ��� �� ����� ��� �غ��� �����̾�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "���ο� ��ǥ�� �����",
                                "������ �ð��� ������",
                                "�� ������ ����ϱ�"
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
                        new DialogueLine("���ο� ��ǥ�� ����� ��õ�ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                76, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("������ �ð��� ��������", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                77, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�� ������ ����ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                62, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� �� ���� ��Ȳ�� ���� �������.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("������� ���� ���踦 ���ؼ� �پ��� ������� ��ǿ� �����.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷�, �Ƹ��� ��� ������ ����� ������ �����̾�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "�뵵�÷� �����ϱ�",
                                "����� �����ϱ�",
                                "���� ����� �����ϱ�"
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
                        new DialogueLine("�뵵�÷� �����ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                79, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("����� �����ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                80, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("���� ����� �����Ҳ���", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                63, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ���� ��, ������� ���� ����� ��ư����� ����.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("���� �Բ� ������ ����ü ������ ��������, ��ȸ�� �����ϴ� �� ���� ������� ���� ���ɾ�.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�Ƹ��� �� �Ͽ� ��� ������ �����̾�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "�̿��� ���� ��ġ��",
                                "���� Ȱ���� �����ϱ�"
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
                        new DialogueLine("�̿��� ���� ���ľ���", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                82, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("����Ȱ���� �����ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                64, new DialogueData(
                    "����� ������ �� ����",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ�, ������ ��ó�� �غ��ϰ� ����, ���ο� ����� ã�ư��� ����.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׶��� �̷��� ���� �Ҿȵ� ������, ����� ǰ�� ��� ����鵵 ���Ҿ�.", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�̸��� ��� ����� ã�� �ž�?", "����� ������ �� ����", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a5")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "???", Resources.Load<Sprite>("a5"), Resources.Load < Sprite >("a5"), true,
                            new string[] {
                                "���ο� ��ȸ�� ã��",
                                "��ȸ�� �⿩�ϱ�"
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
                        new DialogueLine("���ο� ��ȸ�� ã���ž�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                84, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("��ȸ�� �⿩�ҷ�", "???", Resources.Load<Sprite>("b3"), Resources.Load < Sprite >("a6")),

                    }
                )
            },
            {
                85, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ҸӴ�! �ҸӴ�! ���õ� �����̾߱� ���ּ���!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("���̱� �츮 �˰�����, �ҹ̰� ���ִ� ���� �̾߱Ⱑ �׷��� ����?", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("��! ���ּ���", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("����..�ҹ̰� ���̾� �� �츮 �Ƹ��� ���� �� ������ �Ϻ����� �Դܴ�", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("ó������ ���� ��������. ��Ÿ��� �������� �������� �ɾ��ܴ�.", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�׷��ٰ� ������ ���Ⱑ ���� �Ͼ� ȣ���̰� ��! �������°ž�!", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("ȣ��� ���Ҵ� �ҹ̴� ȣ���̸� �Ѿư��ܴ�. �� �ڷ�..", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("'��..? ���� ��������! ���Ⱑ �����µ�'", "�Ƹ���", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("'�ڼ��� ����;�!'", "�Ƹ���", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("(ȣ���̸� �Ѿ� ������ ����)", "�Ƹ���", Resources.Load<Sprite>("1"), Resources.Load < Sprite >("aa")),
                        
                    }
                )
            },
            {
                86, new DialogueData(
                    "???",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ���! �Ƹ���! ����? �׵��� ���ϰ� ���¾�? ��û �������̴�!", "����� ������ �� ���ھ���", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�Ƹ�..? �װ� ������? ���� �Ƹ��̾�!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�����Ҹ���? �Ƹ��̴� ������? �ʴ� �Ƹ����ݾ�", "����� ������ �� ���ھ���", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�Ƹ��̰� ����!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�ʴ� �Ƹ����ݾ�! ����.. ��·�� ȣ���� �Ѿư��°���?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("���� �����ٲ�!", "����� ������ �� ���ھ���", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("��..?", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�׾� �Ƹ��̴� ���� ģ���ϱ�!", "����� ������ �� ���ھ���", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                        new DialogueLine("�Ƹ��̶�ϱ�.. ·�� ����!", "???", Resources.Load<Sprite>("b1"), Resources.Load < Sprite >("a2")),
                    }
                )
            },
            {
                87, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ҸӴ�! �ҸӴ�!!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�츮 ������ �������̾�?", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("��! ���⳪�� �Ͼ� ȣ���̸� �þ��! �׸��� �ű��ѵ� �����! �׸��� �ű⼭ ����� ������ �� ģ���� �����µ�..", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("����", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�ٵ� �� ģ���� �Ƹ��̶� �Ҿ����! �� �Ƹ����ε�!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("��ġ �츮 �������� �Ƹ�����~", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�׸��� �� ���� ��ҿ� �����! �� ��������.. �ű��߾��!!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�׷�����~", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�ٵ� �� ��ҵ��� �ϳ����� �ҸӴϰ� ���� ���� �̾߱�� �Ȱ��Ҿ��! ���� �ҸӴϰ� �ƾ����!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�װ��� �ű��� ���̱���", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("��! �׸��� �׸���..", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("(������..)", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�ҸӴ�..�� ����Ŀ�..", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("����..�׷��׷�, �ϴ� �ҹ̰� �����س����� � ����", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("�� ������ �� �̾߱� ���ַ�?", "�ҸӴ�", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),
                        new DialogueLine("��!", "�Ƹ���", Resources.Load<Sprite>("hh"), Resources.Load < Sprite >("aa")),

                    }
                )
            },
            // �߰� ��ȭ ������...
        };

        // �ʱ⿡�� ��� ��ȭâ�� ����
        HideAllDialoguePanels();

        // ���� �ʱ�ȭ
        score = 0;
    }

    void Update()
    {
        // ���콺 Ŭ�� �� ���� ���� ����
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            // �������� ��ٸ��� ���̶�� �������� ����
            if (!waitingForChoice)
            {
                ShowNextLine();
            }
        }
    }

    // Ư�� ��ȭ ID�� ��ȭ�� ����
    public void ShowDialogue(int dialogueId)
    {
        currentDialogueId = dialogueId; // ���� ��ȭ ID ����
        currentLineIndex = 0; // ��� �� �ε��� �ʱ�ȭ
        isDialogueActive = true; // ��ȭ Ȱ��ȭ ���� ����

        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            // ��ȭ UI �ʱ�ȭ
            InitializeDialogueUI(dialogueData);

            // ù ��° ��� �� ǥ��
            ShowNextLine();
        }
    }

    // ���� ��� ���� ǥ��
    public void ShowNextLine()
    {
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            if (currentLineIndex < dialogueData.lines.Length)
            {
                DialogueLine line = dialogueData.lines[currentLineIndex];

                if (line.hasChoices && line.choices != null)
                {
                    // ������ ��ȭ UI�� ��ȯ
                    ShowChoiceDialogue(line);
                }
                else
                {
                    // �Ϲ� ��ȭ UI�� ��ȯ
                    ShowNormalDialogue(line);
                    currentLineIndex++; // ���� ��� �ٷ� ����
                }
            }
            else
            {
                // �� �̻� ��簡 ������ ��ȭ ����
                EndDialogue();
            }
        }
    }

    // �Ϲ� ��ȭ UI ������Ʈ
    private void ShowNormalDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(true);
        choiceDialoguePanel.SetActive(false);

        dialogueTextNormal.text = line.text;
        npcNameTextNormal.text = line.npcName;
        npcImageNormal.sprite = line.npcImage;
        npcImage22.sprite = line.npcImage2;
    }

    // ������ ��ȭ UI ������Ʈ
    // ������ ��ȭ UI ������Ʈ
    private void ShowChoiceDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(true);

        dialogueTextChoice.text = line.text;
        npcNameTextChoice.text = line.npcName;
        npcImageChoice.sprite = line.npcImage;

        // ���� ������ ��ư ����
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // ������ ��ư ���� �� �ִϸ��̼� ����
        // ������ ��ư ���� �� �ִϸ��̼� ����
        for (int i = 0; i < line.choices.Length; i++)
        {
            GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[i];

            // ��ư�� �ִ� Image ������Ʈ�� ã�Ƽ� Animator�� ã��
            Image buttonImage = choiceButton.GetComponentInChildren<Image>();  // Image ������Ʈ ��������
            Animator animator = choiceButton.GetComponentInChildren<Animator>();  // Animator ������Ʈ ��������

            // ��ư�� Ŭ������ �� ó��
            int choiceIndex = i; // Closure ���� ����
            choiceButton.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        }



        waitingForChoice = true; // �������� ��ٸ��� ���·� ����
    }


    // �������� ���õǾ��� �� ó��
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"�÷��̾ ������ �߽��ϴ�: {choiceIndex}");

        waitingForChoice = false; // ������ ��� ���� ����

        // ���� ��ȭ ID���� ��ȭ �����͸� ��������
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            if (line.hasChoices && line.nextDialogueIds != null && choiceIndex < line.nextDialogueIds.Length)
            {
                // ���� ������Ʈ
                UpdateScore(choiceIndex);

                int nextDialogueId = line.nextDialogueIds[choiceIndex];
                currentDialogueId = nextDialogueId;
                currentLineIndex = 0; // ���� ��ȭ�� ù ��° �ٷ� �ʱ�ȭ

                ShowNextLine(); // ���� ��縦 ������
            }
        }
    }


    // ���� ������Ʈ
    private void UpdateScore(int choiceIndex)
    {
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];

            if (line.scores != null && choiceIndex < line.scores.Length)
            {
                int pointsToAdd = line.scores[choiceIndex]; // �������� �ش��ϴ� ���� ��������
                score += pointsToAdd; // ���� ������Ʈ
                Debug.Log($"������: {choiceIndex}, �߰� ����: {pointsToAdd}, ���� ����: {score}"); // ����� �α�
            }
            else
            {
                Debug.LogWarning("�������� ���� ������ �����ϴ�.");
            }
        }
    }


    // ��ȭ ���� ó��
    private void EndDialogue()
    {
        isDialogueActive = false; // ��ȭ ��Ȱ��ȭ
        HideAllDialoguePanels(); // ��� ��ȭâ ����
        Debug.Log("��ȭ�� ����Ǿ����ϴ�.");
        StartObj.SetActive(true);
    }

    // ��ȭ UI �ʱ�ȭ
    private void InitializeDialogueUI(DialogueData dialogueData)
    {
        // �Ϲ� ��ȭâ�� ������ ��ȭâ ��� ����
        HideAllDialoguePanels();

        npcNameTextNormal.text = dialogueData.npcName;
        npcImageNormal.sprite = dialogueData.npcImage;
        npcImage22.sprite = dialogueData.npcImage2;

    }

    // ��� ��ȭâ ����
    private void HideAllDialoguePanels()
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(false);
    }

}

// ��ȭ ������ Ŭ����
[System.Serializable]
public class DialogueData
{
    public string npcName; // NPC �̸�
    public Sprite npcImage;
    public Sprite npcImage2;
    public DialogueLine[] lines; // ��� ��

    public DialogueData(string npcName, DialogueLine[] lines)
    {
        this.npcName = npcName;
        this.lines = lines;
    }
}

// ��� �� Ŭ����
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
    public int[] scores; // ���� �迭 �߰�

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
        this.scores = scores; // ���� �迭 �ʱ�ȭ
    }
}


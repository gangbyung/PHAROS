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


    // ���� ����
    private int score;

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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("����, �Ƹ��� �� Ÿ�� �ѱ��� ó�� �����ߴ� �� ��ﳪ?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׶� �Ϻ��̶� �޶� ���� �ű�����!", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�θ�Ե� �ѱ� �� ���ڸ��� ��û �����ϼ��ݾ�?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�Ƹ��̵� �׶� '���Ⱑ ��¥ �� �����ΰ�?' �ϰ� ���� ������ �� ������, ����?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̴� �Ϻ����� �¾�� �׷��� �ѱ��� ���� �� �� ���������?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�ѱ��� ���ڸ��� ���� ���̵��̶� ��� �;��� �ٵ�, ���� �� ���ؼ� ������� �ž�", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�θ���̶� �ҸӴϰ� ������ �ѱ��� �������ֱ� ������, �׷��� �Ƹ��̴� ������ �Ϻ�� �� ������ ���ݾ�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̴� �Ϻ����� �Դ� ������ ���� �׸����� �ž�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�ѱ� ���� ���ĵ��� �� ������ ���� �����ݾ�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�ʰ� ���� �Ƴ��� �Ϻ��� å��, �ѱ����� �������ݾ�?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�ٵ� �ѱ������� �Ϻ��� å ���� �� ���� ���� �ʴٴ� ��⸦ �����.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷��� �θ���� ������ �Ϻ��� å���� ġ��� �ѱ��� å�� ������� �ߴµ�, �ʰ� �󸶳� �ƽ����ھ�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�ʰ� �ѱ� �б��� ó�� �� ��, ��¥ ������ �������� �ž�, �� �׷�?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("���ǿ��� �ڱ�Ұ��� �� �Ϻ��� �̸��� ���ߴ���, �������̶� ģ������ ���� ��Ȳ�� ��ġ����.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷��� �ʴ� �θ���̶� �ѱ��� �̸��� ����ϱ� ��������.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("��, �Ƹ��̴� �Ϻ����� �¾�ŵ�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�ٵ� �ֺ� ������� �ٵ� �Ƹ��̸� �ѱ��� �̶�� ������.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("��, �Ƹ��̴� ���� �Ű� �� ���� �� ���� �ѵ�, �׷��� ������ �ڱⰡ �� �̹���ó�� �������� ��.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�Ƹ��̳� ������ �ѱ����� ���ƿ��� ��, �ֺ��� ���� �������ϰ� ���� ������� �ִ���.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("'�� �Ϻ����� ��ҳ�'�� ���� ���԰�, �Ƹ��� �θ���� �װ� �����ϴ��� �� �������.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�Ƹ��̵� �� �ü� ������ �� �������?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("�ð��� �����鼭 �Ƹ��̴� �ѱ����� �� �ҳ�� ģ������.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�� ģ���� �Ƹ��̰� �Ϻ����� �Դٰ� �ؼ� ���ٸ� ��ߵ� ������, �׳� �ڿ������� �ٰ�����ŵ�.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷��� ���� ������ ��ȭ�� �����ϸ鼭 ������ �׾ư���.", "����� ������ �� ���ھ���", Resources.Load<Sprite>("Npc2_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
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
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�׷� �ѹ� Ž���� ����!", "�Ƹ���", Resources.Load<Sprite>("npc1_image"))
                    }
                )

            },
            {
                9, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�� �θ�԰� �Բ� ������..", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),
                        
                    }
                )
            },
            {
                10, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�� ������ �����ؾ���!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                11, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("������ �� �̼������� ����ؾ���", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                12, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�����ؼ� �ƹ����� ���ҰŰ���..", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                13, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("���ο� ���� �����!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                14, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�θ���� ������ ������ ���ι޾ƾ߰ھ�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                15, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("���� �۾��� �޴��� ���ĸ� �԰�;�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                16, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ��� å���� ��̸� ã��!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                17, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�Ϻ��� å���� �߾��� ������ �����ҷ�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                18, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("���� �۾����� å������ ����;�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                19, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ��� �̸��� ���غ���!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                20, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("��� �Ϻ��� �̸��� ����ҷ�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                21, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ҽ��ϰ� �ൿ�ҷ�..", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                22, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�ѱ� ��ȭ�� ������ �����ҷ�!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                23, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("���� �Ϻ� ��ȭ�� ������ ���淡", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                24, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("ģ���ϰ� �ൿ�ؾ���!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                25, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�� ����ϰ� ����!", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                26, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�� �ȿ� Ʋ�������..", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                27, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("ģ���� ������ �޾� �ѱ� ��Ȱ�� �����ؾ߰ھ�", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

                    }
                )
            },
            {
                28, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("������ ������ �� ���������", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),

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
        nPCDialogueTrigger = FindObjectOfType<NPCDialogueTrigger>();
        nPCDialogueTrigger.DeleteMyParent();
    }

    // ��ȭ UI �ʱ�ȭ
    private void InitializeDialogueUI(DialogueData dialogueData)
    {
        // �Ϲ� ��ȭâ�� ������ ��ȭâ ��� ����
        HideAllDialoguePanels();

        npcNameTextNormal.text = dialogueData.npcName;
        npcImageNormal.sprite = dialogueData.npcImage;
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
    public bool hasChoices;
    public string[] choices;
    public int[] nextDialogueIds;
    public int[] scores; // ���� �迭 �߰�

    public DialogueLine(string text, string npcName, Sprite npcImage, bool hasChoices = false,
        string[] choices = null, int[] nextDialogueIds = null, int[] scores = null)
    {
        this.text = text;
        this.npcName = npcName;
        this.npcImage = npcImage;
        this.hasChoices = hasChoices;
        this.choices = choices;
        this.nextDialogueIds = nextDialogueIds;
        this.scores = scores; // ���� �迭 �ʱ�ȭ
    }
}


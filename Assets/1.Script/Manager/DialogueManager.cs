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

    public Button choiceButton1; // ù ��° ������ ��ư
    public Button choiceButton2; // �� ��° ������ ��ư
    public Button choiceButton3; // �� ��° ������ ��ư

    // ��ȭ ������
    private Dictionary<int, DialogueData> dialogues; // ��ȭ �����͸� ��� �ִ� ��ųʸ�
    private int currentDialogueId = 0; // ���� ��ȭ ID
    private int currentLineIndex = 0; // ���� ��� �� �ε���

    private bool isDialogueActive = false; // ��ȭ Ȱ��ȭ ����
    private bool waitingForChoice = false; // ������ ��� ���� ����

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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �������� �ʵ��� ����
        }
    }

    void Start()
    {
        // ��ȭ ������ �ʱ�ȭ
        dialogues = new Dictionary<int, DialogueData>
        {
            {
                0, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("�ȳ�! ���� ������ ����?", "npc1", Resources.Load<Sprite>("npc1_image"), true, new string[] { "1�� ����", "2�� ����", "3�� ����" }, new int[] { 1, 2, 3 })
                    }
                )
            },
            {
                1, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("1���̱���, ���� �����̾�!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                2, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("2���̱���, ������ �����̾�!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                3, new DialogueData(
                    "NPC1",
                    new DialogueLine[] {
                        new DialogueLine("3���� �����߱���, ���� �����̾�!", "npc1", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            }
        };

        // �ʱ⿡�� ��� ��ȭâ�� ����
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(false);
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);

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
            // �Ϲ� ��ȭ UI ������Ʈ
            npcNameTextNormal.text = dialogueData.npcName;
            dialogueTextNormal.text = "";
            npcImageNormal.sprite = null;

            // ������ ��ȭ UI ������Ʈ
            npcNameTextChoice.text = dialogueData.npcName;
            dialogueTextChoice.text = "";
            npcImageChoice.sprite = null;

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
                    // �Ϲ� ��ȭâ ����� ������ ��ȭâ ǥ��
                    normalDialoguePanel.SetActive(false);
                    choiceDialoguePanel.SetActive(true);

                    // ������ ��ȭ �ؽ�Ʈ ������Ʈ
                    dialogueTextChoice.text = line.text;

                    // ������ ��ȭâ�� NPC �̹��� ������Ʈ
                    npcImageChoice.sprite = line.npcImage;

                    // ������ ��ư�� �ؽ�Ʈ ����
                    choiceButton1.gameObject.SetActive(true);
                    choiceButton2.gameObject.SetActive(true);
                    choiceButton3.gameObject.SetActive(true);

                    choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[0];
                    choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[1];
                    choiceButton3.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[2];

                    // ������ ��ư�� ������ �߰�
                    choiceButton1.onClick.RemoveAllListeners();
                    choiceButton2.onClick.RemoveAllListeners();
                    choiceButton3.onClick.RemoveAllListeners();
                    choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
                    choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
                    choiceButton3.onClick.AddListener(() => OnChoiceSelected(2));

                    // �������� ��ٸ��� ���·� ����
                    waitingForChoice = true;
                }
                else
                {
                    // �Ϲ� ��ȭâ ǥ���ϰ� ������ ��ȭâ �����
                    normalDialoguePanel.SetActive(true);
                    choiceDialoguePanel.SetActive(false);

                    // �Ϲ� ��ȭ �ؽ�Ʈ ������Ʈ
                    dialogueTextNormal.text = line.text;

                    // �Ϲ� ��ȭâ�� NPC �̹��� ������Ʈ
                    npcImageNormal.sprite = line.npcImage;

                    // ������ ��ư �����
                    choiceButton1.gameObject.SetActive(false);
                    choiceButton2.gameObject.SetActive(false);
                    choiceButton3.gameObject.SetActive(false);

                    // ���� ��� �ٷ� ����
                    currentLineIndex++;
                }
            }
            else
            {
                // �� �̻� ��簡 ������ ��ȭ ����
                EndDialogue();
            }
        }
    }

    // �������� ���õǾ��� �� ó��
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"�÷��̾ ������ �߽��ϴ�: {choiceIndex}");

        // ������ ��ư �����
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);

        // ������ �Ϸ�Ǿ����Ƿ� ������ ��� ���� ����
        waitingForChoice = false;

        // ���ÿ� ���� ���� ��ȭ ID ���� �� ��� �� �ʱ�ȭ
        if (dialogues.TryGetValue(currentDialogueId, out DialogueData dialogueData))
        {
            DialogueLine line = dialogueData.lines[currentLineIndex];
            if (line.hasChoices && line.nextDialogueIds != null && choiceIndex < line.nextDialogueIds.Length)
            {
                int nextDialogueId = line.nextDialogueIds[choiceIndex];

                // �������� ���� ���� ����
                switch (choiceIndex)
                {
                    case 0: // ù ��° ������
                        score += 10; // ���� �߰� (��: 10��)
                        break;
                    case 1: // �� ��° ������
                        score += 20; // ���� �߰� (��: 20��)
                        break;
                    case 2: // �� ��° ������
                        score += 30; // ���� �߰� (��: 30��)
                        break;
                }
                Debug.Log($"���� ����: {score}");

                // ���� ��ȭ ID ����
                currentDialogueId = nextDialogueId;
                currentLineIndex = 0; // ���� ��ȭ�� ù ��° �ٷ� �ʱ�ȭ
                ShowNextLine(); // ���� ��縦 ������
            }
        }
    }

    // ��ȭ ���� ó��
    void EndDialogue()
    {
        isDialogueActive = false; // ��ȭ ��Ȱ��ȭ
        normalDialoguePanel.SetActive(false); // �Ϲ� ��ȭâ �����
        choiceDialoguePanel.SetActive(false); // ������ ��ȭâ �����
        Debug.Log("��ȭ�� ����Ǿ����ϴ�.");
    }

    // ��ȭ ������ ����ü
    [System.Serializable]
    public struct DialogueData
    {
        public string npcName; // NPC �̸�
        public DialogueLine[] lines; // ��� �� �迭

        public DialogueData(string npcName, DialogueLine[] lines)
        {
            this.npcName = npcName;
            this.lines = lines;
        }
    }

    // ��� �� ����ü
    [System.Serializable]
    public struct DialogueLine
    {
        public string text; // ��� �ؽ�Ʈ
        public string npcName; // NPC �̸�
        public Sprite npcImage; // NPC �̹���
        public bool hasChoices; // �������� �ִ��� ����
        public string[] choices; // ������ �迭
        public int[] nextDialogueIds; // ���� ��ȭ ID �迭

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

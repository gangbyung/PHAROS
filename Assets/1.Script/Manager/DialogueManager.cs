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
                    "����� ������ �� ���ھ���",
                    new DialogueLine[] {
                        new DialogueLine("����, �Ƹ��� �� Ÿ�� �ѱ��� ó�� �����ߴ� �� ��ﳪ?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("�׶� �Ϻ��̶� �޶� ���� �ű�����!", "����� ������ �� ���ھ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("�θ�Ե� �ѱ� �� ���ڸ��� ��û �����ϼ��ݾ�?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("�Ƹ��̵� �׶� '���Ⱑ ��¥ �� �����ΰ�?' �ϰ� ���� ������ �� ������, ����?", "����� ������ �� ���ھ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("�׷� ��Ȳ �ӿ� �־��ٸ� ����...", "�Ƹ����� ����� �� �Ƹ���", Resources.Load<Sprite>("npc1_image"), true,
                            new string[] {
                                "1�� ����: ���Ÿ� �ǵ��ƺ��� ������ ����Ѵ�",
                                "2�� ����: �̼� ������ ���������� ����Ѵ�",
                                "3�� ����: ������ ���� �����δ�"
                            },
                            new int[] { 1, 2, 3 },
                            new int[] {50,20,30}
                        )
                    }
                )
            },
            {
                1, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�׷�... �׶��� ����� �׻� ������ Ư����. ����� �� ������ �� �������� ���ݵ� ���� �������ִ� �� ����.", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("����. �� �׻� �׶��� ���� ������ִ±���.", "�Ƹ���", Resources.Load<Sprite>("npc1_image"))
                    }
                )

            },
            {
                2, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�׷�... �׶��� ����� �׻� ������ Ư����. ����� �� ������ �� �������� ���ݵ� ���� �������ִ� �� ����.", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("����. �� �׻� �׶��� ���� ������ִ±���.", "�Ƹ���", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            },
            {
                3, new DialogueData(
                    "�Ƹ���",
                    new DialogueLine[] {
                        new DialogueLine("�׷�... �׶��� ����� �׻� ������ Ư����. ����� �� ������ �� �������� ���ݵ� ���� �������ִ� �� ����.", "�Ƹ���", Resources.Load<Sprite>("npc1_image")),
                        new DialogueLine("����. �� �׻� �׶��� ���� ������ִ±���.", "�Ƹ���", Resources.Load<Sprite>("npc1_image"))
                    }
                )
            }
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

        // ������ ��ư �����
        HideChoiceButtons();
    }

    // ������ ��ȭ UI ������Ʈ
    private void ShowChoiceDialogue(DialogueLine line)
    {
        normalDialoguePanel.SetActive(false);
        choiceDialoguePanel.SetActive(true);

        dialogueTextChoice.text = line.text;
        npcNameTextChoice.text = line.npcName;
        npcImageChoice.sprite = line.npcImage;

        // ������ ��ư ������Ʈ
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);
        choiceButton3.gameObject.SetActive(true);

        choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[0];
        choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[1];
        choiceButton3.GetComponentInChildren<TextMeshProUGUI>().text = line.choices[2];

        // ������ ��ư ������ �߰�
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();
        choiceButton3.onClick.RemoveAllListeners();
        choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
        choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
        choiceButton3.onClick.AddListener(() => OnChoiceSelected(2));

        waitingForChoice = true; // �������� ��ٸ��� ���·� ����
    }

    // �������� ���õǾ��� �� ó��
    void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log($"�÷��̾ ������ �߽��ϴ�: {choiceIndex}");

        HideChoiceButtons();
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

    // ������ ��ư �����
    private void HideChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
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


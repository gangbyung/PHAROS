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

    public Sprite[] npcSprites; // NPC ��������Ʈ �迭

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
            "����, ������ ��å���Դ��ϸ� ��� ������ �̸���̳� ...:0",
            "�����ҿ��� ���� ���� ���ĺ��� �ϴ��� ��г��ڰ� �Ա����� �������� ����:1",
            "�׷��� ���� �Ѱ�, ������ ���⼭ ���Ѵٰ� �߾��� �� ������, ���� �˾�?:2",
            "�Ƹ� ������ �����̶� ������ ���� �ſ���.:3",
            "�׷�? �׷� �� �׷��� ū �ϵ� �ƴϱ���.:4",
            "��, ���������� �帶ö�̱⵵ �߱���,:5",
            "'...��Ȳ�� ��� �ǰ� �ִ����� ���� ȣ��� ���� ������ �ٹ濡 ����Ǹ� ū���� ���ٵ�':6",
            ":7" // ���� ���
        });

        talkData.Add(2000, new string[] {
            "� ������! ����� �츮 ������ �߿��� ���Դϴ�.:0",
            "�̰��� �������� �߿��� �ڿ����� ��������.:1",
            "������ ���͵帱���?:2",
            "�̰��� �ڿ��� ���Ǿ� ���� �ֽ��ϴ�.:3",
            "�����ΰ� ���� ����� �������?:4",
            "���� ���� �����δ� ������ �ɱ��?:5",
            "����� �ּ���, ���� �ҽ��� �������� ����.:6",
            ":7" // ���� ���
        });

        talkData.Add(3000, new string[] {
            "�̰��� ����� ������ ���Դϴ�.:0",
            "���� ������� �̰��� ���ϰ� ����.:1",
            "������ �̰����� �ذ�å�� ã�´ٸ� ���ڱ���.:2",
            ":3" // ���� ���
        });
    }

    void InitializeNameData()
    {
        nameData.Add(1000, new string[] { "��ĸ���ܸ�&0", "��ĸ���ܸ�&1", "��ĸ���ܸ�&2", "���ΰ�&3", "��ĸ���ܸ�&4", "��ĸ���ܸ�&5", "���ΰ�&6" });
        nameData.Add(2000, new string[] { "���� �ֹ�&0", "���� �ֹ�&1", "���� �ֹ�&2", "���� �ֹ�&3", "���� �ֹ�&4", "���� �ֹ�&5", "���� �ֹ�&6" });
        nameData.Add(3000, new string[] { "������ ���&0", "������ ���&1", "������ ���&2" });
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
                Debug.LogWarning($"'{dialogueID}'�� �ش��ϴ� ��簡 �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"'{npcName}'�� �ش��ϴ� ��� ID�� �����ϴ�.");
        }
    }

    void DisplayCurrentDialogue()
    {
        if (currentDialogueIndex < talkData[currentDialogueID].Length)
        {
            string dialogueLine = talkData[currentDialogueID][currentDialogueIndex];
            if (string.IsNullOrEmpty(dialogueLine))
            {
                // ���� ��� ó��
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
                speakerNameText.text = "�� �� ���� �ι�";
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
            Debug.Log("��� ��縦 ����߽��ϴ�.");
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
                choiceButtonText1.text = "������ 1"; // ���� ������ �ؽ�Ʈ�� ����
                choiceButton1.onClick.RemoveAllListeners();
                choiceButton1.onClick.AddListener(() => OnChoiceSelected(2001)); // �����δ� ���� ��� ID�� ��ü
            }
            else
            {
                choiceButton1.gameObject.SetActive(false);
            }

            if (talkData[currentDialogueID].Length > 2)
            {
                choiceButton2.gameObject.SetActive(true);
                choiceButtonText2.text = "������ 2"; // ���� ������ �ؽ�Ʈ�� ����
                choiceButton2.onClick.RemoveAllListeners();
                choiceButton2.onClick.AddListener(() => OnChoiceSelected(2002)); // �����δ� ���� ��� ID�� ��ü
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
            Debug.LogWarning($"'{nextDialogueID}'�� �ش��ϴ� ��簡 �����ϴ�.");
            isChoiceActive = false;
        }
    }
}

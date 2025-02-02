using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public GameObject WallWarningImage;

    public static HUD instance;

    public Transform player;
    public Transform ai;

    public Image uiImage;

    public Sprite closeSprite;   // ����� ������ �� �̹���
    public Sprite mediumSprite;  // �߰� �Ÿ��� �� �̹���
    public Sprite farSprite;     // �� �Ÿ��� �� �̹���

    
    public int Tutorial_index; //Ʃ�丮�� �ε�����ȣ
    public Image currentTutorialImage; //���� Ʃ�丮�� �̹���
    public Sprite[] TutorialImages; // ����� Ʃ�丮�� �̹���
    
    public TextMeshProUGUI TutorialText; //Ʃ�丮�� ��� ���� �ؽ�Ʈ
    public TextMeshProUGUI NameText; //Ʃ�丮�� ��� �̸� �ؽ�Ʈ
    public string[] texts;
    public string[] Nametexts;

    public Button nextButton;  // ���� ��ư
    public Button prevButton;  // ���� ��ư

    public GameObject TutoPanel;
    public Button explanationButton;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            instance = this; // �ν��Ͻ��� ���� ��ü�� ����
        }
        Tutorial_index = 0;
    }
    private void Start()
    {
        UpdateUI();
        nextButton.onClick.AddListener(Next);
        prevButton.onClick.AddListener(Previous);
        explanationButton.onClick.AddListener(OnOff);
    }
    private void Update()
    {
        // �÷��̾�� AI ���� �Ÿ� ���
        float distance = Vector3.Distance(player.position, ai.position);

        // �Ÿ� ���ǿ� ���� UI ����
        if (distance <= 30f)
        {
            uiImage.sprite = closeSprite; // ����� �̹����� ����
        }
        else if (distance > 30f && distance <= 60f)
        {
            uiImage.sprite = mediumSprite; // �߰� �Ÿ� �̹����� ����
        }
        else
        {
            uiImage.sprite = farSprite; // �� �Ÿ� �̹����� ����
        }
    }
    public void OnWallWarnImage(bool _bool)
    {
        // null üũ�� ���� WallWarningImage�� null�� ��� ���� ����
        if (WallWarningImage != null)
        {
            WallWarningImage.SetActive(_bool);
        }
        else
        {
            Debug.LogWarning("WallWarningImage�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
    private void UpdateUI()
    {
        currentTutorialImage.sprite = TutorialImages[Tutorial_index];
        TutorialText.text = texts[Tutorial_index];
        NameText.text = Nametexts[Tutorial_index];
    }
    void Next()
    {
        if(Tutorial_index < TutorialImages.Length -1)
        {
            Tutorial_index++;
            UpdateUI();
        }
    }
    void Previous()
    {
        if (Tutorial_index > 0)
        {
            Tutorial_index--; 
            UpdateUI();
        }
    }
    void OnOff()
    {
        if (TutoPanel.activeSelf)
        {
            TutoPanel.SetActive(false);
        }
        else
        {
            TutoPanel.SetActive(true);
        }
    }
}

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

    public Sprite closeSprite;   // 가까운 상태일 때 이미지
    public Sprite mediumSprite;  // 중간 거리일 때 이미지
    public Sprite farSprite;     // 먼 거리일 때 이미지

    
    public int Tutorial_index; //튜토리얼 인덱스번호
    public Image currentTutorialImage; //현재 튜토리얼 이미지
    public Sprite[] TutorialImages; // 저장된 튜토리얼 이미지
    
    public TextMeshProUGUI TutorialText; //튜토리얼 기능 설명 텍스트
    public TextMeshProUGUI NameText; //튜토리얼 기능 이름 텍스트
    public string[] texts;
    public string[] Nametexts;

    public Button nextButton;  // 다음 버튼
    public Button prevButton;  // 이전 버튼

    public GameObject TutoPanel;
    public Button explanationButton;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            instance = this; // 인스턴스를 현재 객체로 설정
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
        // 플레이어와 AI 사이 거리 계산
        float distance = Vector3.Distance(player.position, ai.position);

        // 거리 조건에 따른 UI 변경
        if (distance <= 30f)
        {
            uiImage.sprite = closeSprite; // 가까운 이미지로 변경
        }
        else if (distance > 30f && distance <= 60f)
        {
            uiImage.sprite = mediumSprite; // 중간 거리 이미지로 변경
        }
        else
        {
            uiImage.sprite = farSprite; // 먼 거리 이미지로 변경
        }
    }
    public void OnWallWarnImage(bool _bool)
    {
        // null 체크를 통해 WallWarningImage가 null일 경우 오류 방지
        if (WallWarningImage != null)
        {
            WallWarningImage.SetActive(_bool);
        }
        else
        {
            Debug.LogWarning("WallWarningImage가 할당되지 않았습니다.");
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

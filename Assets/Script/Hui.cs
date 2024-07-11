using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hui : MonoBehaviour
{   
    public bool GameEscape = false; //현재 esc를 눌렀는지 누르지 않았는지 알려주는 변수
    public GameObject pauseMainCanvas; //일시정지를 누를 시 나오는 Ui
    public Button ResumeButton; //이어하기 버튼 변수
    public Button PauseButton; //일시정지 버튼 변수

    void Start()
    {
        PauseButton.onClick.AddListener(OnPauseButtonClicked);
        ResumeButton.onClick.AddListener(OnResumeButtonClicked);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //esc를 눌렀을때
        {
            if(GameEscape) //게임이 실행중이면 멈추고 멈춘상태면 실행하기
            {
                ResumeGame();
            }   
            else 
            {
                PauseGame();
            }
        }
    }
    void OnPauseButtonClicked() //클릭 시 게임 일시정지
    {
        if (!GameEscape)
        {
            PauseGame();
        }
    }

    void OnResumeButtonClicked() //클릭 시 게임 일시정지 해제
    {
        if (GameEscape)
        {
            ResumeGame();
        }
    }

    public void PauseGame() //게임 일시정지 함수
    {
        GameManager.Instance.Pause();
        GameEscape = true;
        pauseMainCanvas.SetActive(true);

        PauseButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(true);
    }

    public void ResumeGame() //게임 일시정지 해제 함수
    {
        GameManager.Instance.Resume();
        GameEscape = false;
        pauseMainCanvas.SetActive(false);

        ResumeButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(true);
    }
}

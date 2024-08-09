using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void MainGameScene() //씬 체인지 기능
    {
        SceneManager.LoadScene("MainGameScene"); //메인 게임 씬으로 넘어가기
    }
    public void EndScene() //씬 체인지 기능
    {
        SceneManager.LoadScene("EndScene"); //엔딩 게임 씬으로 넘어가기
    }
}

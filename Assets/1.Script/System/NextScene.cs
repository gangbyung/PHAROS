using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public static void Gameover_99() //씬 체인지 기능
    {
        SceneManager.LoadScene("99.GameOver"); //게임 시작 씬으로 넘어가기
    }
    public static void GameTitle_0()
    {
        SceneManager.LoadScene("0.GameTitle");//게임메뉴씬으로 이동
    }
    public static void Part1_10() //씬 체인지 기능
    {
        SceneManager.LoadScene("10.part1"); //파트1 게임 씬으로 넘어가기
    }
}

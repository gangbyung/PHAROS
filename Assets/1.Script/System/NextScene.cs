using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public static void Gameover_99() //�� ü���� ���
    {
        SceneManager.LoadScene("99.GameOver"); //���� ���� ������ �Ѿ��
    }
    public static void GameTitle_0()
    {
        SceneManager.LoadScene("0.GameTitle");//���Ӹ޴������� �̵�
    }
    public static void Part1_10() //�� ü���� ���
    {
        SceneManager.LoadScene("10.part1"); //��Ʈ1 ���� ������ �Ѿ��
    }
}

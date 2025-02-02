using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // �̱��� ����
    public static SceneTransition instance;
    public Vector3 spawnPosition;
    void Awake()
    {
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
    public void EndScene()
    {
        SceneManager.LoadScene("99.GameOver");
    }
    public void Endd()
    {
        SceneManager.LoadScene("100.GameOver");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("0.GameTitle");
    }
    public void Scene10()
    {
        SceneManager.LoadScene("10.part1");
    }
    public void Scene11()
    {
        SceneManager.LoadScene("11.part2");
    }
    public void Scene12()
    {
        SceneManager.LoadScene("12.part3");
    }
    //�������� 1 ������
    public void Stage1Bad()
    {
        SceneManager.LoadScene("40.BadEndPart1");
    }
    public void Stage1Nomal()
    {
        SceneManager.LoadScene("42.NomalEndPart");
    }
    public void Stage1Happy()
    {
        SceneManager.LoadScene("43.HappyEndPart");
    }
    public void Stage2Bad()
    {
        SceneManager.LoadScene("50.BadEndPart");
    }
    public void Stage2Happy()
    {
        SceneManager.LoadScene("51.HappyEndPart");
    }
    public void Stage3Bad()
    {
        SceneManager.LoadScene("60.BadEndPart");
    }
    public void Stage3Happy()
    {
        SceneManager.LoadScene("61.HappyEndPart");
    }
    public void Stage1()
    {
        int sceneScore = DialogueManager.instance.score;
        if (sceneScore >= 11)
        {
            if(sceneScore <= 17)
            {
                //�븻����
                Stage1Nomal();
            }
            else
            {
                //���ǿ���
                Stage1Happy();
            }
        }
        if (sceneScore <= 10)
        {
            //��忣��
            Stage1Bad();
        }
    }
    public void Stage2()
    {
        int sceneScore = DialogueManager.instance.score;
        if (sceneScore > 11)
        {
            Stage2Happy();
        }
        else
        {
            Stage2Bad();
        }
    }
    public void Stage3()
    {
        int sceneScore = DialogueManager.instance.score;
        if (sceneScore > 11)
        {
            Stage3Happy();
        }
        else
        {
            Stage3Bad();
        }
    }

    private IEnumerator PlayerSpawn(int index)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == index);
        yield return new WaitForSeconds(0.1f);
        spawnPosition = new Vector3(85, 1, -125);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
            StopCoroutine(PlayerSpawn(index));
        }

    }
    // ��ȭ ���� �� �� ����
    
}

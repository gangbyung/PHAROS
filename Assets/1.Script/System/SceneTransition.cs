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
    public void GameStart()
    {
        SceneManager.LoadScene("0.GameTitle");
    }
    public void Scene10()
    {
        SceneManager.LoadScene("10.part1");
    }
    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;
        SceneManager.LoadScene(index);
        StartCoroutine(PlayerSpawn(index));
        
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

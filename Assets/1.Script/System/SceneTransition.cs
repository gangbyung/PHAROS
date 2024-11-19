using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // 싱글톤 패턴
    public static SceneTransition instance;
    public Vector3 spawnPosition;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 이미 다른 인스턴스가 있으면 삭제
        }
        else
        {
            instance = this; // 인스턴스를 현재 객체로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트가 삭제되지 않도록 설정
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
    // 대화 종료 후 씬 변경
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; //싱글톤

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    public void Pause() //게임 일시정지 기능
    {

        Time.timeScale = 0f;
        Debug.Log("게임 일시정지");
    }
    public void Resume() //일시정지 해제 기능
    {
        Time.timeScale = 1.0f;
        

        Debug.Log("게임 일시정지 해제");
    }
}

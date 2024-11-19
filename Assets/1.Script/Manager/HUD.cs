using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject WallWarningImage;

    public static HUD instance;

    private void Awake()
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
}

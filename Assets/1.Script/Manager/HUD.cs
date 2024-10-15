using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject WallWarningImage;

    public static HUD instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 이미 있으면 중복 방지
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

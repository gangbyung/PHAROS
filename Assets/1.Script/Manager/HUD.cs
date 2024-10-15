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
            Destroy(gameObject); // �ν��Ͻ��� �̹� ������ �ߺ� ����
        }
    }

    public void OnWallWarnImage(bool _bool)
    {
        // null üũ�� ���� WallWarningImage�� null�� ��� ���� ����
        if (WallWarningImage != null)
        {
            WallWarningImage.SetActive(_bool);
        }
        else
        {
            Debug.LogWarning("WallWarningImage�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}

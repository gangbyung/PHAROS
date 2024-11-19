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
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ ����
        }
        else
        {
            instance = this; // �ν��Ͻ��� ���� ��ü�� ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ������Ʈ�� �������� �ʵ��� ����
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

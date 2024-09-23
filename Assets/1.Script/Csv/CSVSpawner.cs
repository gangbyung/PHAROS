using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVSpawner : MonoBehaviour
{
    public GameObject prefab;  // ������ ������
    public string filePath;    // CSV ���� ���

    // ���� ���� ��ǥ
    public float startX = 5f;
    public float startZ = -5f;
    public float yPosition = 5f;  // Y ��ǥ ����

    void Start()
    {
        ReadCSVAndSpawnObjects();
    }

    void ReadCSVAndSpawnObjects()
    {
        // StreamReader�� ����Ͽ� ���� �б�
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int row = 0;

                // ���� ������ �� ���� �о��
                while ((line = sr.ReadLine()) != null)
                {
                    // ��ǥ�� ������
                    string[] entries = line.Split(',');

                    for (int col = 0; col < entries.Length; col++)
                    {
                        if (string.IsNullOrWhiteSpace(entries[col]))  // ��ǥ��?
                        {
                            // X ��: �Ʒ��ٷ� ������ -10�� �̵�
                            float xPos = startX + (col * 10f);

                            // Z ��: ���������� ������ -10�� �̵�
                            float zPos = startZ + (row * -10f);

                            // ������Ʈ ����
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);
                            Instantiate(prefab, spawnPosition, Quaternion.identity);
                        }
                    }
                    row++;  // ���� �ٷ� �̵�
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("������ �д� �� ���� �߻�: " + e.Message);
        }
    }
}

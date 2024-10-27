using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVSpawner : MonoBehaviour
{
    public GameObject[] prefabs;  // ������ ������ �迭
    public string filePath;    // CSV ���� ���

    // ���� ���� ��ǥ
    public float startX = 5f;
    public float startZ = -5f;
    public float yPosition = 0f;  // Y ��ǥ ����

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
                        // 'O'�� �ִ� ���� ������Ʈ ����
                        if (entries[col] == "O")
                        {
                            // X ��: ���� ���� +10�� �̵�
                            float xPos = startX + (col * 10f);

                            // Z ��: �ٿ� ���� -10�� �̵�
                            float zPos = startZ + (row * -10f);

                            // ������Ʈ ������ ��ġ
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);

                            // ���� ������ ����
                            if (prefabs.Length > 0)
                            {
                                GameObject randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];

                                // ������Ʈ ����
                                Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                                Debug.Log($"Spawned {randomPrefab.name} at {spawnPosition}");
                            }
                            else
                            {
                                Debug.LogWarning("������ �迭�� �Ҵ�� �׸��� �����ϴ�.");
                            }
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

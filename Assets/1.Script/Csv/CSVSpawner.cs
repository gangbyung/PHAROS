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
                        if (string.IsNullOrWhiteSpace(entries[col]))  // ��ǥ üũ
                        {
                            // X ��: �Ʒ��ٷ� ������ -10�� �̵�
                            float xPos = startX + (col * 10f);

                            // Z ��: ���������� ������ -10�� �̵�
                            float zPos = startZ + (row * -10f);

                            // ������Ʈ ������ ��ġ
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);

                            // ���� ������ ����
                            GameObject randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];

                            // ������ �迭���� �� ��°���� Ȯ��
                            int prefabIndex = Array.IndexOf(prefabs, randomPrefab);

                            // ȸ���� ����
                            Quaternion rotation;

                            if (prefabIndex < 2)
                            {
                                // �迭 �ε��� 5 ����: 0 ~ 90�� ������ ���� Y�� ȸ��
                                float randomYRotation = UnityEngine.Random.Range(0f, 90f);
                                rotation = Quaternion.Euler(0f, randomYRotation, 0f);
                            }
                            else
                            {
                                // �迭 �ε��� 6 �̻�: Y�� ȸ�� 0�� �Ǵ� 90�� �� �ϳ�
                                float fixedYRotation = UnityEngine.Random.Range(0, 2) == 0 ? 0f : 90f;
                                rotation = Quaternion.Euler(0f, fixedYRotation, 0f);
                            }

                            // ������Ʈ ����
                            Instantiate(randomPrefab, spawnPosition, rotation);
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

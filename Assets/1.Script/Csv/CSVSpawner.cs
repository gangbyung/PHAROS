using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVSpawner : MonoBehaviour
{
    public GameObject prefab;  // 생성할 프리팹
    public string filePath;    // CSV 파일 경로

    // 스폰 시작 좌표
    public float startX = 5f;
    public float startZ = -5f;
    public float yPosition = 5f;  // Y 좌표 고정

    void Start()
    {
        ReadCSVAndSpawnObjects();
    }

    void ReadCSVAndSpawnObjects()
    {
        // StreamReader를 사용하여 파일 읽기
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int row = 0;

                // 파일 끝까지 각 줄을 읽어옴
                while ((line = sr.ReadLine()) != null)
                {
                    // 쉼표로 나누기
                    string[] entries = line.Split(',');

                    for (int col = 0; col < entries.Length; col++)
                    {
                        if (string.IsNullOrWhiteSpace(entries[col]))  // 쉼표임?
                        {
                            // X 값: 아랫줄로 갈수록 -10씩 이동
                            float xPos = startX + (col * 10f);

                            // Z 값: 오른쪽으로 갈수록 -10씩 이동
                            float zPos = startZ + (row * -10f);

                            // 오브젝트 생성
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);
                            Instantiate(prefab, spawnPosition, Quaternion.identity);
                        }
                    }
                    row++;  // 다음 줄로 이동
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("파일을 읽는 중 오류 발생: " + e.Message);
        }
    }
}

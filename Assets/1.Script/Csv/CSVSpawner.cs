using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVSpawner : MonoBehaviour
{
    public GameObject[] prefabs;  // 생성할 프리팹 배열
    public string filePath;    // CSV 파일 경로

    // 스폰 시작 좌표
    public float startX = 5f;
    public float startZ = -5f;
    public float yPosition = 0f;  // Y 좌표 고정

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
                        // 'O'가 있는 셀에 오브젝트 생성
                        if (entries[col] == "O")
                        {
                            // X 값: 열에 따라 +10씩 이동
                            float xPos = startX + (col * 10f);

                            // Z 값: 줄에 따라 -10씩 이동
                            float zPos = startZ + (row * -10f);

                            // 오브젝트 생성할 위치
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);

                            // 랜덤 프리팹 선택
                            if (prefabs.Length > 0)
                            {
                                GameObject randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];

                                // 오브젝트 생성
                                Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                                Debug.Log($"Spawned {randomPrefab.name} at {spawnPosition}");
                            }
                            else
                            {
                                Debug.LogWarning("프리팹 배열에 할당된 항목이 없습니다.");
                            }
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

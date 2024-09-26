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
                        if (string.IsNullOrWhiteSpace(entries[col]))  // 쉼표 체크
                        {
                            // X 값: 아랫줄로 갈수록 -10씩 이동
                            float xPos = startX + (col * 10f);

                            // Z 값: 오른쪽으로 갈수록 -10씩 이동
                            float zPos = startZ + (row * -10f);

                            // 오브젝트 생성할 위치
                            Vector3 spawnPosition = new Vector3(xPos, yPosition, zPos);

                            // 랜덤 프리팹 선택
                            GameObject randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];

                            // 프리팹 배열에서 몇 번째인지 확인
                            int prefabIndex = Array.IndexOf(prefabs, randomPrefab);

                            // 회전값 설정
                            Quaternion rotation;

                            if (prefabIndex < 2)
                            {
                                // 배열 인덱스 5 이하: 0 ~ 90도 사이의 랜덤 Y축 회전
                                float randomYRotation = UnityEngine.Random.Range(0f, 90f);
                                rotation = Quaternion.Euler(0f, randomYRotation, 0f);
                            }
                            else
                            {
                                // 배열 인덱스 6 이상: Y축 회전 0도 또는 90도 중 하나
                                float fixedYRotation = UnityEngine.Random.Range(0, 2) == 0 ? 0f : 90f;
                                rotation = Quaternion.Euler(0f, fixedYRotation, 0f);
                            }

                            // 오브젝트 생성
                            Instantiate(randomPrefab, spawnPosition, rotation);
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

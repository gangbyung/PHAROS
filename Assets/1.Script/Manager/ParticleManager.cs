using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ParticleManager Instance { get; private set; }

    public ParticleSystem[] particles;  // 파티클 시스템 배열

    private void Awake()
    {
        // 싱글톤 패턴: 인스턴스가 없을 때 자신을 할당
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // 이미 인스턴스가 존재하면 삭제
        }
    }

    // 아이템을 먹었을 때 특정 파티클을 실행하는 함수
    public void OnItemCollected(int particleIndex)
    {
        // 배열 범위를 벗어나지 않도록 확인
        if (particleIndex >= 0 && particleIndex < particles.Length)
        {
            StartCoroutine(ActivateParticle(particles[particleIndex]));
        }
        else
        {
            Debug.LogWarning("잘못된 파티클 인덱스입니다.");
        }
    }

    // 파티클을 재생하고 재생 시간이 끝나면 자동으로 꺼지게 하는 코루틴
    IEnumerator ActivateParticle(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);  // 파티클 활성화
        particle.Play();  // 파티클 재생

        // 파티클 재생 시간이 끝날 때까지 대기
        while (particle.isPlaying)
        {
            yield return null;
        }

        particle.gameObject.SetActive(false);  // 파티클 비활성화
    }
}

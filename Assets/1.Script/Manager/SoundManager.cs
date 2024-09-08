using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SoundManager Instance { get; private set; }

    public AudioClip[] soundClips; // 사운드 클립 배열
    private AudioSource audioSource; // 오디오 소스 변수

    void Awake()
    {
        // 싱글톤 패턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재할 경우 중복 파괴
        }
    }

    void Start()
    {
        // 오디오 소스 컴포넌트를 가져옴
        audioSource = GetComponent<AudioSource>();
    }

    // 사운드를 재생하는 함수
    public void PlaySound(int index)
    {
        if (index >= 0 && index < soundClips.Length) // 배열 범위 내에서 인덱스를 확인
        {
            audioSource.clip = soundClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("사운드 인덱스가 범위를 벗어났습니다!");
        }
    }
}

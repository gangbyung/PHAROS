using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static SoundManager Instance { get; private set; }

    public AudioClip[] soundClips; // ���� Ŭ�� �迭
    private AudioSource audioSource; // ����� �ҽ� ����

    void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ ��� �ߺ� �ı�
        }
    }

    void Start()
    {
        // ����� �ҽ� ������Ʈ�� ������
        audioSource = GetComponent<AudioSource>();
    }

    // ���带 ����ϴ� �Լ�
    public void PlaySound(int index)
    {
        if (index >= 0 && index < soundClips.Length) // �迭 ���� ������ �ε����� Ȯ��
        {
            audioSource.clip = soundClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("���� �ε����� ������ ������ϴ�!");
        }
    }
}

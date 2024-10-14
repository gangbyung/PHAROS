using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static ParticleManager Instance { get; private set; }

    public ParticleSystem[] particles;  // ��ƼŬ �ý��� �迭

    private void Awake()
    {
        // �̱��� ����: �ν��Ͻ��� ���� �� �ڽ��� �Ҵ�
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // �̹� �ν��Ͻ��� �����ϸ� ����
        }
    }

    // �������� �Ծ��� �� Ư�� ��ƼŬ�� �����ϴ� �Լ�
    public void OnItemCollected(int particleIndex)
    {
        // �迭 ������ ����� �ʵ��� Ȯ��
        if (particleIndex >= 0 && particleIndex < particles.Length)
        {
            StartCoroutine(ActivateParticle(particles[particleIndex]));
        }
        else
        {
            Debug.LogWarning("�߸��� ��ƼŬ �ε����Դϴ�.");
        }
    }

    // ��ƼŬ�� ����ϰ� ��� �ð��� ������ �ڵ����� ������ �ϴ� �ڷ�ƾ
    IEnumerator ActivateParticle(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);  // ��ƼŬ Ȱ��ȭ
        particle.Play();  // ��ƼŬ ���

        // ��ƼŬ ��� �ð��� ���� ������ ���
        while (particle.isPlaying)
        {
            yield return null;
        }

        particle.gameObject.SetActive(false);  // ��ƼŬ ��Ȱ��ȭ
    }
}

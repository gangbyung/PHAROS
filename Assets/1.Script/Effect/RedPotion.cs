using UnityEngine;

public class RedPotion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.ApplyRedPotionEffect();
                Destroy(gameObject); // ������ ����
                ParticleManager.Instance.OnItemCollected(1);
            }
        }
    }
}

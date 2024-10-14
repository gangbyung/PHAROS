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
                Destroy(gameObject); // 아이템 제거
                ParticleManager.Instance.OnItemCollected(1);
            }
        }
    }
}

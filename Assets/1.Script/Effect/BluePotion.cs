using UnityEngine;

public class BluePotion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.ApplyBluePotionEffect();
                Destroy(gameObject); // 아이템 제거
            }
        }
    }
}

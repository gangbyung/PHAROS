using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] map;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gameObject.name == "Map1_Item")
        {
            map[0].SetActive(true);
            Destroy(gameObject);
        }
        if (other.tag == "Player" && gameObject.name == "Map2_Item")
        {
            map[1].SetActive(true);
            Destroy(gameObject);
        }
        if (other.tag == "Player" && gameObject.name == "Map3_Item")
        {
            map[2].SetActive(true);
            Destroy(gameObject);
        }
        if (other.tag == "Player" && gameObject.name == "Map4_Item")
        {
            map[3].SetActive(true);
            Destroy(gameObject);
        }
        if (other.tag == "Player" && gameObject.CompareTag("Key"))
        {
            PlayerMove.Instance.KeyIndexadd();
            Destroy(gameObject);
        }
    }
}

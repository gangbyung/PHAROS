using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] map;
    float Yrotate = 100f;
    float height = 1f;
    float speed = 1f;

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        transform.Rotate(new Vector3(0f, Yrotate * Time.deltaTime , 0f));
        
        float newY = Mathf.PingPong(Time.time * speed, height * 2) - height;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
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
            PlayerMove.instance.KeyIndexadd();
            SoundManager.Instance.PlaySound(3);
            Destroy(gameObject);
        }
    }
}

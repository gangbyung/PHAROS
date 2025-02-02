using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStage3Clear : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransition.instance.Stage3();
        }
    }
    
}

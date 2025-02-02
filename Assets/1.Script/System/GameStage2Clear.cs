using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStage2Clear : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransition.instance.Stage2();
        }
    }
    
}

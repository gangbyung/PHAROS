using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameGOEnd : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(10f);
        SceneTransition.instance.EndScene();
    }
}

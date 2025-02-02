using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndScene : MonoBehaviour
{
    GameObject videos;
    void Start()
    {
        videos = GameObject.Find("Panle");
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(10f);
        SceneTransition.instance.Scene11();
    }
}

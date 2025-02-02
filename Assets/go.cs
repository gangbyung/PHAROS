using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go : MonoBehaviour
{
    public GameObject endObj;
    private void Start()
    {
        if (endObj == null)
        {
            SceneTransition.instance.Scene10();
        }
        else
        {
            SceneTransition.instance.Endd();
        }
        
    }
}

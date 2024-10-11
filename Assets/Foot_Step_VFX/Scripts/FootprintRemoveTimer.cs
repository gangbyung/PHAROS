using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintRemoveTimer : MonoBehaviour
{

    public float footprintDuration = 12f;
    private float footprintDurationTimer = 0f;

    void Update()
    {

        footprintDurationTimer += Time.deltaTime;

        if (footprintDurationTimer > footprintDuration)
        { 
            
            Destroy(gameObject); 
        
        }

    }

}

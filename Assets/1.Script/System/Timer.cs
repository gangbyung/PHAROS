using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        timer.text = "��� �ð� : " + Mathf.Round(time);
    }
}

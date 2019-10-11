using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

    float time = 0.0f;
    int seconds, minutes;
    string milliseconds;
    
    void Update()
    {
        print(time);
        time += Time.deltaTime;
        milliseconds = time.ToString("F2");
        seconds = (int)(time % 60);
        minutes = (int)(time / 60);

        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = milliseconds;
    }
}

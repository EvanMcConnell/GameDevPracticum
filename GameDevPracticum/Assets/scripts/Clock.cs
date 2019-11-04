using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

    // Use this for initialization
    float timeLimit = 120;
    float time = 0.0f;
    string seconds, minutes;
    void Start () {

        
    }
    
    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
		//print(time);
        seconds = (time%60).ToString("f2");
		//seconds = Convert.ToString();
        minutes = ((int)time/60).ToString();

        
        //print(minutes+":"+seconds);
        GameObject.Find("Clock").GetComponent<TMPro.TextMeshProUGUI>().text = seconds;
        if (time > (timeLimit - 2))
        {
                //GameObject.Find("userMessage").GetComponent<Text>().text ="Time UP!";


        }
        if (time > timeLimit)
        {
            //SceneManager.LoadScene("level1");
        }
        
        
    }
}

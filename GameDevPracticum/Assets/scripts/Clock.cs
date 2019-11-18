using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class Clock : MonoBehaviour {

    // Use this for initialization
    float timeLimit = 120;
    float time = 3f;
    string seconds, minutes;
    GameObject[] walls = null;
    NavMeshSurface nm;
    bool wallsDropped = false;
    void Start () {
        nm = GameObject.FindGameObjectWithTag("NavMesh").GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update() {
        if(time > 0) {
        time -= Time.deltaTime;
        //print(time);
        seconds = (time % 120).ToString("f2");
        //seconds = Convert.ToString();
        minutes = ((int)time / 60).ToString();


        //print(minutes+":"+seconds);
        GameObject.Find("Timer Text").GetComponent<TMPro.TextMeshProUGUI>().text = seconds;
        }

        if (time <= 0 && wallsDropped == false)
        {
            if(walls == null) { walls = GameObject.FindGameObjectsWithTag("Inner Wall"); }
            GameObject.Find("Timer Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Run";
            foreach (GameObject n in walls)
            {
                n.GetComponent<Animator>().enabled = true;
            }
            print("starting to wait for wall drop");
            StartCoroutine(waitForWallDrop());
            print("waiting for wall drop");
            wallsDropped = true;
        }

        IEnumerator waitForWallDrop()
        {
            print("waiting to build navmesh after wall drop");
            yield return new WaitForSecondsRealtime(10f);
            nm.BuildNavMesh();
            print("built navmesh after wall drop");
        }
    }
}

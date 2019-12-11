using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class Clock : MonoBehaviour {

    // Use this for initialization
    float timeLimit = 120;
    float time = 120f;
    string seconds, minutes;
    public GameObject[] walls = null, enemies;
    public GameObject player;
    NavMeshSurface nm;
    public bool wallsDropped = false;
    void Start () {
        nm = GameObject.FindGameObjectWithTag("NavMesh").GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update() {
        if(time > 0 && player.GetComponent<PlayerMovement>().isWithNPC != true) {
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
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach(GameObject n in enemies)
            {
                n.gameObject.GetComponentInParent<EnemyMovement>().transitioning = true;
                Animator enemyAnim = n.gameObject.GetComponentInParent<EnemyMovement>().anim;
                n.gameObject.GetComponentInParent<NavMeshAgent>().speed = 0;
                n.gameObject.GetComponentInParent<Light>().color = Color.red;
                enemyAnim.SetBool("isDropping", true);
                yield return new WaitForSecondsRealtime((float)2.9);
                enemyAnim.SetBool("isFollowing", true);
                enemyAnim.SetBool("isDropping", false);
                n.gameObject.GetComponentInParent<EnemyMovement>().following = true;
                n.gameObject.GetComponentInParent<NavMeshAgent>().speed = 5;
                n.gameObject.GetComponentInParent<EnemyMovement>().transitioning = false;
            }

            print("built navmesh after wall drop");
        }
    }
}

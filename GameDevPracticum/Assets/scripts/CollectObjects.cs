using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectObjects : MonoBehaviour {

    // Use this for initialization

    int score;
    int time;

    void Start () {
        print ("Start method");
        score = 0;
        time = 0;
    }
    
    // Update is called once per frame
    void Update () {
        print ("Update method");
		//Timer();
    }


    // void OnCollisionEnter (Collision coll)
    // {
    //      string nameOfObject = coll.collider.gameObject.name;
    //      print ("Collided with "+ nameOfObject);


    // }

   /* void Timer(){
        time += (int)Time.deltaTime;
        GameObject.Find("Clock").GetComponent<Text>().text = ":" + time;
    }*/

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "pick_me")
        {
            string nameOfObject = hit.collider.gameObject.name;
            print ("Collided with "+ nameOfObject);
            Destroy(hit.collider.gameObject);
            score++;
            print("Score: "+score);
            if (score > 2) SceneManager.LoadScene("level2");
            //please add level2 to teh Build Settings
            GameObject.Find("userMessage").GetComponent<Text>().text  = "Score: " + score;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

	public GameObject[] objects;
    GameObject spawnedObject, player;
    public bool isEntrance;
    bool firstEntrance;
    public Vector3 r, entranceRotation;
    public string tester = "null spawn tester";
    int rand;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        firstEntrance = player.GetComponent<PlayerMovement>().firstEntranceSpawned;
        rand = Random.Range(0, objects.Length);
        if (isEntrance == true) { print("This is an isEntrance: " + isEntrance + "The First isEntrance has been spawned: " + firstEntrance + "Number of objects: " + objects.Length); }
        if (isEntrance == true && firstEntrance == false && objects.Length > 0) { 
            print("Spawned an entrance");
            print("Entrance Rotation : " + GameObject.FindGameObjectWithTag("Entrance Point").GetComponent<SpawnObject>().entranceRotation);
            entranceRotation = GameObject.FindGameObjectWithTag("Entrance Point").GetComponent<SpawnObject>().entranceRotation; 
            spawnedObject = Instantiate(objects[rand], transform.position, Quaternion.identity);
            spawnedObject.transform.parent = transform;
        }
        else if(objects.Length > 0) { 
            if(objects[0].name == "Shop") { print("Wrong method used to spawn entrance, whoops"); }
            print(tester);
            spawnedObject = Instantiate(objects[rand], transform.position, Quaternion.Euler(r)); spawnedObject.transform.parent = transform;
            spawnedObject.transform.parent = transform;
        }
        
	}

    public void refresh()
    {
        rand = Random.Range(0, objects.Length);
        Destroy(spawnedObject);
        spawnedObject = Instantiate(objects[rand], transform.position, Quaternion.Euler(r)); spawnedObject.transform.parent = transform;
        spawnedObject.transform.parent = transform;
    }
}

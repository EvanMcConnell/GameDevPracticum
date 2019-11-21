﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

	public GameObject[] objects;
    GameObject spawnedObject, player;
    public bool entrance;
    bool firstEntrance;
    public Vector3 r, entrancePoint;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        firstEntrance = player.GetComponent<PlayerMovement>().firstEntranceSpawned;
		int rand = Random.Range(0, objects.Length);
        if (entrance == true && firstEntrance == true) { entrancePoint = GameObject.FindGameObjectWithTag("Entrance Point").GetComponent<SpawnObject>().r; spawnedObject = Instantiate(objects[rand], transform.position, Quaternion.Euler(entrancePoint)); }
        else if (entrance != true) { spawnedObject = Instantiate(objects[rand], transform.position, Quaternion.Euler(r)); spawnedObject.transform.parent = transform; }
        
	}
}

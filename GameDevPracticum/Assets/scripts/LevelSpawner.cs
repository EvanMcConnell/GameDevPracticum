using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelSpawner : MonoBehaviour
{
    public GameObject spawnedLevel, Level, entrance;
    public NavMeshSurface NM;
    public GameObject[] doors;
    GameObject current, exit, player;
    List<GameObject> doorSpots;
    int entranceChoice;
    int exitChoice;
    Vector3 entranceRotation, exitRotation;

    void Start()
    {
        spawnedLevel = Instantiate(Level, new Vector3(0,0,0), Quaternion.identity);
        spawnedLevel.transform.parent = transform;
        spawnedLevel.transform.position = gameObject.transform.position;

        doorSpots = GameObject.FindGameObjectsWithTag("door").ToList();
        entranceChoice = Random.Range(0, doorSpots.Count -1);
        exitChoice = Random.Range(0, doorSpots.Count - 2);

        current = doorSpots[entranceChoice];
        current.tag = "Entrance Point";
        entranceRotation = current.GetComponent<SpawnObject>().r;
        entrance = Instantiate(doors[1], current.transform.position, Quaternion.Euler(entranceRotation));
        entrance.transform.parent = transform;
        doorSpots.RemoveAt(entranceChoice);

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = entrance.transform.position;

        current = doorSpots[exitChoice];
        exitRotation = current.GetComponent<SpawnObject>().r;
        exit = Instantiate(doors[2], current.transform.position, Quaternion.Euler(exitRotation));
        exit.transform.parent = transform;
        doorSpots.RemoveAt(exitChoice);

        foreach(GameObject n in doorSpots)
        {
            current = Instantiate(doors[0], n.transform.position, Quaternion.identity);
            current.transform.parent = transform;
        }

        StartCoroutine(waitForNavMesh());
    }

    IEnumerator waitForNavMesh()
    {
        print("waiting");
        yield return new WaitForSeconds(1);
        NM.BuildNavMesh();
        print("NavMesh Built");
    }
}

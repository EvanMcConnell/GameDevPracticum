using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelSpawner : MonoBehaviour
{
    public 
    public GameObject Level;
    public NavMeshSurface NM;
    public GameObject[] doors;
    GameObject current, exit, entrance, player;
    List<GameObject> doorSpots;
    int entranceChoice;
    int exitChoice;
    public NavMeshSurface NavMesh;

    void Start()
    {
        Instantiate(Level);

        doorSpots = GameObject.FindGameObjectsWithTag("door").ToList();
        entranceChoice = Random.Range(0, doorSpots.Count -1);
        exitChoice = Random.Range(0, doorSpots.Count - 2);

        current = doorSpots[entranceChoice];
        entrance = Instantiate(doors[1], current.transform.position, Quaternion.identity);
        entrance.transform.parent = transform;
        doorSpots.RemoveAt(entranceChoice);

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = entrance.transform.position;

        current = doorSpots[exitChoice];
        exit = Instantiate(doors[2], current.transform.position, Quaternion.identity);
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
        NavMesh.BuildNavMesh();
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitUntil();
    }
}

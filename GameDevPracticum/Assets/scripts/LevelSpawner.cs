using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelSpawner : MonoBehaviour
{
    public 
    public GameObject Level;
    public NavMeshSurface NavMesh;

    void Start()
    {
        Instantiate(Level);

        NavMesh.BuildNavMesh();
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitUntil();
    }
}

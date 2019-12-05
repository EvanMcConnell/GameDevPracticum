using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDropper : MonoBehaviour
{
    public GameObject player;
    public GameObject[] walls;

    public void findWalls()
    {
        walls = GameObject.FindGameObjectsWithTag("Inner Wall");
        print(walls.Length);
    }

    public void dropWalls()
    {
        print("wall dropped");
        foreach (GameObject n in walls)
        {
            n.GetComponent<Animator>().enabled = true;
        }
    }
}

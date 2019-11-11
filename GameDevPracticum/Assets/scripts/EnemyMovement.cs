using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform target, nextStop, lastPosition, currentPosition;
    public Transform[] patrolRoute;
    public bool following = false;
    bool turnable = true;
    int stopCount = 0;
    Ray playerFinder;
    public Collider searchCone;
    public Animator anim;
    public AudioSource audioSourceIdle, audioSourceDying, audioSourceChasing;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitingToSpeak());
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nextStop = patrolRoute[stopCount];
        //print("stopCount: " + stopCount.ToString());

        playerFinder = new Ray(transform.position, Vector3.back);

        currentPosition = transform;
        lastPosition = transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerFinder = new Ray(transform.position, transform.forward);
        Debug.DrawRay(playerFinder.origin, playerFinder.direction, Color.red);

        lastPosition = currentPosition;
        currentPosition = transform;



        if (following == true)
        {
            agent.SetDestination(target.position);
        }
        else 
        {
            agent.SetDestination(nextStop.position);
        }

        if(agent.remainingDistance < 0.1 && turnable == true)
        {

            if (stopCount == patrolRoute.Length - 1)
            {
                stopCount = 0;
            } else stopCount++;
            //print("stopCount: " + stopCount.ToString());
            nextStop = patrolRoute[stopCount];
            turnable = false;
            StartCoroutine(waitingToTurn());
        }
    }

    IEnumerator waitingToTurn()
    {
        yield return new WaitForSeconds(1);
        turnable = true;
    }

    IEnumerator waitingToSpeak()
    {
        yield return new WaitForSeconds(Random.Range((float)0.1, 1));
        audioSourceIdle.enabled = true;
    }
}

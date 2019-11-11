using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int score, lives;
    public Rigidbody rb;
    public float speed = 4;
    public Camera cam;
    Vector3 lookTarget = Vector3.forward;
    GameObject CollidedObject;
    bool exitOpen = false;
    public AudioClip idle, dying, chasing;
    public GameObject[] redCubes, greenCubes, activeLives, deadLives;
    GameObject entrance;
    public GameObject greenCubesContainer, exitText;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        print(lives);
        score = 0;
        GameObject.Find("Score Text").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        StartCoroutine(WaitForLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSecondsRealtime((float)0.4);
        entrance = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpawner>().entrance;
    }

    IEnumerator WaitForPickup()
    {
        CollidedObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime((float)0.4);
        Destroy(CollidedObject);
    }

    IEnumerator EnemyAnimationsHit(Collider hit)
    {
        Animator enemyAnim = hit.gameObject.GetComponentInParent<EnemyMovement>().anim;
        hit.gameObject.GetComponentInParent<NavMeshAgent>().speed = 0;
        hit.gameObject.GetComponentInParent<Light>().color = Color.red;
        enemyAnim.SetBool("isDropping", true);
        yield return new WaitForSecondsRealtime((float)2.9);
        enemyAnim.SetBool("isFollowing", true);
        hit.gameObject.GetComponentInParent<EnemyMovement>().following = true;
        hit.gameObject.GetComponentInParent<NavMeshAgent>().speed = 5;
    }

    IEnumerator EnemyAnimationsTriggered(GameObject n)
    {
        Animator enemyAnim = n.gameObject.GetComponentInChildren<EnemyMovement>().anim;
        n.gameObject.GetComponentInChildren<NavMeshAgent>().speed = 0.4f;
        n.gameObject.GetComponent<Light>().color = Color.green;
        enemyAnim.SetBool("isFollowing", false);
        enemyAnim.SetBool("isDropping", false);
        n.gameObject.GetComponentInChildren<SphereCollider>().enabled = false;

        yield return new WaitForSecondsRealtime(2);
        n.gameObject.GetComponentInChildren<SphereCollider>().enabled = true;
    }

    void OnTriggerEnter(Collider hit)
    {
        CollidedObject = hit.gameObject;

        //PickUp Trigger
        if (hit.gameObject.tag == "pick_me")
        {
            hit.gameObject.GetComponent<MeshRenderer>().enabled = false;
            hit.gameObject.GetComponent<AudioSource>().enabled = true;
            StartCoroutine(WaitForPickup());

            score++;

            redCubes[score - 1].gameObject.SetActive(false);
            greenCubes[score - 1].gameObject.SetActive(true);

            GameObject.Find("Score Text").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
            if (score == 9)
            {
                exitOpen = true;
                greenCubesContainer.SetActive(false);
                exitText.SetActive(true);

            }
        }

        //Death Trigger
        if (hit.gameObject.tag == "Enemy")
        {
            if (lives == 0) 
            {
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
                SceneManager.LoadScene("End Scene"); 
            }
            else
            {
                int i = 0;
                foreach (GameObject n in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    i++;
                    
                    n.GetComponent<EnemyMovement>().following = false;
                    StartCoroutine(EnemyAnimationsTriggered(n));
                }

                print("no of enemies: " + i);
                this.transform.position = entrance.transform.position;

                activeLives[lives - 1].SetActive(false);
                deadLives[lives - 1].SetActive(true);

                lives--;
                print(lives);
            }
        }

        //Chase Trigger
        if(hit.gameObject.tag == "SearchCone" && hit.gameObject.GetComponentInParent<EnemyMovement>().following == false)
        {
            StartCoroutine(EnemyAnimationsHit(hit));
        }

        //Exit Trigger
        if(hit.gameObject.tag == "Exit" && exitOpen == true)
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
            SceneManager.LoadScene("Level End Scene");
        }
    }

    void FixedUpdate()
    {
        //Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontal = horizontal / 2;
        }

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        rb.AddForce(movement * speed / Time.deltaTime);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lookTarget = hit.point;
        }
        transform.LookAt(lookTarget);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}

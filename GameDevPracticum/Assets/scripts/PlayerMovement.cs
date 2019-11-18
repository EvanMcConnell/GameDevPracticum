using System.Collections;
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
    public GameObject greenCubesContainer, exitText, LevelSpawner;
    public Animator entranceDoorAnim;
    public bool firstEntranceSpawned = false, nextLevelSpawned = false;
    public int nextLevelSpawnPointChoice;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        print(lives);
        score = 8;
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
        firstEntranceSpawned = true;
    }

    IEnumerator WaitForPickup()
    {
        CollidedObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime((float)0.4);
        Destroy(CollidedObject);
    }

    IEnumerator EnemyAnimationsChaseStart(Collider hit)
    {
        print("starting chase");
        hit.gameObject.GetComponentInParent<EnemyMovement>().transitioning = true;
        Animator enemyAnim = hit.gameObject.GetComponentInParent<EnemyMovement>().anim;
        hit.gameObject.GetComponentInParent<NavMeshAgent>().speed = 0;
        hit.gameObject.GetComponentInParent<Light>().color = Color.red;
        enemyAnim.SetBool("isDropping", true);
        yield return new WaitForSecondsRealtime((float)2.9);
        enemyAnim.SetBool("isFollowing", true);
        enemyAnim.SetBool("isDropping", false);
        hit.gameObject.GetComponentInParent<EnemyMovement>().following = true;
        hit.gameObject.GetComponentInParent<NavMeshAgent>().speed = 5;
        hit.gameObject.GetComponentInParent<EnemyMovement>().transitioning = false;
    }

    IEnumerator EnemyAnimationsKilledPlayer(GameObject n, int i)
    {
        print("player slain");
        if (n.gameObject.GetComponentInChildren<EnemyMovement>().transitioning == true) { print("waiting for transition");  yield return new WaitUntil(() => n.gameObject.GetComponentInChildren<EnemyMovement>().transitioning == false); }
        n.GetComponent<EnemyMovement>().following = false;
        Animator enemyAnim = n.gameObject.GetComponentInChildren<EnemyMovement>().anim;
        n.gameObject.GetComponentInChildren<NavMeshAgent>().speed = 0.4f;
        n.gameObject.GetComponent<Light>().color = Color.green;
        enemyAnim.SetBool("isFollowing", false);
        enemyAnim.SetBool("isDropping", false);
        enemyAnim.SetBool("isReset", true);
        

        n.gameObject.GetComponentInChildren<SphereCollider>().enabled = false;

        yield return new WaitForSecondsRealtime(2);
        enemyAnim.SetBool("isReset", false);
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
            print(score);

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


        //Shop Exit Trigger
        if(hit.gameObject.tag == "Shop Exit")
        {
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Close", false);
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Open", true);
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
                    StartCoroutine(EnemyAnimationsKilledPlayer(n, i));
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
            StartCoroutine(EnemyAnimationsChaseStart(hit));
        }


        //Exit Trigger
        if(hit.gameObject.tag == "Exit" && exitOpen == true)
        {
            hit.GetComponent<BoxCollider>().enabled = false;
            print("exit hitted");
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
            GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Open", true);
            //hit.gameObject.
            //GameObject.FindGameObjectWithTag("Entrance Door").GetComponent<Animator>().SetBool("Open", true);
            //SceneManager.LoadScene("Level End Scene");
            nextLevelSpawnPointChoice = 0;
            if(nextLevelSpawned == false) { Instantiate(LevelSpawner); nextLevelSpawned = true; }
        }


        //Entrance Trigger
        if (hit.gameObject.tag == "Entrance")
        {
            print("entrance");
            GameObject.FindGameObjectWithTag("Entrance Door").GetComponent<Animator>().SetBool("Close", true);
            GameObject.FindGameObjectWithTag("Entrance Door").GetComponent<Animator>().SetBool("Open", false);
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

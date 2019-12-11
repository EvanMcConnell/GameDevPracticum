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
    GameObject CollidedObject, entrance, nextLevel;
    public AudioClip idle, dying, chasing;
    public GameObject[] redCubes, greenCubes, activeLives, deadLives;
    public GameObject greenCubesContainer, exitText, LevelSpawner, NPCSprite, clock;
    string scoreText;
    public Animator entranceDoorAnim;
    public bool firstEntranceSpawned = false, nextLevelSpawned = false, isWithNPC = false, exitOpen = false;
    public int nextLevelSpawnPointChoice;
    public SpriteRenderer thisDialoguePrompt;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        print(lives);
        score = 0;
        scoreText = GameObject.Find("Score Text").GetComponent<TMPro.TextMeshProUGUI>().text;
        scoreText = score.ToString();
        StartCoroutine(WaitForLevel());

        thisDialoguePrompt = GameObject.FindGameObjectWithTag("Dialogue Prompt").gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        thisDialoguePrompt.transform.LookAt(cam.transform);
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSecondsRealtime((float)0.4);
        NPCSprite.GetComponent<NPCHandler>().npcText.text = "Hey kid, there's some yellow glowy cube things in the next room that look kinda neat. Why don't you go get em all n bring em back to me. thanks kid! oh and by the way, if you're not done within the next 2 minutes, you'll regret it!!";
        NPCSprite.GetComponent<NPCHandler>().npcText.enabled = true;
        NPCSprite.GetComponent<SpriteRenderer>().enabled = true;
        NPCSprite.GetComponent<AudioSource>().enabled = true;
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

        yield return new WaitForSecondsRealtime(0.1f);
        NPCSprite.GetComponent<NPCHandler>().npcText.enabled = true;
        NPCSprite.GetComponent<SpriteRenderer>().enabled = true;
        NPCSprite.GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSecondsRealtime(1.9f);
        
        enemyAnim.SetBool("isReset", false);
        n.gameObject.GetComponentInChildren<SphereCollider>().enabled = true;

        print("After waiting Wallse Dropped: " + clock.GetComponent<Clock>().wallsDropped);
        if (clock.GetComponent<Clock>().wallsDropped == true)
        {
            
            foreach (GameObject x in clock.GetComponent<Clock>().enemies)
            {
                x.gameObject.GetComponentInParent<EnemyMovement>().transitioning = true;
                enemyAnim = x.gameObject.GetComponentInParent<EnemyMovement>().anim;
                x.gameObject.GetComponentInParent<NavMeshAgent>().speed = 0;
                x.gameObject.GetComponentInParent<Light>().color = Color.red;
                enemyAnim.SetBool("isDropping", true);
                yield return new WaitForSecondsRealtime((float)2.9);
                enemyAnim.SetBool("isFollowing", true);
                enemyAnim.SetBool("isDropping", false);
                x.gameObject.GetComponentInParent<EnemyMovement>().following = true;
                x.gameObject.GetComponentInParent<NavMeshAgent>().speed = 5;
                x.gameObject.GetComponentInParent<EnemyMovement>().transitioning = false;
            }

            print("Wall Dropper Found");
            GameObject.FindGameObjectWithTag("Wall Dropper").GetComponent<WallDropper>().findWalls();
            GameObject.FindGameObjectWithTag("Wall Dropper").GetComponent<WallDropper>().dropWalls();
        }
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
            if (score % 9 == 0)
            {
                exitOpen = true;
                //greenCubesContainer.SetActive(false);
                //exitText.SetActive(true);

            }
        }


        //Shop Exit Trigger
        if(hit.gameObject.tag == "Shop Exit")
        {
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Close", false);
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Open", true);

            if(exitOpen == true)
            {
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Close", true);
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Open", false);
            }
        }


        //Shop Entrance Trigger
        if (hit.gameObject.tag == "Shop Entrance")
        {
            hit.GetComponent<BoxCollider>().enabled = false;
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Open", false);
            hit.gameObject.GetComponentInParent<Animator>().SetBool("Close", true);


            //Destroy(GameObject.FindGameObjectWithTag("Trash"));
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
                this.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
                NPCSprite.GetComponent<NPCHandler>().npcText.text = "Hey kid, way to go you managed to get " + score + " of them box things before dying. Maybe try not dying until you get em all this time, good luck!!";
                

                activeLives[lives - 1].SetActive(false);
                deadLives[lives - 1].SetActive(true);

                lives--;
                print(lives);

                exitOpen = false;
            }
        }


        //Chase Trigger
        if(hit.gameObject.tag == "SearchCone" && hit.gameObject.GetComponentInParent<EnemyMovement>().following == false)
        {
            StartCoroutine(EnemyAnimationsChaseStart(hit));
        }



        //Dialogue Trigger
        if(hit.gameObject.tag == "NPC Character")
        {
            isWithNPC = true;
        }



        //Exit Trigger
        /*if(hit.gameObject.tag == "Entrance" && exitOpen == true)
        {
            hit.GetComponent<BoxCollider>().enabled = false;
            
            print("exit hitted");
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
            GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Open", true);
            GameObject.FindGameObjectWithTag("Level").tag = "Trash";
            GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Open", true);

            foreach(GameObject n in GameObject.FindGameObjectsWithTag("Exit Door"))
            {
                n.GetComponent<Animator>().SetBool("Open", true);
                n.GetComponent<Animator>().SetBool("Close", false);
            }

            SceneManager.LoadScene("Level End Scene");
            /*nextLevelSpawnPointChoice = 0;
            if(nextLevelSpawned == false) { 
                nextLevel = Instantiate(LevelSpawner, GameObject.FindGameObjectWithTag("Next Level Spawn Point").transform); nextLevelSpawned = true;
                nextLevel.transform.SetParent(null);
                hit.transform.parent.SetParent(nextLevel.transform);
            }
        }*/


        //Entrance Trigger
        if (hit.gameObject.tag == "Entrance")
        {
            print("entrance");

            GameObject.FindGameObjectWithTag("Shop Exit").GetComponent<BoxCollider>().enabled = true;
            //GameObject.FindGameObjectWithTag("Shop Exit").gameObject.GetComponentInParent<Animator>().SetBool("Open", true);
            //GameObject.FindGameObjectWithTag("Shop Exit").gameObject.GetComponentInParent<Animator>().SetBool("Close", false);

            GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Close", true);
            GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Open", false);

            if (exitOpen == true)
            {
                GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Close", false);
                GameObject.FindGameObjectWithTag("Exit Door").GetComponent<Animator>().SetBool("Open", true);
            }
        }
    }



    void OnTriggerExit(Collider exit)
    {
        if(exit.gameObject.tag == "NPC Character")
        {
            isWithNPC = false;
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

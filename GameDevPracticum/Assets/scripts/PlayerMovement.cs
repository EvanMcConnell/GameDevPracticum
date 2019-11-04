using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int score;
    public Rigidbody rb;
    public float speed = 4;
    public Camera cam;
    Vector3 lookTarget = Vector3.forward;
    GameObject CollidedObject;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForPickup()
    {
        CollidedObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime((float)0.4);
        Destroy(CollidedObject);
    }

    void OnTriggerEnter(Collider hit)
    {
        CollidedObject = hit.gameObject;

        //PickUp Trigger
        if (hit.gameObject.tag == "pick_me")
        {
            hit.gameObject.GetComponent<AudioSource>().enabled = true;
            StartCoroutine(WaitForPickup());

            score++;
            GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
            if (score == 9)
            {
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
                SceneManager.LoadScene("Level End Scene");

            }
        }

        //Death Trigger
        if (hit.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("End Scene");
        }

        //Chase Trigger
        if(hit.gameObject.tag == "SearchCone")
        {
            hit.gameObject.GetComponentInParent<EnemyMovement>().following = true;
            hit.gameObject.GetComponentInChildren<Light>().color = Color.red;
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

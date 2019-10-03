using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageWeapons : MonoBehaviour
{
    Camera PlayerCamera;
    Ray RayFromPlayer;
    RaycastHit hit;
    public GameObject SparkAtImpact;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RayFromPlayer = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawLine(RayFromPlayer.origin, RayFromPlayer.direction * 100, Color.red);
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(RayFromPlayer, out hit, 100))
            {
                print("The object " + hit.collider.gameObject.name + " is in front of the player.");
                Vector3 PositionOfImpact = hit.point;
                Instantiate(SparkAtImpact, PositionOfImpact, Quaternion.identity);
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCHandler : MonoBehaviour
{
    public string[] phrases;
    public GameObject player;
    public GameObject[] hud;
    public TextMeshProUGUI npcText;
    public bool nearPlayer;

    int phraseChoice;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npcText = GameObject.FindGameObjectWithTag("NPC Text").gameObject.GetComponent<TextMeshProUGUI>();
        npcText.enabled = false;
        hud = GameObject.FindGameObjectsWithTag("HUD");
    }

    void Update()
    {
        nearPlayer = player.GetComponent<PlayerMovement>().isWithNPC;

        if (nearPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
                npcText.enabled = !npcText.enabled;
                phraseChoice = Random.Range(0, phrases.Length);

                if(npcText.enabled == true)
                {
                    npcText.text = phrases[phraseChoice];
                }

                foreach(GameObject n in hud) { n.SetActive(false); }
            }
        }
        else { 
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            npcText.enabled = false;
            foreach (GameObject n in hud) { n.SetActive(true); }
        }
    }
}

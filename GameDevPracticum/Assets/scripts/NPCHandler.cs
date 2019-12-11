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
    public SpriteRenderer dialoguePrompt;
    public AudioSource npcAudio;

    int phraseChoice;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        npcText = GameObject.FindGameObjectWithTag("NPC Text").gameObject.GetComponent<TextMeshProUGUI>();
        npcText.enabled = false;

        hud = GameObject.FindGameObjectsWithTag("HUD");

        dialoguePrompt = GameObject.FindGameObjectWithTag("Dialogue Prompt").gameObject.GetComponent<SpriteRenderer>();
        dialoguePrompt.enabled = false;
    }

    void Update()
    {
        nearPlayer = player.GetComponent<PlayerMovement>().isWithNPC;


        if (nearPlayer == true)
        {
            dialoguePrompt.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
                gameObject.GetComponent<AudioSource>().enabled = !gameObject.GetComponent<AudioSource>().enabled;
                /*if (npcText.enabled == true) { npcText.enabled = false; }
                else { npcText.enabled = false; }*/
                npcText.enabled = !npcText.enabled;
                phraseChoice = Random.Range(0, phrases.Length);

                if(npcText.enabled == true)
                {
                    npcText.text = phrases[phraseChoice];
                }

                if(player.GetComponent<PlayerMovement>().score % 9 ==0 && player.GetComponent<PlayerMovement>().score != 0)
                {
                    npcText.text = "Thanks kid, but you're not done yet. There's more boxes out there now so you know what to do. so quite slackin and get to work!!";
                    foreach(GameObject n in GameObject.FindGameObjectsWithTag("Room Spawn"))
                    {
                        n.GetComponent<SpawnObject>().refresh();
                        player.GetComponent<PlayerMovement>().exitOpen = false;
                    }
                }

                foreach(GameObject n in hud) { n.SetActive(false); }
            }
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            npcText.enabled = false;
            gameObject.GetComponent<AudioSource>().enabled = false;
            foreach (GameObject n in hud) { n.SetActive(true); }
            dialoguePrompt.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "End Scene")
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = "Final Score: " + PlayerPrefs.GetInt("Score").ToString();
        }
        if (SceneManager.GetActiveScene().name == "Level End Scene")
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = "Current Score: " + PlayerPrefs.GetInt("Score").ToString();
        }
    }
}

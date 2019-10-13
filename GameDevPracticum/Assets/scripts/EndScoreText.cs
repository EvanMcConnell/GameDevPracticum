using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "Final Score: " + PlayerPrefs.GetInt("Score").ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    void ResetTransform() {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-1.647f, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Dialogue").SetActive(false);// Turns off this gameobject
        GameObject.Find("Confirm Single Card Button").SetActive(false);// Turn off this gameobject
    }
}

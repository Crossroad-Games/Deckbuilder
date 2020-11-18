using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCamera : MonoBehaviour
{
    private void Awake() => GetComponent<Canvas>().worldCamera = GameObject.Find("Camera").GetComponent<Camera>();// Sets this canvas' camera
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Interactable.NearInteractable += NearInteractable;// Adds this to the listener
        Interactable.Interacting += Interacting;// Adds this to the Listener
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        Interactable.NearInteractable -= NearInteractable;// Unsubscribe this event
        Interactable.Interacting -= Interacting;// Unsubscribe this event
    }
    public void NearInteractable()
    {

    }
    public void Interacting()
    {

    }
}

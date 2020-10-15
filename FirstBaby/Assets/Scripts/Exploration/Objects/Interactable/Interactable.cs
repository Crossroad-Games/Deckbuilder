using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float InteractableDistance = .5f;// Defines the distance from which the player can interact with this
    public static Action NearInteractable;// Event that is called when the player is near this interactable
    public static Action Interacting;// Event that is called when the player is interacting with this object
    private GameObject Player;// Player Reference will be used to determine distance from this object and possibly other methods
    [SerializeField] public List<Targetable> Targets;// Target objects reference
    [SerializeField] private bool Reusable = false;// Boolean to determine if this can be interacted with more than once
    private bool Used = false;// Boolean to determine if, if its not reusable, this has been actuated once or not
    public virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");// Find the player's reference
        foreach (Targetable Target in Targets)// Go through the list of Targetable objects
            if (Target != null)// If not null
                Interacting += Target.ExecuteAction;// Stores the reference to the linked object
    }
    private void Update() => DetectPlayer();// Check if the player is near every frame
    public virtual void DetectPlayer()// Function designed to detect if the player is withing interactable distance from the object
    {
        if (Player != null && !Used)// If there is a reference to the player
            if ((Player.transform.position - transform.position).magnitude <= InteractableDistance)// If player is within interactable distance
            {
                NearInteractable?.Invoke();// Calls all functions tied to  being near an interactable object
                if (Input.GetButtonDown("Interact"))// If the player presses the interact button and is able to use it
                {
                    Interacting?.Invoke();// Calls all function subscribed to interacting with an object
                    Actuated();// Function that will do something when actuated(Animation, specific effects...)
                    if (!Reusable)// If it is not reusable
                        Used = true;// Can't use it again
                }
                else if (Input.GetButtonUp("Interact"))// When the user lets go of the button
                {
                    OnUnActuation();// Function designed to deal with special cases when releasing the interactive object should do something
                }
            }
    }
    public virtual void OnUnActuation()
    {
        // Do something//
    }
    public abstract void Actuated();// Function that will do something when actuated(Animation, specific effects...)
}

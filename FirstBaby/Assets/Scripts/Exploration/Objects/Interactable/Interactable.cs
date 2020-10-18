﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float InteractableDistance = .5f;// Defines the distance from which the player can interact with this
    public Action NearInteractable;// Event that is called when the player is near this interactable
    public Action Interacting;// Event that is called when the player is interacting with this object
    protected GameObject Player;// Player Reference will be used to determine distance from this object and possibly other methods
    [SerializeField] public List<Targetable> Targets;// Target objects reference
    [SerializeField] private bool Reusable = false;// Boolean to determine if this can be interacted with more than once
    public bool Used = false;// Boolean to determine if, if its not reusable, this has been actuated once or not
    private SaveLoad saveLoad;
    public virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");// Find the player's reference
        saveLoad = GameObject.Find("Game Master").GetComponent<SaveLoad>();
        NearInteractable += Player.GetComponent<DungeonPlayer>().NearInteractable;// Adds this to the listener
        Interacting += Player.GetComponent<DungeonPlayer>().Interacting;// Adds this to the Listener
        if (saveLoad.DungeonScene)
        {
            SaveLoad.LoadEvent += LoadDungeonState;// Subscrive on the load dungeon interectable objects states
            Debug.Log("subscribed");
        }
    }

    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadDungeonState;// Unsubscribe on the load dungeon interectable objects states
        Debug.Log("Unsubscribed");
    }

    protected virtual void Start()
    {
        
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
                    Debug.Log("Interacted with it");
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
    public virtual void ExecuteTargetActions()// Execute all actions from the targets
    {
        if (Targets.Count > 0)// If there is any target on the list
        {
            foreach (Targetable Target in Targets)// Go through the target list
                if(Target!=null)// If target is not null
                    Target.ExecuteAction();// Execute the target's action
        }
    }
    public virtual void OnUnActuation()
    {
        // Do something//
    }
    public abstract void Actuated();// Function that will do something when actuated(Animation, specific effects...)

    public abstract void LoadDungeonState();// Function that will return the interactable object to the state it was when saved game
}

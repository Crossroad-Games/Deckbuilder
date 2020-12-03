using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class NPC : Interactable
{
    [SerializeField] protected TextAsset DialogueText;
    [SerializeField] protected List<int> DialogueStateList;// Which part of the dialogue will be shown next
    [SerializeField] protected bool HiddenNPC;// This NPC will appear when the players gets close to it
    public int whichDialogue=0;
    protected bool InteractingWithThis=false;// The player is interacting with this NPC
    protected override void Update()
    {
        base.Update();
        if (DialogueBoxUI.activeSelf && Input.GetButtonDown("Dialogue Confirm") && InteractingWithThis)// If the dialogue box is open and the player presses C
        {
            if (whichDialogue + 1 >= DialogueStateList.Count)// If all the dialogue was exhausted
            {
                Player.GetComponent<PlayerMovement>().canMove = true;// Player can move if there is no more dialogue
                DialogueBoxUI.SetActive(false);// Deactivate the dialogue
                Reusable = false;// Dialogue was exhausted
                Used = true;// Can't use it anymore
                Player.transform.Find("Interaction").gameObject.SetActive(false);// Deactivates the Interaction Icon
                InteractingWithThis = false;
                ExecuteTargetActions();// Execute whatever actions are supposed to happen
            }
            else// If there is more dialogue
            { 
                whichDialogue++;// Go to the next dialogue
                ShowDialogue();// Display the text
            }

        }
    }
    protected void ShowDialogue()// This method reads through the referenced file and displays the text based on the current dialogue state
    {
        var TMPText = DialogueBoxUI.GetComponentInChildren<TMP_Text>();// Acquire reference to the dialogue box UI
        TMPText.text = string.Empty;// Clears the text
        var ListofLines = new List<string>();// Each Line of text in the dialogue text asset
        var BeginRecording = false;// Wheter this part of the dialogue is being printed to the box or rejected
        if (DialogueText != null)// If there is a Dialogue Text file 
            ListofLines = DialogueText.text.Split('\n').ToList<string>();// Break it into lines
        else
            Debug.LogError("Can't find the file");
        foreach (string Line in ListofLines)// Go through each Line
        {
            if (Line == "#End")// If there is an End mark, stop recording
                BeginRecording = false;// Stop recording
            if (BeginRecording)// If recording
                TMPText.text += (Line + '\n');// Add this line to the dialogue box
            if (Line == $"#Begin {DialogueStateList[whichDialogue]}")// Start recording at this part of the text
                BeginRecording = true;// Start recording

        }
    }
    public override void Actuated()
    {
        InteractingWithThis = true;
        CanInteract = false;// Can't interact while dialogue is present
        Player.GetComponent<PlayerMovement>().canMove = false;// Player can't move while talking to a NPC
        DialogueBoxUI.SetActive(true);// Turn on the Dialogue Box
        ShowDialogue();// Acquire the text and change the text component text field 
    }
    public override void DetectPlayer()
    {
        base.DetectPlayer();
        if(HiddenNPC)
            transform.Find("Graphic").GetComponent<SpriteRenderer>().enabled = PlayerNearby;// Turn this NPC's sprite on and off based on player's proximity
    }
    public override void LoadDungeonState()
    {
        if (whichDialogue>= DialogueStateList.Count)// If the dialogue has been exhausted
        {
            Reusable = false;// Can't talk to this NPC anymore
            ExecuteTargetActions();// Target's should have been triggered
        }
    }
}

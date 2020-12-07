using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class ShowDialogueBox : Targetable
{
    private GameObject DialogueBox;// Reference will be used to turn the dialogue box on and off, and also control its text
    private GameObject Player;// Reference will be used to access various player's information
    [SerializeField] protected List<Targetable> Targets;// Reference will be used to execute specific actions
    [SerializeField] protected TextAsset DialogueText;
    [SerializeField] protected List<int> DialogueStateList;// Which part of the dialogue will be shown next
    public int whichDialogue;
    private bool InteractingWithThis=false;
    private void Awake()
    {
        DialogueBox = GameObject.Find("Dungeon Canvas").transform.Find("Dialogue").gameObject;// Reference to the button is defined
        Player = GameObject.FindGameObjectWithTag("Player");// Reference is define
    }
    private void Update()
    {
        if (DialogueBox.activeSelf && Input.GetButtonDown("Dialogue Confirm") && InteractingWithThis)// If the dialogue box is open and the player presses C
        {
            if (whichDialogue + 1 >= DialogueStateList.Count)// If all the dialogue was exhausted
            {
                Player.GetComponent<PlayerMovement>().canMove = true;// Player can move if there is no more dialogue
                DialogueBox.SetActive(false);// Deactivate the dialogue
                Player.transform.Find("Interaction").gameObject.SetActive(false);// Deactivates the Interaction Icon
                InteractingWithThis = false;
                DoSomething();// Execute whatever actions are supposed to happen
            }
            else// If there is more dialogue
            {
                whichDialogue++;// Go to the next dialogue
                ShowDialogue();// Display the text
            }

        }
    }
    public override void ExecuteAction()
    {
        DialogueBox.SetActive(true);// Turn on the Dialogue Box GO
        InteractingWithThis = true;
        ShowDialogue();
    }
    private void DoSomething()
    {
        foreach (Targetable Target in Targets)// Go through all the targetables attached to this object
            if (Target != null)// If not null
                Target.ExecuteAction();// Execute its action
    }
    protected void ShowDialogue()// This method reads through the referenced file and displays the text based on the current dialogue state
    {
        var TMPText = DialogueBox.GetComponentInChildren<TMP_Text>();// Acquire reference to the dialogue box UI
        TMPText.text = string.Empty;// Clears the text
        var ListofLines = new List<string>();// Each Line of text in the dialogue text asset
        var BeginRecording = false;// Wheter this part of the dialogue is being printed to the box or rejected
        if (DialogueText != null)// If there is a Dialogue Text file 
            ListofLines = DialogueText.text.Split('\n').ToList<string>();// Break it into lines
        else
            Debug.LogError("Can't find the file");
        foreach (string Line in ListofLines)// Go through each Line
        {
            if (Line.Contains("#End"))// If there is an End mark, stop recording
                BeginRecording = false;// Stop recording
            if (BeginRecording)// If recording
                TMPText.text += (Line + '\n');// Add this line to the dialogue box
            if (Line.Contains($"#Begin {DialogueStateList[whichDialogue]}"))// Start recording at this part of the text
                BeginRecording = true;// Start recording

        }

    }
}

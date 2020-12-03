using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public DungeonPlayerData myData;// Player's Dungeon information such as scene, name and resources
    private SaveLoad Saver;// Saveload reference will be used to save the game when the player starts a dungeon scene
    private GameObject NearInteractableIcon;// This reference will be used to make an icon appear/disappear based on the player's proximity to an interactable object
    private float InteractableIconHeight=0;// This variable will be used to make the icon go up and down above the player's head
    [SerializeField] private float HeightVariation = .05f;// Controls how high/low the icon goes
    [SerializeField] private float HeightVariationSpeed = 2f;// Controls how fast it wobbles up and down
    [SerializeField] private float MediumHeight = .7f;// Controls the middle/starting point of the icon movement
    private void Awake()
    {
        Saver = GameObject.Find("Game Master").GetComponent<SaveLoad>();// Reference to the saveload script is defined
        SaveLoad.LoadEvent += LoadData;// Subscribe this method to the event
        NearInteractableIcon = transform.Find("Interaction").gameObject;// Reference to the icon is defined
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (NearInteractableIcon.activeSelf)// If the Icon is active
        {
            // Assign a new Y position to the icon
            var newHeight = HeightVariation*Mathf.Sin(InteractableIconHeight)+MediumHeight;// Sine function to determine the icon's height
            NearInteractableIcon.transform.localPosition = new Vector3(NearInteractableIcon.transform.localPosition.x, newHeight, NearInteractableIcon.transform.localPosition.z);
            InteractableIconHeight += Time.deltaTime*HeightVariationSpeed;// Height varies with time
        }
    }
    private void LoadData()
    {
        transform.position = DungeonGameData.Current.PlayerPosition;// Syncs the player's position to the one on the save file
        myData = DungeonGameData.Current.PlayerData;// Syncs the player's information to the one on the save file
        myData.Name = PlayerPrefs.GetString("Name");
        Saver.SaveGame();// Save the game when entering a dungeon scene
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadData;// Subscribe this method to the event
    }
    public void NearInteractable()
    {
        if(!NearInteractableIcon.activeSelf)// If the Icon is inactive
            NearInteractableIcon.SetActive(true);// Activate it
    }
    public void LeavingInteractable()
    {
        if (NearInteractableIcon.activeSelf)// If the Icon is active
            NearInteractableIcon.SetActive(false);// Deactivate it
        InteractableIconHeight = 0;// Reset the icon's height control
    }
    public void Interacting()
    {
        Debug.Log("Interacted");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RewardManager : MonoBehaviour
{
    private CardDatabase cardDatabase;
    [Header("UI elements")]
    [SerializeField] private GameObject CardSelectionUI = null;
    [SerializeField] private List<Button> cardOptionsButtons = null;
    [SerializeField] private List<TMP_Text> cardOptionsCosts = null;// Text that will show the card's cost in resources

    #region References
    private CombatPlayer combatPlayer;// Reference to the combat player
    private DungeonPlayer dungeonPlayer; //Reference to the player in the dungeon
    private SaveLoad thisSceneSaveLoad;// Reference to the SaveLoad present in the current scene 
    #endregion

    private Dictionary<Button, CardInfo> cardOptions = new Dictionary<Button, CardInfo>();


    private void Awake()
    {
        //Initialization
        cardDatabase = GameObject.Find("Card Database").GetComponent<CardDatabase>();
        thisSceneSaveLoad = GameObject.Find("Game Master").GetComponent<SaveLoad>();
        //--------------------
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Card Selectiion
    public void FillCombatCardSelection()
    {
        combatPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>(); // finds the combat player
        ClearCardOptions();// Clear the card options 
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        
        for(int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);// r will be a random index for the database, this way we get a random card from the card available in the game
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);//Creates an instance of the card that was randomized
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);// Adds the card instance to the cardOptions, so when the player selects it clicking the button, he will be buying that instance
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;// Change the button image to be the card's image
            cardOptionsCosts[i].text = "Cost: " + cardInfoInstance.ResourceCost;// Change the text UI to show the card's cost
            if (combatPlayer.myData.PlayerLifeForce > cardInfoInstance.ResourceCost)// If the player has enough life/resource
                cardOptionsCosts[i].color = Color.green; //turn the cost color green
            else
                cardOptionsCosts[i].color = Color.red; //otherwise turn it red
            databaseCopy.RemoveAt(r); // Remove the card instance from the available cards so you don't get repeated cards on the "shop"
        }
    }

    public void FillDungeonCardSelection()
    {
        dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>(); // finds the dungeon player
        ClearCardOptions();
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        for (int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;
            cardOptionsCosts[i].text = "Cost: " + cardInfoInstance.ResourceCost;
            if (dungeonPlayer.myData.PlayerLifeForce > cardInfoInstance.ResourceCost)
                cardOptionsCosts[i].color = Color.green;
            else
                cardOptionsCosts[i].color = Color.red;
            databaseCopy.RemoveAt(r);
        }
    }

    public void ClearCardOptions()
    {
        cardOptions.Clear();
        foreach(Button cardOptionButton in cardOptionsButtons)
        {
            cardOptionButton.image.sprite = null;
        }
    }

    public void ChooseCard(int buttonIndex)
    {
        if (thisSceneSaveLoad.CombatScene) // If the player just won a battle
        {
            if (cardOptions[cardOptionsButtons[buttonIndex]] != null) // if the button is correctly filles with a card for the player to buy
            {
                if (combatPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[buttonIndex]].ResourceCost) //If the player can afford the card's cost
                {
                    DungeonGameData.Current.PlayerData.CardCollectionID.Add(cardOptions[cardOptionsButtons[buttonIndex]].ID); //Adds card to player's owned card
                    combatPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[buttonIndex]].ResourceCost; // Effectvely remove the player's resource in exchange for the card
                    cardOptions[cardOptionsButtons[buttonIndex]] = null; //There's no more card attached to the card option button
                    StopSelection(); // Stop the selection
                }
                else
                {
                    Debug.Log("Not enought resource/HP");
                }
            }
            else
                throw new Exception("there is no cardInfo attached to this button");
        }
        else if (thisSceneSaveLoad.DungeonScene)
        {
            dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();
            if (cardOptions[cardOptionsButtons[buttonIndex]] != null)
            {
                if (dungeonPlayer.myData.PlayerLifeForce > cardOptions[cardOptionsButtons[buttonIndex]].ResourceCost)
                {
                    dungeonPlayer.myData.CardCollectionID.Add(cardOptions[cardOptionsButtons[buttonIndex]].ID);
                    dungeonPlayer.myData.PlayerLifeForce -= cardOptions[cardOptionsButtons[buttonIndex]].ResourceCost;
                    cardOptions[cardOptionsButtons[buttonIndex]] = null;
                    StopSelection();
                    dungeonPlayer.GetComponent<PlayerMovement>().canMove = true;
                }
            }
        }
        else
            throw new Exception("not in combat scene neither in dungeon scene");
    }
    
    public void SkipSelection()
    {
        StopSelection(); // Stop the selection
    }

    public void StopSelection()
    {
        if (thisSceneSaveLoad.CombatScene)
        {
            var dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Dungeon");// Saves the information at this locationn
            CardSelectionUI.SetActive(false);// Deactivate the card selection UI
            DungeonGameData.Current.PlayerData.PlayerLifeForce = combatPlayer.GetComponent<CombatPlayer>().myData.PlayerLifeForce;// Updates the dungeon Life Force to be the same as the combat
            string jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
            using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
            {
                streamWriter.Write(jsonString);// Writes the content in json format
            }
            dataPath=Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Combat");// Acquires the path to the combat file
            if (File.Exists(dataPath))// If there is an initial state
                File.Delete(dataPath);// Delete it
            SceneManager.LoadSceneAsync(DungeonGameData.Current.DungeonScene, LoadSceneMode.Single);// Loads the Dungeon Scene
        }
        else if (thisSceneSaveLoad.DungeonScene)
        {
            CardSelectionUI.SetActive(false);
            dungeonPlayer.GetComponent<PlayerMovement>().canMove = true;
        }
    }
    #endregion

}

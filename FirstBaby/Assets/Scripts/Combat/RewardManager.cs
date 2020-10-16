using System;
using System.Collections;
using System.Collections.Generic;
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

    #region References
    private Deck playerDeck;
    #endregion

    private Dictionary<Button, CardInfo> cardOptions = new Dictionary<Button, CardInfo>();


    private void Awake()
    {
        //Initialization
        cardDatabase = GameObject.Find("Card Database").GetComponent<CardDatabase>();
        //--------------------
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Card Selectiion
    public void FillCardSelection()
    {
        ClearCardOptions();
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        Debug.Log(databaseCopy);
        
        for(int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;
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

    public void ChooseCard0()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[0]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[0]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[0]] = null;
        StopSelection();
    }
    public void ChooseCard1()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[1]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[1]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[1]] = null;
        StopSelection();
    }
    public void ChooseCard2()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[2]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[2]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[2]] = null;
        StopSelection();
    }

    public void StopSelection()
    {
        var dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName.Dungeon");// Saves the information at this location
        CardSelectionUI.SetActive(false);// Deactivate the card selection UI
        DungeonGameData.Current.PlayerData.PlayerLifeForce = playerDeck.gameObject.GetComponent<CombatPlayer>().myData.PlayerLifeForce;// Updates the dungeon Life Force to be the same as the combat
        string jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        SceneManager.LoadSceneAsync(DungeonGameData.Current.DungeonScene, LoadSceneMode.Single);// Loads the Dungeon Scene
    }
    #endregion

}

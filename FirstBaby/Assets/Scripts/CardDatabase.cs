using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public List<CardInfo> GameCards;
    public GameObject CardUIGO;// Store this objects reference to be deleted later
    public void ShowCard(int whichCardID)// Show a single card out of the database
    {
        if (CardUIGO != null)//If there is a CardUIGO parent 
        {
            if (CardUIGO.transform.GetChild(0) != null)// If it has a child
                Destroy(CardUIGO.transform.GetChild(0).gameObject);// Destroy it to make room for the next card
        }
        else
        {
            CardUIGO = new GameObject();// Create an empty game object
            CardUIGO.transform.SetParent(GameObject.Find("Dungeon Canvas").transform);// Set it as a child of the canvas
            CardUIGO.transform.localPosition = new Vector3(0, 0, 0);// Center this object
            CardUIGO.name = "CardUIGO";// Set its name
        }
        GameObject cardUI = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + whichCardID), CardUIGO.transform);// Set the temporary GO as its parent
    }
}

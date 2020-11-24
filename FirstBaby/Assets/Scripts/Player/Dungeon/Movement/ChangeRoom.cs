using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    //[SerializeField] private List<GameObject> ControlWalls=new List<GameObject>();
    //[SerializeField] private List<GameObject> ControlFloors = new List<GameObject>();
    //[SerializeField] private List<GameObject> ControlObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> ControlRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> ReverseControlRooms = new List<GameObject>();
    [SerializeField] private List<GameObject> LoadLayerStateList = new List<GameObject>();
    private List<int> ReverseInitialLayerList = new List<int>();// List of the layers each object had before the flip
    private List<int> InitialLayerList = new List<int>();// List of the layers each object had before the flip
    public ListWrapper ListWrapper = new ListWrapper();
    private int iterator;// Used to sync each item to their respective on the previouslayer list when recursevely alternating their states
    private List<Interactable> dungeonInteractables = new List<Interactable>();
    private void Awake()
    {
        dungeonInteractables = GameObject.Find("Game Master").GetComponent<InteractableDatabase>().InsteractablesInScene;
        foreach (GameObject Room in ControlRooms)// Cycle through all the objects this component will change layers
            if (Room != null)// If component not null
                StoreLayerRecursively(Room, false);// Store its initial layer
        foreach (GameObject Room in ReverseControlRooms)// Cycle through all the objects this component will change layers
            if (Room != null)// If component not null
                StoreLayerRecursively(Room, true);// Store its initial layer
        ListWrapper.LayerList = new List<int>();
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")// If the player just triggered this door
        {
            var whichLayer = 0;
            var reverseWhichLayer = 0;
            iterator = 0;// Reset the iterator value
            ListWrapper.LayerList.Clear();
            if (transform.parent.rotation.eulerAngles.y == 180 || transform.parent.rotation.eulerAngles.y == -180 || transform.parent.rotation.eulerAngles.y == 0)
            {
                whichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 29 : 0;// If the player has a higher Z value, wall is behind him, fade it
                reverseWhichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 0 : 29;
            }
            else
            {
                whichLayer = (other.gameObject.transform.position.x - this.transform.position.x) > 0 ? 0 : 29;// If the player has a higher Z value, wall is behind him, fade it
                reverseWhichLayer = (other.gameObject.transform.position.x - this.transform.position.x) > 0 ? 29 : 0;
            }
            #region Ghost Room
            foreach (GameObject Room in ControlRooms)// For each Room gameRoom tied to this controller
                if(Room!=null)
                    SetLayerRecursively(Room, whichLayer, false);
            iterator = 0;
            foreach (GameObject Room in ReverseControlRooms)
                if (Room != null)
                    SetLayerRecursively(Room, reverseWhichLayer, true);
            #endregion
            foreach (GameObject Room in LoadLayerStateList)
                if (Room != null)
                    StoreLayerRecursively(Room, false, true);
            LevelGameData.Current.ObjectsLayer = ListWrapper;// Copy this ListWrapper
            iterator = 0;
            foreach(Door Door in dungeonInteractables)// Go through each Door in the interactables list
            {
                if (Door == this.transform.parent.GetComponentInChildren<Door>())// If this Door is the same as the one on the list
                {
                    LevelGameData.Current.whichDoor = iterator;// Store this door position on the list
                    break;// Stop checking each door
                }
                else
                    iterator++;// Increment
            }
            /*
            #region Ghost Walls
            whichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 29 : 30;// If the player has a higher Z value, wall is behind him, fade it
            foreach (GameObject Wall in ControlWalls)// For each Wall gameobject tied to this controller
            {
                if (Wall == null)
                    continue;
                Wall.layer = whichLayer;// Change its layer
                foreach (Transform Child in Wall.GetComponentInChildren<Transform>(true))// Access its children
                    Child.gameObject.layer = whichLayer;// Change their layer
                
            }
            foreach (Transform Child in transform.parent.GetComponentInChildren<Transform>(true))//For each children inside this objects parent 
                Child.gameObject.layer = whichLayer;// Change its layer
            transform.parent.gameObject.layer = whichLayer;// Change its layer
            #endregion
            #region Ghost Floors
            whichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 29 : 31;// If the player has a higher Z value, wall is behind him, fade it
            foreach (GameObject Floor in ControlFloors)// For each Floor gameobject tied to this controller
            {
                if (Floor == null)
                    continue;
                Floor.layer = whichLayer;// Change its layer
                foreach (Transform Child in Floor.GetComponentInChildren<Transform>(true))// Access its children
                    Child.gameObject.layer = whichLayer;// Change their layer

            }
            #endregion
            #region Ghost Objects
            whichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 29 : 0;// If the player has a higher Z value, wall is behind him, fade it
            foreach (GameObject Object in ControlObjects)// For each Object gameobject tied to this controller
            {
                if (Object == null)
                    continue;
                Object.layer = whichLayer;// Change its layer
                foreach (Transform Child in Object.GetComponentInChildren<Transform>(true))// Access its children
                    Child.gameObject.layer = whichLayer;// Change their layer

            }
            #endregion
            */
        }
    }
    public void SetLayerRecursively(GameObject obj, int newLayer, bool isReverse)
    {
        Debug.Log("Setting Layers");
        if (null == obj)
        {
            return;
        }
        if (newLayer != 0)// If not going to default
            obj.layer = newLayer;// Ghost this layer
        else
        {
            if (!isReverse)
                obj.layer = InitialLayerList[iterator];
            else
                obj.layer = ReverseInitialLayerList[iterator];
            iterator++;
        }

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer, isReverse);
        }
    }
    void SetLayerRecursively(GameObject obj)
    {
        if (null == obj)
        {
            return;
        }
        if (ListWrapper.LayerList.Count == iterator)
            return;
        Debug.Log("Setting Layer: " + ListWrapper.LayerList[iterator]);
        obj.layer = ListWrapper.LayerList[iterator];
        iterator++;
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject);
        }
    }
    void StoreLayerRecursively(GameObject obj, bool isReverse)
    {
        if (null == obj)
        {
            return;
        }
        if (!isReverse)
            InitialLayerList.Add(obj.layer);// Store this object's layer
        else
            ReverseInitialLayerList.Add(obj.layer);// Store this object's layer
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }

            StoreLayerRecursively(child.gameObject,isReverse);
        }
    }
    void StoreLayerRecursively(GameObject obj, bool isReverse, bool ToSave)
    {
        if (null == obj)
        {
            return;
        }
        ListWrapper.LayerList.Add(obj.layer);
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            StoreLayerRecursively(child.gameObject,isReverse, ToSave);
        }
    }
    public void LoadList(ListWrapper LoadedList)
    {
        
        iterator = 0;
        if (LoadedList.LayerList != null)
            ListWrapper = LoadedList;
        else
        {
            Debug.Log("No Layer List");
            return;
        }
        foreach (GameObject Room in LoadLayerStateList)
            if (Room != null)
                SetLayerRecursively(Room);
            else
                Debug.Log("No Rooms in the LoadList");
        
    }
}

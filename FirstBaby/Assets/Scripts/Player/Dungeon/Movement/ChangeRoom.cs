using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    [SerializeField] private List<GameObject> ControlWalls=new List<GameObject>();
    [SerializeField] private List<GameObject> ControlFloors = new List<GameObject>();
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")// If the player just triggered this door
        {
            #region Ghost Walls
            var whichLayer = (other.gameObject.transform.position.z - this.transform.position.z) > 0 ? 29 : 30;// If the player has a higher Z value, wall is behind him, fade it
            foreach (GameObject Wall in ControlWalls)// For each Wall gameobject tied to this controller
            { 
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
                Floor.layer = whichLayer;// Change its layer
                foreach (Transform Child in Floor.GetComponentInChildren<Transform>(true))// Access its children
                    Child.gameObject.layer = whichLayer;// Change their layer

            }
            #endregion
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    public int ActionID { get; protected set; }// Unique identifier of this Action
    public string ActionName { get; protected set; }
    public string Description { get; protected set; }
    public abstract void Effect();// Every Action must have an Effect
    private EnemyClass myclass;
    public EnemyClass myClass// Keeps reference to which Enemy Class has ownership over this script
    {
        get
        {
            myclass = myclass?? gameObject.GetComponent<EnemyClass>();// If this field is currently null, get the reference from the object attached to
            return myclass;// Return the reference
        }
    }
}

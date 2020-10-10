using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTarget
{
    public Vector3 position;
    public Quaternion rotation;

    public CardTarget(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositionToFollow
{
    public Vector3 position;
    public Quaternion rotation;

    public CardPositionToFollow(Vector3 pos, Quaternion rot) //this is the position entity that a card in hand will follow for it's movement
    {
        position = pos;
        rotation = rot;
    }
}

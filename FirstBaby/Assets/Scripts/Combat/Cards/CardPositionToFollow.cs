﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositionToFollow
{
    public Vector3 position;
    public Quaternion rotation;

    public CardPositionToFollow(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newCombatProperties", menuName = "Data/Combat/Combat Properties Asset")]
public class CombatProperties : ScriptableObject
{
    public float cardDrawingSpeed = 4f;
    public float cardRotationSpeed = 1f;
    public float offsetBetweenCards = 0.4f;
    public float cardsHeightDiff = 0.5f;
    public float angleBetweenCards = 10f;
    public float cardHighlightScale = 0.5f;
    public float cardNormalScale = 0.4f;
    public float zAxisOffsetWhenCardDrag = -1f;
    public float HighlightHeight = -2f;
}

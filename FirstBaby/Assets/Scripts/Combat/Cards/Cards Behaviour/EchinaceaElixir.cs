using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchinaceaElixir : NonTargetCard
{
    public override IEnumerator CardEffect()
    {
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericDefense/GenericDefense"), new Vector3(-1.92f, 0.25f, -0.23f), Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericDefenseEffect>().card = this;
        return base.CardEffect();
    }

    public override void DealEffect()
    {
        effectFinished = true;
    }

    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseShield = 16;
                break;
            case 1:// One LVL higher than base
                BaseShield = 30;
                break;
            case 2:// Two LVLs higher than base
                BaseShield = 45;
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Ward {thisVirtualCard.CalculateAction(BaseShield)}\n\nFallout:\n        Ward 8\nWall 5";
        Debug.LogWarning("Needs to rework Echinaceas Fallout to change its lvl up and text");
    }
}

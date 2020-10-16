using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private float SlideSpeed = 1f;
    public override void Actuated()
    {
        StartCoroutine(SlideDown());
    }
    IEnumerator SlideDown()
    {
        float newHeight=transform.position.y;
        while(transform.position.y>-4.5f)// If the door's height is not yet -20
        {
            newHeight -= Time.deltaTime*SlideSpeed;// Slide down based on time
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);//  Update the transform
            yield return null;// Return next frame
        }
        ExecuteTargetActions();// If there are any targets, execute their actions
        yield break;
    }
}

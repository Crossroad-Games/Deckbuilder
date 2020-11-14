using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectTest2 : MonoBehaviour
{
    public AlchemistFireCard card;
    public Transform targetTransform;
    private Animator anim;
    private bool moveToTarget;
    public float orbMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveToTarget)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, orbMovementSpeed * Time.deltaTime); // Move the orb to enemy position
            if((transform.position - targetTransform.position).magnitude <= 0.2f)
            {
                //card.DealEffect();
                anim.SetTrigger("ActivateExplosion");
            }
        }
    }

    public void beginMovement()
    {
        moveToTarget = true;
    }

    public void ByeBye()
    {
        Destroy(this.gameObject);
    }
}

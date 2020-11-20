using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttackKeywordEffect : MonoBehaviour
{
    public VirtualCard virtualCard;
    public Transform targetTransform;
    private Animator anim;
    public GameObject cardUI;
    private bool moveToTarget;
    private bool actuated;
    public bool dealEffect;
    public float orbMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        actuated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToTarget)
        {
            if (targetTransform != null)
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, orbMovementSpeed * Time.deltaTime); // Move the orb to enemy position
            if ((transform.position - targetTransform.position).magnitude <= 0.2f && !actuated)
            {
                actuated = true;
                if (dealEffect)
                {
                    foreach(KeyValuePair<string, VirtualCardExtension> extensionEffect in virtualCard.virtualCardExtensions)
                    {
                        extensionEffect.Value.DealEffect();
                    }
                    ByeByeCardUI();
                }
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

    public void ByeByeCardUI()
    {
        Destroy(cardUI);
    }
}

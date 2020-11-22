using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool InitialState = true;// Which interactable state does this object start on? Can you interact with it? Or only if some condition happens?
    void Start()
    {
      //  this.GetComponent<Interactable>
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

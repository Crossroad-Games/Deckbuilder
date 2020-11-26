using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool InitialState = true;// Which interactable state does this object start on? Can you interact with it? Or only if some condition happens?
    [SerializeField] private List<Interactable> InteractToUnlock = new List<Interactable>();
    void Start()
    {
        this.GetComponent<Interactable>().CanInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Interactable>().CanInteract = true;
        foreach (Interactable Key in InteractToUnlock)
            if(Key!=null)
            {
                if(Key.Used==false)
                {
                    this.GetComponent<Interactable>().CanInteract = false;
                    break;
                }
            }
    }
}

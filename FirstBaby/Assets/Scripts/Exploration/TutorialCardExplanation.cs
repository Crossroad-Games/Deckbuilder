using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialCardExplanation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<GameObject> OtherTutorialBoxes;
    private GameObject Explanation;// Reference to the extended explanation text
    void Awake()
    {
        Explanation = transform.Find("Extended Explanation").gameObject;// Reference is defined
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject Box in OtherTutorialBoxes)// Cycle through each other tutorial box
            if (Box != null)// If box is not null
                Box.SetActive(false);// Deactivate it
        Explanation.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject Box in OtherTutorialBoxes)// Cycle through each other tutorial box
            if (Box != null)// If box is not null
                Box.SetActive(true);// Activate it
        Explanation.SetActive(false);
    }
    
}

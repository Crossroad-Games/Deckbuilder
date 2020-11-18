using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private CombatPlayer combatPlayer;
    private Color shieldColor;
    public Material shieldMaterial;


    private void Awake()
    {
        combatPlayer = GetComponentInParent<CombatPlayer>();
        shieldMaterial = GetComponent<MeshRenderer>().material;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

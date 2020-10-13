using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer=0;
    public bool mouseOverEnemy;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMouseOnEnemy())
        {
            mouseOverEnemy = true;
        }
        else
        {
            mouseOverEnemy = false;
        }
    }

    public EnemyClass isMouseOnEnemy()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero, 15f, enemyLayer);

        if(hitInfo.collider != null)
        {
            EnemyClass enemy = hitInfo.collider.gameObject.GetComponent<EnemyClass>();
            if (enemy != null)
            {
                return enemy;
            }
        }
        return null;
    }
}

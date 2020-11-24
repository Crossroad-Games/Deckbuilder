using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer=0;
    public Camera camera2;
    public bool mouseOverEnemy;
    private Bezier bezierArrowCurve;

    
    // Start is called before the first frame update
    void Start()
    {
        bezierArrowCurve = GetComponent<Bezier>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMouseOnEnemy())
        {
            mouseOverEnemy = true;
            bezierArrowCurve.lineRenderer.startColor = bezierArrowCurve.lineRenderer.startColor;
            bezierArrowCurve.lineRenderer.endColor = new Color(255, 0, 0);
        }
        else
        {
            mouseOverEnemy = false;
            bezierArrowCurve.lineRenderer.startColor = bezierArrowCurve.lineRenderer.startColor;
            bezierArrowCurve.lineRenderer.endColor = new Color(220, 30, 30);
        }
    }

    public EnemyClass isMouseOnEnemy()
    {
        /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero, 15f, enemyLayer);*/
        Ray ray = camera2.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.collider != null)
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

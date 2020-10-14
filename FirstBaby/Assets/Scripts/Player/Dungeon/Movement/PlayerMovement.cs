using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private Rigidbody playerRB;

    [Header("Input")]
    [Range(-1, 1)] [SerializeField] private float inputH;
    [Range(-1, 1)] [SerializeField] private float inputV;

    #region Checks
    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region Booleans
    public bool canMove = true;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();//gets the player's rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        myInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 directionToMove = new Vector3(inputH, 0f, inputV);
        directionToMove = Vector3.ClampMagnitude(directionToMove, 1);
        if (isGrounded && canMove) // conditions for player to be able to move
        {
            playerRB.velocity = directionToMove * speed;
        }
    }

    private void myInput()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
    }


    //Ground Checks
    private void OnCollisionStay(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (groundLayer == (groundLayer | (1 << layer)))  //  returns true if the collision.contact layer is groundLayer
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (groundLayer == (groundLayer | (1 << layer)))  //  returns true if the collision.contact layer is groundLayer
        {
            isGrounded = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public static bool isPlayer = true;
    private float mx;

    // Update is called once per frame
    private void Update()
    {
        mx = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() 
    {
        if (isPlayer == true) 
        {
            Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
            rb.velocity = movement;
        }    
    }
}

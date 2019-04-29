using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{


    Rigidbody2D rigidbody;

    public float jumpPower;
    public bool isJumping;

    public float fSpeed;

    public Vector2 direction;

    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        isJumping = false;
    }
    
    float jumpTimer = 0.0f;

    Vector2 jumpDir;

    public LayerMask ground;
    public bool isGrounded = true;

    private void Update() 
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        direction.Set(h, v);

        rigidbody.velocity = new Vector2(direction.x * fSpeed, rigidbody.velocity.y);
        

        if(isGrounded == true && Input.GetKeyDown(KeyCode.X))
        {
            jumpDir = direction;
            isJumping = true;
        }

        if(isJumping)
        {   
            jumpTimer += Time.deltaTime;
            if(jumpTimer >= 0.2f)
            {
                isJumping = false;
                jumpTimer = 0f;
            }
            rigidbody.velocity = new Vector2(rigidbody.velocity.x + jumpDir.x * fSpeed, Vector2.up.y * jumpPower);
        }

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 100, ground);
        // Debug.DrawLine(transform.position, hit.point, Color.red);

        // float distance = Vector2.Distance(hit.point, transform.position);

        // if(!isGrounded)
        // {
        //     if(distance > 0.04f)
        //     {
        //         rigidbody.velocity = new Vector2(rigidbody.velocity.x + jumpDir.x * fSpeed, Vector2.up.y * jumpPower);
        //     }
        //     else
        //     {   
        //         isGrounded = true;
        //         rigidbody.velocity = Vector2.zero;
        //     }
        // }
    }

    private void FixedUpdate() 
    {
        
    }



}
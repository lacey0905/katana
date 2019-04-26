using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    Rigidbody2D rigidbody;


    public float jumpTime;
    public float jumpTimeCounter;

    public bool isJumping = false;
    public bool isGrounded = true;

    public LayerMask ground;

    public float jumpPower;

    public Vector2 direction;

    public GameObject feetPos;

    public float fSpeed;

    private void Start() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    IEnumerator Dash()
    {
        rigidbody.gravityScale = 0;
        dash = true;
        dashDir = direction;
        yield return new WaitForSeconds(0.1f);
        rigidbody.gravityScale = 5f;
        dash = false;
    }

    bool dash = false;
    Vector2 dashDir;

    private void Update() 
    {

        RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position, -Vector2.up, 100, ground);
        Debug.DrawLine(feetPos.transform.position, hit.point, Color.red);
        
        float distance = Vector2.Distance(hit.point, feetPos.transform.position);

        // Debug.Log(distance);

        if(isGrounded == true && Input.GetKeyDown(KeyCode.Z))
        {
            
            // rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpPower);

            Vector2 jumpDir = new Vector2(direction.x * (jumpPower / 2f), jumpPower);

            rigidbody.AddForce(jumpDir, ForceMode2D.Impulse);
            
        }

        if(dash)
        {
            transform.Translate(dashDir * 20f * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            rigidbody.velocity = Vector2.zero;
            StartCoroutine("Dash");


            // transform.position = Vector2.MoveTowards(transform.position, transform.position * direction, 10f * Time.deltaTime);

            // Vector2 jumpDir = new Vector2(direction.x * (jumpPower / 2f), jumpPower);
            // rigidbody.AddForce(jumpDir, ForceMode2D.Impulse);
        }

        if(distance > 0.04f)
        {
            isJumping = true;
            isGrounded = false;
        }
        else
        {   
            if(isJumping)
            {
                rigidbody.velocity = Vector2.zero;
                isJumping = false;
            }
            isGrounded = true;
        }

        // if(Input.GetKeyDown(KeyCode.Space))
        // {
            
        //     jumpTimeCounter = jumpTime;
        //     // rb.velocity = Vector2.up * jumpForce;
        //     rb.AddForce(Direction * jumpForce, ForceMode2D.Impulse);
        //     StartCoroutine("Jump");
        // }

        velocity = rigidbody.velocity;

    }

    public Vector2 velocity;


    private void FixedUpdate() 
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        direction.Set(h,v);

        if(isGrounded)
        {
            rigidbody.velocity = new Vector2(h * fSpeed, rigidbody.velocity.y);
        }
        else 
        {
            rigidbody.AddForce(new Vector2(direction.x * fSpeed * 5f, rigidbody.velocity.y));
        }

        

        

        // Debug.Log(rb.velocity);
        
    }

    /* 
        점프를 했을때 -> 좌우 속도는 현재 속도에서 가감
        점프가 아닐때는 직접 넣기
    */

    // bool GroundCheck = true;

    // private void Update()
    // {
        
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     // GroundCheck = false;
        //     // rb.velocity = Vector2.zero;
        //     Vector2 temp = new Vector2(rb.velocity.x, v);
        //     rb.AddForce(temp * jumpForce, ForceMode2D.Impulse);
        // }

        // isGrounded= Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        // if(isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        // {
        //     isJumping = true;
        //     jumpTimeCounter = jumpTime;
        //     // rb.velocity = Vector2.up * jumpForce;
        //     rb.AddForce(Direction * jumpForce, ForceMode2D.Impulse);
        //     StartCoroutine("Jump");
        // }

        // if(!GroundCheck)
        // {
        // }
    

        // if(Input.GetKey(KeyCode.Space) && isJumping == true)
        // {
        //     if(jumpTimeCounter > 0)
        //     {
        //         rb.velocity = Vector2.up * jumpForce;
        //         jumpTimeCounter -= Time.deltaTime;
        //     }
        //     else 
        //     {
        //         isJumping = false;
        //     }
        // }

        // if(Input.GetKeyUp(KeyCode.Space))
        // {
        //     isJumping = false;
        // }

    // }








    // public float m_fSpeed = 5f;
    // Rigidbody Rigidbody;
    // Vector3 movement;

    // public Animator Anim;

    // private void Awake() 
    // {
    //     Rigidbody = GetComponent<Rigidbody>();
    // }

    // bool isAttack = false;

    // private void FixedUpdate() 
    // {
    //     float h = 0;

    //     if(!isAttack)
    //     {
    //         h = Input.GetAxisRaw("Horizontal");
    //         movement.Set(h, 0, 0);
    //         movement = movement.normalized * m_fSpeed * Time.deltaTime;
    //         Rigidbody.MovePosition(transform.position + movement);
    //     }

    //     if(h != 0)
    //     {
    //         Anim.SetBool("Run", true);
    //         if(h > 0) {
    //             Anim.GetComponent<SpriteRenderer>().flipX = false;
    //         } 
    //         else {
    //             Anim.GetComponent<SpriteRenderer>().flipX = true;
    //         }
    //     } 
    //     else 
    //     {
    //         Anim.SetBool("Run", false);
    //     }



    // }

    
    

    
}

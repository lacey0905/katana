using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    Rigidbody2D rigidbody;

    public bool isGround;
    public LayerMask groundLayers;

    float skinWidth = 0.015f;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();	
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate() 
    {
        UpdateRaycastOrigins();
        // VerticalCollisions(rigidbody.velocity);
        // isGround = Physics2D.OverlapArea(raycastOrigins.bottomLeft, raycastOrigins.bottomRight, groundLayers);
        // if(isGround)
        // {
        //     rigidbody.gravityScale = 0f;
        //     rigidbody.velocity = Vector2.zero;
        // }
        // else
        // {
        //     rigidbody.gravityScale = 2f;
        // }

        
        
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    // }

    // private void OnTriggerEnter2D(Collider2D other) {
        
    // }

    // private void OnTriggerStay2D(Collider2D other) {
    //     rigidbody.gravityScale = 0f;
    //     rigidbody.velocity = Vector2.zero;
    // }

    // private void OnTriggerExit2D(Collider2D other) {
        
    // }


    // void VerticalCollisions(Vector3 velocity)
	// {

        

	// 	float directionY = Mathf.Sign(velocity.y);
	// 	float rayLength = Mathf.Abs(velocity.y) + skinWidth;

	// 	Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
		
	// 	RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, 0.5f, groundLayers);
		
	// 	Debug.DrawRay(rayOrigin, Vector2.up * directionY * 0.5f, Color.red);
		
	// 	if(hit)
	// 	{   
    //         Debug.Log(velocity);
    //         Vector2 newVelocity = new Vector2(rigidbody.velocity.x, 0f);
	// 		rigidbody.velocity = newVelocity;
	// 		rayLength = hit.distance;
	// 	}
	// }

    struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

    void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		// bounds.Expand(skinWidth * -2);

		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

	}



    void OnDrawGizmos() {
        Gizmos.color = new Color(0,1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.505f), new Vector2(1, 0.01f));    
    }
}

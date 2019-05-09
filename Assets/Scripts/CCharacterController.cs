using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    Rigidbody2D rigidbody;

    public bool isGround;
    public LayerMask groundLayers;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();	
        rigidbody = GetComponent<Rigidbody2D>();
    }


    public Vector3 velocity = Vector3.zero;
    public float fSpeed = 5.0f;
    public float jumpPower = 10f;

    void Update() 
    {
        UpdateRaycastOrigins();
        isGround = Physics2D.OverlapArea(raycastOrigins.bottomLeft, raycastOrigins.bottomRight, groundLayers);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);

        velocity.x = input.x * fSpeed;
        rigidbody.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Jump");
            if(isGround)
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Dash");
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(input.normalized * jumpPower, ForceMode2D.Impulse);
        }

    }

    struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

    void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

	}

    void OnDrawGizmos() {
        Gizmos.color = new Color(1f,0, 0, 1f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.505f), new Vector2(1, 0.01f));    
    }
}

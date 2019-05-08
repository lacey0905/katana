using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController : MonoBehaviour 
{

	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	
	public LayerMask collisionMask;

	const float skinWidth = 0.015f;

	void Awake() 
	{
		collider = GetComponent<BoxCollider2D>();	
	}

	public void Move(Vector3 velocity)
	{

		UpdateRaycastOrigins();
		// collisions.Reset();
        // collisions.velocityOld = velocity;

		if(velocity.y != 0)
		{
			VerticalCollisions(ref velocity);
		}

		
		transform.Translate(velocity);
	}


	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		
		float directionY = Mathf.Sign(velocity.y);

		Debug.Log(velocity.y);

		collisions.above = directionY == 1;

		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
		
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
		
		Debug.DrawRay(rayOrigin, Vector2.up * directionY * 1f, Color.red);


		
		
		if(hit)
		{

			collisions.isGround = directionY == 1;

			


			velocity.y = (hit.distance - skinWidth) * directionY;
			rayLength = hit.distance;

			collisions.below = directionY == 1;
			// collisions.above = directionY == 1;

		}
		else
		{
			if(directionY == 1)
			{
				collisions.isGround = false;
			}
		}
	}

	void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

	}

	public CollisionInfo collisions;

	public struct CollisionInfo
    {

		public bool isGround;

        // 그라운드 체크
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            
            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }
    }

}

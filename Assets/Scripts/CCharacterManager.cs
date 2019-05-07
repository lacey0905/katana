using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterManager : MonoBehaviour 
{

	public float jumpHeight = 4.0f;
	public float jumpTime = 0.4f;

	public float moveSpeed = 6.0f;

	float gravity;
	float jumpVelocity;

	Vector3 velocity;

	CController controller;

	void Awake() 
	{
		controller = GetComponent<CController>();	
	}

	void Start () 
	{
		gravity = -(2 * jumpHeight) / Mathf.Pow(jumpTime, 2);
		jumpVelocity = Mathf.Abs(gravity) * jumpTime;
	}
	
	void Update () 
	{

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(controller.collisions.above)
        {
            velocity.y = 0;
			velocity.x = input.x * moveSpeed;
			Debug.Log(controller.collisions.above);
        }
		
		

		// Debug.Log(controller.collisions.below);

		if(controller.collisions.below)
		{
			
		}
		// else 
		// {
		// 	
		// }

		if (Input.GetKeyDown(KeyCode.Z))
        {
            velocity.y = jumpVelocity;
			
			if(input.x != 0)
			{
				velocity.x = 10f * input.x;
			}
        }

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
		
	}
}

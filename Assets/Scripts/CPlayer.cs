using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CController2D))]
public class CPlayer : MonoBehaviour
{

    // 점프 높이
    public float jumpHeight = 4.0f;
    // 점프 유지 시간
    public float timeToJumpApex = 0.4f;

    /// <summary>
    /// d = vit + at^2 / 2
    /// jumpHeight(점프거리) = gravity(초기속도) * timeToJumpApex^2(점프 유지 시간^2) / 2
    /// gravity = 2 * jumpHeight / timeToJumpApex^2
    /// </summary>

    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6f;

    float gravity;
    float jumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    CController2D controller;


    void Awake() {
        controller = GetComponent<CController2D>();
    }

    void Start()
    {
        // 점프 중력 구하는 수식 : 복습해야함
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        Debug.Log("Gravity : " + gravity + "Jump Velocity : " + jumpVelocity);
    }

    void Update()
    {

        if(controller.coliisions.above || controller.coliisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Z) && controller.coliisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.coliisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CController2D))]
public class CPlayer : MonoBehaviour
{
    float moveSpeed = 6f;
    float gravity = -20f;
    Vector3 velocity;

    CController2D controller;


    void Awake() {
        controller = GetComponent<CController2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

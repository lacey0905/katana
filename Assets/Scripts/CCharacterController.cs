using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    public float m_fSpeed = 10f;
    Rigidbody Rigidbody;
    Vector3 movement;


    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");

        movement.Set(h, 0, 0);
        movement = movement.normalized * m_fSpeed * Time.deltaTime;

        Rigidbody.MovePosition(transform.position + movement);
    }


}

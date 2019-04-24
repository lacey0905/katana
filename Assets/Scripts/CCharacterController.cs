using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    
 
    // public LayerMask TargetMask;    //Enemy 레이어마스크 지정을 위한 변수
    // public LayerMask ObstacleMask;  //Obstacle 레이어마스크 지정 위한 변수
 
    


    public float m_fSpeed = 5f;
    Rigidbody Rigidbody;
    Vector3 movement;

    public Animator Anim;

    private void Awake() 
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    bool isAttack = false;

    private void FixedUpdate() 
    {
        float h = 0;

        if(!isAttack)
        {
            h = Input.GetAxisRaw("Horizontal");
            movement.Set(h, 0, 0);
            movement = movement.normalized * m_fSpeed * Time.deltaTime;
            Rigidbody.MovePosition(transform.position + movement);
        }

        if(h != 0)
        {
            Anim.SetBool("Run", true);
            if(h > 0) {
                Anim.GetComponent<SpriteRenderer>().flipX = false;
            } 
            else {
                Anim.GetComponent<SpriteRenderer>().flipX = true;
            }
        } 
        else 
        {
            Anim.SetBool("Run", false);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            slide = true;
            isAttack = true;
            Anim.SetBool("Attack", true);
            StartCoroutine("Attack");
        }


        if(slide)
        {
            transform.Translate(movement * 20f * Time.deltaTime);
        }

    }

    bool slide = false;
     
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.0f);
        isAttack = false;
        Anim.SetBool("Attack", false);
        slide = false;
    }
    

    
}

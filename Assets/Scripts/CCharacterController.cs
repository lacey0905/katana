using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{

    
 
    // public LayerMask TargetMask;    //Enemy 레이어마스크 지정을 위한 변수
    // public LayerMask ObstacleMask;  //Obstacle 레이어마스크 지정 위한 변수
 
    private Transform _transform;


    public float m_fSpeed = 10f;
    Rigidbody Rigidbody;
    Vector3 movement;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");

        movement.Set(h, 0, 0);
        movement = movement.normalized * m_fSpeed * Time.deltaTime;

        Rigidbody.MovePosition(transform.position + movement);
    }
     
    void Update ()
    {
        DrawView();             //Scene뷰에 시야범위 그리기
        FindVisibleTargets();   //Enemy인지 Obstacle인지 판별


    }

    private float ViewAngle = 30f;    //시야각
    private float ViewDistance = 3f; //시야거리
 
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //탱크의 좌우 회전값 갱신
        angleInDegrees += transform.eulerAngles.z;
        //경계 벡터값 반환
        // return new Vector3(0, Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
 
    public void DrawView()
    {
        Vector3 leftBoundary = DirFromAngle(-ViewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(ViewAngle / 2);
        Debug.DrawLine(_transform.position, _transform.position + leftBoundary * ViewDistance, Color.blue);
        Debug.DrawLine(_transform.position, _transform.position + rightBoundary * ViewDistance, Color.blue);
    }
 
    int m_Mask = 1 << 5;

    void OnDrawGizmos()  
    {  
        Gizmos.matrix = transform.localToWorldMatrix;  
        Gizmos.color = Color.yellow;  
        Gizmos.DrawWireSphere(Vector3.zero, ViewDistance);  
    } 

    public void FindVisibleTargets()
    {
        //시야거리 내에 존재하는 모든 컬라이더 받아오기
        Collider[] targets = Physics.OverlapSphere(_transform.position, ViewDistance);

        // Debug.Log(targets.Length);

        for (int i = 0; i < targets.Length; i++)
        {
            if(targets[i].tag != "Player"){
                continue;
            }
            Transform target = targets[i].transform;
 
            //탱크로부터 타겟까지의 단위벡터
            // Vector3 dirToTarget = (target.position - _transform.position).normalized;

            Vector3 leftBoundary = DirFromAngle(-ViewAngle / 2);
            Vector3 rightBoundary = DirFromAngle(ViewAngle / 2);

            Vector3 top = ((_transform.position + leftBoundary * ViewDistance) - transform.position).normalized;
            Vector3 bot = ((_transform.position + rightBoundary * ViewDistance) - transform.position).normalized;
            
            float dot = Vector3.Dot(top, bot);

            Vector3 targetDir = (targets[i].transform.position - transform.position).normalized;

            float targetDot = Vector3.Dot(targetDir, transform.right);


            if (dot < targetDot)
            {
                Debug.Log("a");
            }

            // Vector3 top = ((_transform.position + leftBoundary * ViewDistance) - _transform.position).normalized;
            // Vector3 top = (targets[i].transform.position - _transform.position).normalized;

            // float dot = Vector3.Dot(top, transform.right);

            // Debug.DrawLine(, , Color.blue);
            // Debug.DrawLine(_transform.position, _transform.position + rightBoundary * ViewDistance, Color.blue);


            // Debug.Log(dot);

            // Debug.DrawLine(target.position, _transform.position, Color.red);


            // Debug.Log(Vector3.Dot(_transform.position, dirToTarget));
            
            
            // if (dot < Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad))
            // {
            //     // Debug.Log(Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad));
            //     // Debug.Log(Vector2.Dot(_transform.right, dirToTarget));
            // }
 
            //_transform.forward와 dirToTarget은 모두 단위벡터이므로 내적값은 두 벡터가 이루는 각의 Cos값과 같다.
            //내적값이 시야각/2의 Cos값보다 크면 시야에 들어온 것이다.
            // if (Vector3.Dot(_transform.right, dirToTarget) > Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad))
            // //if (Vector3.Angle(_transform.forward, dirToTarget) < ViewAngle/2)
            // {
            //     float distToTarget = Vector3.Distance(_transform.position, target.position);
 
            //     if (!Physics.Raycast(_transform.position, dirToTarget, distToTarget))
            //     {
            //         Debug.DrawLine(_transform.position, target.position, Color.red);
            //     }
            // }   
        }
    }

}

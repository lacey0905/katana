using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttack : MonoBehaviour
{
    public Collider[] colls;  
    public Vector3 boxSize = new Vector2(1f, 1f);  
    
    Vector2 pos;

    void Update () {  

        pos = new Vector2(transform.position.x, transform.position.y);

        colls = Physics.OverlapBox(transform.position, boxSize, transform.rotation);  

        if(colls.Length > 0)
        {
            Debug.Log("Hit");
        }
    }  
  
    void OnDrawGizmos()  
    {  
        Gizmos.matrix = transform.localToWorldMatrix;  
        Gizmos.color = Color.yellow;  
        Gizmos.DrawWireCube(Vector3.zero, boxSize);  
    } 

}



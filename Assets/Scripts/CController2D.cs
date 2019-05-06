using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class CController2D : MonoBehaviour
{   

    public LayerMask collisionMask;
    
    // 콜라이더가 겹칠 수 있는 범위 -> 여백이 없으면 움직이지 못함
    // 자세히 알아봐야함
    const float skinWidth = 0.015f;

    // 레이를 가로, 세로 몇개로 지정 할 것인가
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    // 레이 발사 지점 간격
    float horizontalRaySpacing;
    float verticalRaySpacing;

    float maxClimbAngle = 80f;
    float maxDescendAngle = 75f;

    // 콜라이더
    BoxCollider2D collider;
    // 레이 발사 시작 지점
    RaycastOrigins raycastOrigins;

    public ColiisionInfo coliisions;

    void Awake() 
    {
        collider = GetComponent<BoxCollider2D>();
    }

    void Start() 
    {
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        coliisions.Reset();
        coliisions.velocityOld = velocity;

        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // 벽에서 멈추는 것 구현
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if(hit)
            {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if(coliisions.descendingSlope)
                    {
                        coliisions.descendingSlope = false;
                        velocity = coliisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != coliisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if(!coliisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if(coliisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(coliisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    coliisions.left = directionX == -1;
                    coliisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        // Sign = 부호를 반환함 0이상이면 +1, -1이하면 -1을 반환
        float directionY = Mathf.Sign(velocity.y);
        // 레이의 길이를 찾음
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        // isGround 구현, Ground에서 멈추는 방법 (이해 할 때 까지 봐야함)
        for (int i = 0; i < verticalRayCount; i++)
        {
            // 레이 발사 위치 찾기, +면 위, -면 아래
            // 콜라이더 bounds position 찾아서 저장
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            
            // 레이 간격 기준으로 점 갯수 만큼 설치함
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // 
            if(hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                
                rayLength = hit.distance;

                if(coliisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(coliisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                coliisions.below = directionY == -1;
                coliisions.above = directionY == 1;
            }
        }
        
        if(coliisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up *  velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != coliisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    coliisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY =  Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            coliisions.below = true;
            coliisions.climbingSlope = true;
            coliisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        coliisions.slopeAngle = slopeAngle;
                        coliisions.descendingSlope = true;
                        coliisions.below = true;
                    }
                }
            }
        }
    }

    // 레이 발사 지점 좌표 구조체
    struct RaycastOrigins 
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    /// <summary>
    /// 레이는 월드 좌표로 쏘니까 플레이어가 움직이면 레이 시작 지점도 갱신해야함
    /// </summary>
    void UpdateRaycastOrigins()
    {
        // 콜라이더가 겹칠만큼의 여백을 줌
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // 레이 시작 지점을 콜라이더의 꼭지점 좌표로 지정 -> 콜라이더는 GameObject를 따라 다니니깐
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max. y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // 레이를 가로, 세로 몇개 발사 할지 계산
    void CalculateRaySpacing()
    {
        // 발사 지점이 여백만큼 더 벌어져야 하니까
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        
        // 최소는 2개이고 무한대로 늘리게 함
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        // 레이 발사지점 계산
        // 콜라이더 사이즈 / (카운트-1) = 
        // ex) 1 / 3 = 0.3333 -> 0.3333 마다 시작지점 설치하게 함
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct ColiisionInfo
    {
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

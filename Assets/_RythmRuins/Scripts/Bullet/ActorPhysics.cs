using UnityEngine;
using System.Collections;

public class ActorPhysics : MonoBehaviour
{
    public Vector2 velocity;
    public ActorInfo info;
    BoxCollider boxCollider;
    Bounds bound
    {
        get { return boxCollider.bounds; }
    }
    float buffer = 0.01f;
    public bool isFacingRight = true;
    public float facingRight
    {
        get
        {
            if (isFacingRight) return 1;
            return -1;
        }
    }
    private bool isGrounded = false;
    public enum MotionState { Grounded, Falling, Jumping }
    private MotionState _motionState = MotionState.Falling;
    public MotionState motionState
    {
        get
        {
            if (isGrounded) return MotionState.Grounded;
            if (velocity.y > 0) return MotionState.Jumping;
            return MotionState.Falling;
        }
    }
    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider>() as BoxCollider;
    }
    protected void Update()
    {
        Phys();
        //Debug.DrawRay(transform.position,velocity*1);
    }

    public void Move(float dir)
    {
        if (isGrounded)
        {
            velocity.x += dir * info.runSpeed;
        }
        else
        {
            velocity.x += dir * info.airSpeed;
        }
        isFacingRight = dir > 0;
    }
    public void MoveY(float dir)
    {
        velocity.y += dir * info.runSpeed;
    }

    public void Jump(float strength)
    {
        if (isGrounded)
        {
            velocity.y += strength;
            isGrounded = false;
        }
    }

    public void Jump()
    {
        Jump(info.jumpPower);
    }
    public void JumpHit()
    {
        Jump(info.jumpPower);
    }
    public void JumpRelease()
    {
        if (motionState == MotionState.Jumping)
        {
            velocity.y *= 0.5f;
        }
    }

    public float TryHitFloor()
    {
        RaycastHit hit;
        return TryHitFloor(out hit);
    }
    public float TryHitFloor(out RaycastHit hit)
    {
        return TryHitEdge(out hit,
            new Vector3(bound.min.x, bound.min.y, transform.position.z),
            new Vector3(bound.max.x, bound.min.y, transform.position.z),
            -Vector3.up);

    }
    public float TryHitCeiling(out RaycastHit hit)
    {
        return TryHitEdge(out hit,
            new Vector3(bound.min.x, bound.max.y, transform.position.z),
            new Vector3(bound.max.x, bound.max.y, transform.position.z),
            Vector3.up);
    }
    public float TryHitRight(out RaycastHit hit)
    {
        return TryHitEdge(out hit,
            new Vector3(bound.max.x, bound.min.y + buffer, transform.position.z),
            new Vector3(bound.max.x, bound.max.y, transform.position.z),
            Vector3.right);
    }
    public float TryHitLeft(out RaycastHit hit)
    {
        return TryHitEdge(out hit,
            new Vector3(bound.min.x, bound.min.y + buffer, transform.position.z),
            new Vector3(bound.min.x, bound.max.y, transform.position.z),
            -Vector3.right);
    }

    public float TryHitEdge(out RaycastHit hit, Vector3 o1, Vector3 o2, Vector3 d)
    {
        //point 1
        RaycastHit hit1;
        float dist1 = TryHit(out hit1, o1, d);

        //poin 2
        RaycastHit hit2;
        float dist2 = TryHit(out hit2, o2, d);

        //result
        if (dist1 < dist2 && dist1 != 0)
        {
            hit = hit1;
            return dist1;
        }
        hit = hit2;
        return dist2;
    }
    public float TryHit(out RaycastHit hit, Vector3 o, Vector3 d)
    {
        if (Physics.Raycast(new Ray(o, d.normalized), out hit, 100f))
        {
            Debug.DrawRay(o, d, Color.red);
            return hit.distance;
        }
        else
        {
            Debug.DrawRay(o, d, Color.blue);
            return float.MaxValue;
        }
    }
    void Phys()
    {
        Vector3 newPos = transform.position;
        newPos.x += velocity.x * Time.deltaTime;
        newPos.y += velocity.y * Time.deltaTime;
        RaycastHit hit;
        float floorDist = TryHitFloor(out hit);
        if (floorDist != 0 && !isGrounded)
            if (floorDist < -velocity.y * Time.deltaTime)
            {
                isGrounded = true;
                velocity.y = 0;
                newPos.y = hit.point.y + bound.size.y * 0.5f + buffer;
            }
        if (isGrounded && floorDist > buffer * 2)
        {
            isGrounded = false;
        }
        if (motionState == MotionState.Jumping)
        {
            RaycastHit hitCeilling;
            float ceilingDist = TryHitCeiling(out hitCeilling);
            if (ceilingDist < velocity.y * Time.deltaTime && ceilingDist != 0)
            {
                newPos.y = hitCeilling.point.y - bound.size.y * 0.5f;
                velocity.y = 0;
            }
        }
        RaycastHit hitWall;
        float wallDist;
        float scaler;
        if (velocity.x > 0)
        {
            wallDist = TryHitRight(out hitWall);
            scaler = -0.5f;
        }
        else
        {
            wallDist = TryHitLeft(out hitWall);
            scaler = 0.5f;
        }

        if (wallDist < Mathf.Abs(velocity.x * Time.deltaTime) && wallDist != 0)
        {
            newPos.x = hitWall.point.x + (bound.size.x + buffer) * scaler;
            velocity.x = 0;
        }

        transform.position = newPos;

        if (isGrounded)
        {
            velocity -= velocity * Time.deltaTime * info.friction;
        }
        else
        {
            velocity -= velocity * Time.deltaTime * info.airFriction;
            velocity.y -= Time.deltaTime + info.gravity;
        }
    }
    /*
    void Grounded()
    {

    }
    void Jumping()
    {

    }
    void Faling()
    {

    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Rigidbody2D m_physics;

    [SerializeField, Range(0f,1f)]
    private float m_slopeAngle = 0.8f;
    [SerializeField, Range(0f,1f)]
    private float m_wallAngle = 0.8f;
    [SerializeField]
    private float m_speed = 10f;
    [SerializeField]
    private float m_acceleration = 5f;
    [SerializeField]
    private float m_brake = 15f;
    [SerializeField]
    private float m_jump = 10f;
    [SerializeField]
    private Vector2 m_wallJump = new Vector2(7f, 7f);
    [SerializeField]
    private float m_normalGravity = 1f;
    [SerializeField]
    private float m_wallGravity = 0.5f;


    private bool m_onGround = false;
    private bool m_onLeftWall = false;
    private bool m_onRightWall = false;
    private Collider2D m_floorCollider = null;
    private Collider2D m_wallCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        m_physics.gravityScale = m_normalGravity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = m_physics.velocity;
        if(m_onGround && PlayerInput.JumpDown)
        {
            velocity.y = m_jump;
        }
        float move = PlayerInput.Horizontal;
        if (move > 0f && velocity.x < m_speed)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, m_speed, Time.deltaTime * (velocity.x < 0 ? m_brake : m_acceleration));
        }
        if (move < 0f && velocity.x > -m_speed)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, -m_speed, Time.deltaTime * (velocity.x > 0 ? m_brake : m_acceleration));
        }
        m_physics.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        if (Vector2.Dot(normal, Vector2.up) > m_slopeAngle)
        {
            m_floorCollider = collision.otherCollider;
            m_onGround = true;
            return;
        }
        float wallDot = Vector2.Dot(normal, Vector2.right);
        if (wallDot > m_wallAngle)
        {
            m_wallCollider = collision.otherCollider;
            m_onRightWall = true;
            m_physics.gravityScale = m_wallGravity;
            return;
        }
        if (wallDot < -m_wallAngle)
        {
            m_wallCollider = collision.otherCollider;
            m_onLeftWall = true;
            m_physics.gravityScale = m_wallGravity;
            return;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D collider = collision.otherCollider;
        if (m_floorCollider == collider)
        {
            m_onGround = false;
            m_floorCollider = null;
            return;
        }
        if (m_wallCollider == collider)
        {
            m_onLeftWall = false;
            m_onRightWall = false;
            m_wallCollider = null;
            m_physics.gravityScale = m_normalGravity;
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        m_physics = GetComponent<Rigidbody2D>();
    }
#endif
}

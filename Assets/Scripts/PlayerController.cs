//Copyright 2021 Andrew Young
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    [SerializeField]
    private Animator m_animator;

    [Header("Physics")]
    [SerializeField]
    private float m_maxSpeed = 7;
    [SerializeField]
    private float m_jumpTakeOffSpeed = 7;
    [SerializeField]
    protected float m_minGroundNormalY = .65f;
    [SerializeField]
    protected float m_gravityModifier = 1f;

    [Header("Gameplay")]
    [SerializeField]
    private float m_damageOverTime = 1f;

    public Player Player => m_player;

    private Vector2 m_targetVelocity;
    private bool m_grounded;
    private Vector2 m_groundNormal;
    private Rigidbody2D m_physics;
    private Vector2 m_velocity;
    private ContactFilter2D m_contactFilter;
    private RaycastHit2D[] m_hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> m_hitBufferList = new List<RaycastHit2D>(16);

    private float m_currentDOTTimer = -1f;

    private const float MIN_MOVE_DISTANCE = 0.001f;
    private const float SHELL_RADIUS = 0.01f;

    void OnEnable()
    {
        m_physics = GetComponent<Rigidbody2D>();
        m_player.AdaptedElementChanged += OnPlayerAdapted;
    }

    private void OnDisable()
    {
        m_player.AdaptedElementChanged -= OnPlayerAdapted;
    }

    void Start()
    {
        m_contactFilter.useTriggers = false;
        m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        m_contactFilter.useLayerMask = true;
    }

    void Update()
    {
        m_targetVelocity = Vector2.zero;
        ComputeVelocity();

        if (m_player.IsSafe)
        {
            m_currentDOTTimer = m_damageOverTime;
        }
        else
        {
            m_currentDOTTimer -= Time.deltaTime;
            if (m_currentDOTTimer < 0f)
            {
                m_player.Health.Value -= 1;
                m_currentDOTTimer = m_damageOverTime;
            }
        }
    }

    void FixedUpdate()
    {
        m_velocity += m_gravityModifier * Physics2D.gravity * Time.deltaTime;
        m_velocity.x = m_targetVelocity.x;

        m_grounded = false;

        Vector2 deltaPosition = m_velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(m_groundNormal.y, -m_groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MIN_MOVE_DISTANCE)
        {
            int count = m_physics.Cast(move, m_contactFilter, m_hitBuffer, distance + SHELL_RADIUS);
            m_hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                m_hitBufferList.Add(m_hitBuffer[i]);
            }

            for (int i = 0; i < m_hitBufferList.Count; i++)
            {
                Vector2 currentNormal = m_hitBufferList[i].normal;
                if (currentNormal.y > m_minGroundNormalY)
                {
                    m_grounded = true;
                    if (yMovement)
                    {
                        m_groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(m_velocity, currentNormal);
                if (projection < 0)
                {
                    m_velocity = m_velocity - projection * currentNormal;
                }

                float modifiedDistance = m_hitBufferList[i].distance - SHELL_RADIUS;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        m_physics.position = m_physics.position + move.normalized * distance;
    }


    void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && m_grounded)
        {
            m_velocity.y = m_jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (m_velocity.y > 0)
            {
                m_velocity.y = m_velocity.y * 0.5f;
            }
        }

        if (Mathf.Abs(move.x) > 0.01f && (move.x > 0f == m_spriteRenderer.flipX))
        {
            m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
        }

        m_animator.SetBool("grounded", m_grounded);
        m_animator.SetFloat("velocityX", Mathf.Abs(m_velocity.x) / m_maxSpeed);
        m_animator.SetFloat("velocityY", m_velocity.y);

        m_targetVelocity = move * m_maxSpeed;
    }

    void OnPlayerAdapted(Element e)
    {
        m_spriteRenderer.material.SetTexture("_DisplayTex", e.AdaptedPlayerTexture);
    }
}
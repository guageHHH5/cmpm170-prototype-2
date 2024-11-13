using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Links")]
    public PlayerInputMap map;
    Rigidbody2D rb;
    Collider2D m_collider;


    [Header("Movement Values")]
    [SerializeField] float maxGroundedSpeed;
    [SerializeField] float maxAirSpeed; // Max speed where additional strafe force can be applied
    [SerializeField] float moveForce; // Force when holding a direction 
    [SerializeField] float groundedNoInputCounterForce; // Force when grounded not holding a direction to slow down player

    [Header("Other Values")]
    [SerializeField] float playerGroundedRadius = 0.3f;
    [SerializeField] LayerMask obstacleLayer;

    void Awake()
    {
        map = new();
        map.Enable();
        rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        float strafe = map.Player.Movement.ReadValue<float>();
        Strafe(strafe);
    }

    private void Strafe(float dir)
    {
        if (IsGrounded)
        {
            if (dir == 0) // No input
            {
                if (Mathf.Abs(rb.linearVelocityX) > .001f) // slow down
                {
                    rb.AddForceX(Mathf.Sign(rb.linearVelocityX) * -groundedNoInputCounterForce);
                }
            } else
            {
                if (Mathf.Sign(dir) != Mathf.Sign(rb.linearVelocityX)) // Turn around
                {
                    rb.AddForceX(dir * moveForce);
                }
                if (Mathf.Abs(rb.linearVelocityX) < maxGroundedSpeed) 
                {  
                    if (Mathf.Abs(rb.linearVelocityX) < 1) // Acceleration
                    {
                        rb.AddForceX(dir * moveForce * 2);
                    }
                    rb.AddForceX(dir * moveForce);
                } 
    
            }
        } else
        {
            if (dir == 0) return;

            if (Mathf.Sign(dir) != Mathf.Sign(rb.linearVelocityX)) // Turn around
            {
                rb.AddForceX(dir * moveForce);
            }
            if (Mathf.Abs(rb.linearVelocityX) < maxAirSpeed)
            {
                if (Mathf.Abs(rb.linearVelocityX) < 1) // Acceleration
                {
                    rb.AddForceX(dir * moveForce * 2);
                }
                rb.AddForceX(dir * moveForce);
            }
        }
    }

    private bool IsGrounded
    {
        get
        {
            Vector2 pos = new Vector2(m_collider.bounds.center.x, m_collider.bounds.center.y - m_collider.bounds.extents.y);
            Debug.DrawLine(pos, pos + Vector2.down * playerGroundedRadius, Color.green);
            return Physics2D.OverlapCircle(pos, playerGroundedRadius, obstacleLayer);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeGun_Bullet : MonoBehaviour
{
    internal Vector2 dir;
    public float speed;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 _dir, Vector2 _pos)
    {
        transform.position = new Vector2(_pos.x, _pos.y);
        dir = _dir;
        rb.linearVelocity = dir * speed;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PogoMeleeHandler : MonoBehaviour
{
    public bool touchingEnemy;
    private List<Collider2D> enemies;
    private void Start()
    {
        enemies = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Add(collision);
            touchingEnemy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Remove(collision);
            if(enemies.Count == 0)
            {
                touchingEnemy = false;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SyringeType
{
    public virtual void UsePogo(Vector2 dir) { }
    public virtual void OnHoldPogo(Vector2 dir) { }
    public virtual void OnFixedUpdate() { }
}

public class DefaultPogo : SyringeType
{
    float initialPogoBoost = 14f;
    float shortPogoHop = 6f;
    float collisionRadius = .2f;
    float pogoBoostLimit = 1f;
    float extendedPogoBoostAmount = 100f;

    Transform pogoTipTransform;
    Rigidbody2D playerRB;
    PogoManager manager;
    LayerMask obstacleLayer;
    public DefaultPogo(PogoManager _manager, Transform _pogoTipTransform, Rigidbody2D _playerRB, LayerMask layer)
    {
        // Link all of pogo's dependencies
        pogoTipTransform = _pogoTipTransform;
        playerRB = _playerRB;
        obstacleLayer = layer;
        manager = _manager;
    }


    bool pogoCooldown = false;
    bool isBoosting = false;
    Vector2 boostDir;
    public override void UsePogo(Vector2 dir) // Called every time player tries to use pogo
    {
        if (FacingObstacle && !pogoCooldown)
        {
            boostDir = dir.normalized;
            playerRB.linearVelocity = -dir.normalized * initialPogoBoost;
            manager.StartCoroutine(PogoBoost()); // Pogo boost and cooldown
            manager.StartCoroutine(PogoCooldown(0.3f));
            manager.StartCoroutine(ShortHop()); // Check if input is continuously held to activate short hop
        } else
        {
            // If player cannot use pogo, buffer input and continuously check conditions
            manager.StartCoroutine(BufferPogo());
        }
    }

    private IEnumerator BufferPogo()
    {
        float bufferTime = 0f;
        while (!(FacingObstacle && !pogoCooldown))
        {
            if (bufferTime >= .1f)
            {
                // If player cannot meet conditions within buffer window, cue short hop
                manager.StartCoroutine(ShortHop());
                yield break;
            }
            bufferTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        // if conditions are met, call UsePogo() once more to activate it
        UsePogo(manager.direction);
    }

    private IEnumerator ShortHop()
    {
        while (manager.usePogoAction.inProgress)
        {
            yield return new WaitUntil(() => FacingObstacle && !pogoCooldown);
            if (!manager.usePogoAction.inProgress) break;
            // Once player can use pogo, only short hop if player is still holding input down

            // Short hop values
            playerRB.linearVelocityY = -manager.direction.normalized.y * shortPogoHop;
            playerRB.linearVelocityX = -manager.direction.normalized.x * shortPogoHop * 1.4f;

            manager.StartCoroutine(PogoCooldown(0.05f)); // Short hop cooldown
        }
    }

    private IEnumerator PogoBoost()
    {
        float boostTime = 0f;
        isBoosting = true;
        // while moving up && input is held && less than boost time limit
        while (isBoosting && manager.usePogoAction.inProgress && boostTime < pogoBoostLimit)
        {
            boostTime += Time.deltaTime;
            yield return null;
        }
        isBoosting = false;
    }

    public override void OnFixedUpdate()
    {
        if (playerRB.linearVelocity.y <= 0) isBoosting = false; // Disable boosting once player moves down
        else if (isBoosting)
        {
            // Apply extra boost force for holding in input
            playerRB.AddForce(-boostDir * extendedPogoBoostAmount);
        }
    }

    private IEnumerator PogoCooldown(float time)
    {
        pogoCooldown = true;
        yield return new WaitForSeconds(time);
        pogoCooldown = false;
    }
    private bool FacingObstacle
    {
        get
        {
            return Physics2D.OverlapCircle((Vector2)pogoTipTransform.position, collisionRadius, obstacleLayer);
        }
    }
}

public class SyringeGun : SyringeType
{

    Transform pogoTipTransform;
    PogoManager manager;
    LayerMask obstacleLayer;
    GameObject bullet;

    public List<GameObject> bulletPool;

    public SyringeGun(PogoManager _manager, Transform _pogoTipTransform, LayerMask layer)
    {
        pogoTipTransform = _pogoTipTransform;
        obstacleLayer = layer;
        manager = _manager;
        // Create bullet object pool
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        bulletPool = new();
        for (int i = 0; i < 20; i++)
        {
            GameObject bul = Object.Instantiate(bullet, pogoTipTransform.position, Quaternion.identity);
            bul.SetActive(false);
            bulletPool.Add(bul);
        }
    }

    private GameObject GetBulletFromPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy) return bulletPool[i];
        }

        // If no more bullets, create another one
        GameObject bul = Object.Instantiate(bullet, pogoTipTransform.position, Quaternion.identity);
        bul.SetActive(false);
        bulletPool.Add(bul);
        return bul;
    }


    bool pogoCooldown = false;
    public override void UsePogo(Vector2 dir)
    {
        // If no cooldown, create bullet
        if (!pogoCooldown)
        {
            GameObject bul = GetBulletFromPool();
            bul.SetActive(true);
            bul.GetComponent<SyringeGun_Bullet>().Init(dir.normalized, pogoTipTransform.position);
            manager.StartCoroutine(SyringeCooldown());
        }
    }
    private IEnumerator SyringeCooldown()
    {
        pogoCooldown = true;
        yield return new WaitForSeconds(0.25f);
        pogoCooldown = false;
    }

    float collisionRadius = .1f;
    private bool FacingObstacle
    {
        get
        {
            return Physics2D.OverlapCircle((Vector2)pogoTipTransform.position, collisionRadius, obstacleLayer);
        }
    }
}

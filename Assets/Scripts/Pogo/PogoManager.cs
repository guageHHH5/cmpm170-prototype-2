using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PogoManager : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] Transform pogoTip;
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] LayerMask obstaclesLayerMask;

    [Header("Values")]
    [SerializeField] float rotationSpeed;

    public SyringeType defaultPogo;
    public SyringeType currSyringeType;
    public PogoMeleeHandler pogoMeleeHandler;
    private List<SyringeType> syringeTypes;

    internal InputAction usePogoAction;
    internal InputAction useSyringeAction;
    void Start()
    {
        defaultPogo = new DefaultPogo(this, pogoTip, playerRB, obstaclesLayerMask);
        syringeTypes = new()
        {
            new SyringeGun(this, pogoTip, obstaclesLayerMask),
        };
        currSyringeType = syringeTypes[0];

        usePogoAction = PlayerInterface.Singleton.playerInputManager.map.Player.UsePogo;
        usePogoAction.started += UsePogo;
        useSyringeAction = PlayerInterface.Singleton.playerInputManager.map.Player.UseSyringe;
        useSyringeAction.started += UseSyringe;
    }

    // Temp values
    internal Vector3 direction; // current direction this frame

    private void Update()
    {
        // AIM POGO
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
        var targetAngle = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion currentAngle = transform.rotation;
        transform.rotation = Quaternion.Lerp(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        defaultPogo.OnFixedUpdate();
    }

    private void UsePogo(InputAction.CallbackContext c)
    {
        defaultPogo.UsePogo(direction);

    }
    
    private void UseSyringe(InputAction.CallbackContext c)
    {
        if (currSyringeType == null) return;
        currSyringeType.UsePogo(direction);
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface Singleton { get; private set; }
    [Header("Links")]
    public PlayerInputManager playerInputManager;
    private void Awake()
    {
        if (Singleton != null && Singleton != this) // Set up singleton
        {
            Destroy(this);
        } else
        {
            Singleton = this;
        }
    }
}

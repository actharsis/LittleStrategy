using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health;

    internal void ReceiveDamage(float damageInflict)
    {
        Health -= damageInflict;
    }
}

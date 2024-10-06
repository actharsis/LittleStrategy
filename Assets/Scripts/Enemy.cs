using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health;

    internal void TakeDamage(float damageInflict)
    {
        Health -= damageInflict;
    }
}

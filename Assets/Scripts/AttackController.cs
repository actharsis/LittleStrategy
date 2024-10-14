using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    public bool IsPlayer;

    public float UnitDamage;

    public GameObject MuzzleEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer && other.CompareTag("Enemy") && TargetToAttack == null)
        {
            TargetToAttack = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer && other.CompareTag("Enemy") && TargetToAttack == null)
        {
            TargetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer && other.CompareTag("Enemy") && TargetToAttack != null)
        {
            TargetToAttack = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform TargetToAttack;

    public Material IdleStateMaterial;
    public Material FollowStateMaterial;
    public Material AttackStateMaterial;

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

    public void SetIdleMaterial()
    {
        //GetComponent<Renderer>().material = IdleStateMaterial;
    }
    public void SetFollowMaterial()
    {
        //GetComponent<Renderer>().material = FollowStateMaterial;
    }
    public void SetAttackMaterial()
    {
        //GetComponent<Renderer>().material = AttackStateMaterial;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}

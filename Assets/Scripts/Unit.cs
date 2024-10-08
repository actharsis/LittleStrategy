using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Unit : MonoBehaviour
{
    private float _unitHealth;
    public float UnitMaxHealth;

    public HealthTracker HealthTracker;

    private Animator _animator;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        UnitSelectionManager.Instance.AllUnitsList.Add(gameObject);

        _unitHealth = UnitMaxHealth;
        UpdateHealthUI();
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.AllUnitsList.Remove(gameObject);
    }

    private void UpdateHealthUI()
    {
        HealthTracker.UpdateSliderValue(_unitHealth, UnitMaxHealth);

        if (_unitHealth <= 0)
        {
            TriggerUnitDeath();
        }
    }

    private void TriggerUnitDeath()
    {
        Destroy(gameObject);
    }

    internal void TakeDamage(float damageToInflict)
    {
        _unitHealth -= damageToInflict;
        UpdateHealthUI();
    }

    public void Update()
    {
        _animator.SetBool("isMoving", _agent.remainingDistance > _agent.stoppingDistance);
    }
}

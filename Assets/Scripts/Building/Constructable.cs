using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Constructable : MonoBehaviour
{
    private NavMeshObstacle _obstacle; //BUG: building mesh doesn't correspond real object position

    public void ConstructableWasPlaced()
    {
         ActivateObstacle();
    }

    private void ActivateObstacle()
    {
        _obstacle = GetComponentInChildren<NavMeshObstacle>();
        _obstacle.enabled = true;
    }
}

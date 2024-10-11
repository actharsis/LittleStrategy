using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform localTrans;
    private Camera facingCamera;

    private void Start()
    {
        localTrans = GetComponent<Transform>();
        facingCamera = Camera.main;
    }

    private void Update()
    {
        if (facingCamera)
        {
            localTrans.LookAt(2 * localTrans.position - facingCamera.transform.position);
        }
    }
}

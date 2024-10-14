using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    private Camera _sceneCamera;
    [SerializeField] private LayerMask _placementLayerMask;

    [SerializeField] private Vector3 _lastPosition;

    public event Action OnLMBDown, OnRMBDown, OnExit;

    private void Start()
    {
        _sceneCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnLMBDown?.Invoke();
        if (Input.GetMouseButtonDown(1)) OnRMBDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape)) OnExit?.Invoke();
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = _sceneCamera.nearClipPlane;
        
        var ray = _sceneCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _placementLayerMask))
        {
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> AllUnitsList = new List<GameObject>();
    public List<GameObject> UnitsSelected = new List<GameObject>();

    public LayerMask Clickable;
    public LayerMask Ground;

    public LayerMask Attackable;
    public bool AttackCursorVisible;

    public GameObject GroundMarker;

    private Camera _cam;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        HandleClickSelection();
        HandleMoveCommand();
        HandleAttackCommand();

        CursorSelector();
    }

    private void CursorSelector()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _, Mathf.Infinity, Clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        }
        else if (Physics.Raycast(ray, out _, Mathf.Infinity, Attackable)
                 && UnitsSelected.Count > 0 && IsOffensiveUnitIn(UnitsSelected))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);
        }
        else if (Physics.Raycast(ray, out _, Mathf.Infinity, Ground)
                 && UnitsSelected.Count > 0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }
    }

    private void HandleClickSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Clickable))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MultiSelect(hit.collider.gameObject);
            }
            else
            {
                SelectByClicking(hit.collider.gameObject);
            }
        }
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift)) // is it really needed?
            {
                DeselectAll();
            }
        }
    }

    private void HandleMoveCommand()
    {
        if (!Input.GetMouseButtonDown(1) || UnitsSelected.Count <= 0) return;
        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, Ground)) return;
        GroundMarker.transform.position = hit.point;

        GroundMarker.SetActive(false);
        GroundMarker.SetActive(true);
    }

    private void HandleAttackCommand()
    {
        if (UnitsSelected.Count <= 0 || !IsOffensiveUnitIn(UnitsSelected)) return;
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Attackable))
            {
                AttackCursorVisible = true;

                if (!Input.GetMouseButtonDown(1)) return;
                var target = hit.transform;

                foreach (var unit in UnitsSelected.Where(unit => unit.GetComponent<AttackController>()))
                {
                    unit.GetComponent<AttackController>().TargetToAttack = target;
                }
            }
            else
            {
                AttackCursorVisible = false;
            }
        }
    }

    private bool IsOffensiveUnitIn(List<GameObject> gameObjects)
    {
        return UnitsSelected.Any(unit => unit.GetComponent<AttackController>());
    }

    private void MultiSelect(GameObject unit)
    {
        if (!UnitsSelected.Contains(unit))
        {
            UnitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            UnitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in UnitsSelected)
        {
            SelectUnit(unit, false);
        }

        GroundMarker.SetActive(false);
        UnitsSelected.Clear();
    }

    private static void SetMovementAbility(GameObject unit, bool isAbleMoving)
    {
        unit.GetComponent<UnitMovement>().enabled = isAbleMoving;
    }

    private static void SetStateSelectionIndicator(GameObject unit, bool isActive)
    {
        unit.transform.Find("SelectionIndicator").gameObject.SetActive(isActive);
    }

    internal void DragSelect(GameObject unit)
    {
        if (UnitsSelected.Contains(unit)) return;
        UnitsSelected.Add(unit);
        SelectUnit(unit, true);
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        UnitsSelected.Add(unit);

        SelectUnit(unit, true);
    }

    private static void SelectUnit(GameObject unit, bool isSelected)
    {
        SetStateSelectionIndicator(unit, isSelected);
        SetMovementAbility(unit, isSelected);
    }
}

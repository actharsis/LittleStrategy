using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    private Camera myCam;

    [SerializeField] private RectTransform _boxVisual;

    private Rect _selectionBox;

    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private void Start()
    {
        myCam = Camera.main;
        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;

            _selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if (_boxVisual.rect.width > 0 || _boxVisual.rect.height > 0)
            {
                UnitSelectionManager.Instance.DeselectAll();
                SelectUnits();
            }

            _endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (!Input.GetMouseButtonUp(0)) return;
        SelectUnits();

        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
        DrawVisual();
    }

    private void DrawVisual()
    {
        var boxStart = _startPosition;
        var boxEnd = _endPosition;

        var boxCenter = (boxStart + boxEnd) / 2;

        _boxVisual.position = boxCenter;

        var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        _boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        if (Input.mousePosition.x < _startPosition.x)
        {
            _selectionBox.xMin = Input.mousePosition.x;
            _selectionBox.xMax = _startPosition.x;
        }
        else
        {
            _selectionBox.xMin = _startPosition.x;
            _selectionBox.xMax = Input.mousePosition.x;
        }


        if (Input.mousePosition.y < _startPosition.y)
        {
            _selectionBox.yMin = Input.mousePosition.y;
            _selectionBox.yMax = _startPosition.y;
        }
        else
        {
            _selectionBox.yMin = _startPosition.y;
            _selectionBox.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (var unit in UnitSelectionManager.Instance.AllUnitsList.Where(unit =>
                     _selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position))))
        {
            UnitSelectionManager.Instance.DragSelect(unit);
        }
    }
}
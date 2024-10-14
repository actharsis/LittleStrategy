using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectsDatabseSO _database;

    [SerializeField] private GridData _floorData, _furnitureData;

    [SerializeField] private PreviewSystem _previewSystem;

    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer _objectPlacer;

    private int _selectedId;

    private IBuildingState _buildingState;

    private void Start()
    {
        _floorData = new GridData();
        _furnitureData = new GridData();
    }

    public void StartPlacement(int id)
    {
        Debug.Log("Should Start Placement");

        _selectedId = id;

        Debug.Log("Placement ID: " + id);


        StopPlacement();

        _buildingState = new PlacementState(id, _grid, _previewSystem, _database, _floorData, _furnitureData, _objectPlacer);

        _inputManager.OnLMBDown += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();

        _buildingState = new RemovingState(_grid, _previewSystem, _floorData, _furnitureData, _objectPlacer);

        _inputManager.OnLMBDown += PlaceStructure;
        _inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(_inputManager.IsPointerOverUI()){
            Debug.Log("Pointer was over UI - Returned");
            return;
        }
        var mousePosition = _inputManager.GetSelectedMapPosition();
        var gridPosition = _grid.WorldToCell(mousePosition);

        _buildingState.OnAction(gridPosition);


        // ---- Using the ID remove used resources from resource manager ---- // 
        var ob = _database.GetObjectByID(_selectedId);
       // ResourceManager.Instance.RemoveResourcesBasedOnRequirements(ob, database);

        // ---- Add Buildable Benefits ---- // 
        foreach (var bf in ob.benefits)
        {
            CalculateAndAddBenefit(bf);
        }

        StopPlacement();
    }

    private static void CalculateAndAddBenefit(BuildBenefits bf)
    {
        switch (bf.benefitType)
        {
            case BuildBenefits.BenefitType.Housing:
             //StatusManager.Instance.IncreaseHousing(bf.benefitAmount);
                break;
        }
    }

    private void StopPlacement()
    {
        if (_buildingState == null)
            return;
       
        _buildingState.EndState();

        _inputManager.OnLMBDown -= PlaceStructure;
        _inputManager.OnExit -= StopPlacement;

        _lastDetectedPosition = Vector3Int.zero;

        _buildingState = null;
    }

    private void Update()
    {
        // We return because we did not selected an item to place (not in placement mode)
        // So there is no need to show cell indicator
        if (_buildingState == null)
            return;
      
        var mousePosition = _inputManager.GetSelectedMapPosition();
        var gridPosition = _grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition == gridPosition) return;
        _buildingState.UpdateState(gridPosition);
        _lastDetectedPosition = gridPosition;

    }
}

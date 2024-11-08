using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private readonly int _selectedObjectIndex = -1;
    private readonly int _id;
    private readonly Grid _grid;
    private readonly PreviewSystem _previewSystem;
    private readonly ObjectsDatabaseSO _database;
    private readonly GridData _floorData;
    private readonly GridData _furnitureData;
    private readonly ObjectPlacer _objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        _id = iD;
        this._grid = grid;
        this._previewSystem = previewSystem;
        this._database = database;
        this._floorData = floorData;
        this._furnitureData = furnitureData;
        this._objectPlacer = objectPlacer;

        _selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == _id);
        if (_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsData[_selectedObjectIndex].Prefab,
                database.objectsData[_selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        var placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        var index = _objectPlacer.PlaceObject(_database.objectsData[_selectedObjectIndex].Prefab, _grid.CellToWorld(gridPosition));

        ResourceManager.Instance.DecreaseResources(_database.objectsData[_selectedObjectIndex]);

        var buildingType = _database.objectsData[_selectedObjectIndex].BuildingType;
        ResourceManager.Instance.UpdateBuildingChanged(buildingType, true);

        // If this id is a floor id, then its a floor data, else its a furniture data
        var selectedData = GetAllFloorIDs().Contains(_database.objectsData[_selectedObjectIndex].ID) ? _floorData : _furnitureData;
       
        selectedData.AddObjectAt(gridPosition,
            _database.objectsData[_selectedObjectIndex].Size,
            _database.objectsData[_selectedObjectIndex].ID,
            index);

        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    private static List<int> GetAllFloorIDs()
    {
        return new List<int> { 11 };
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
      

        var selectedData = GetAllFloorIDs().Contains(_database.objectsData[selectedObjectIndex].ID) ? _floorData : _furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, _database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // Show the player if he can place the item
        var placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
    }
}

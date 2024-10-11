using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    private readonly Dictionary<Vector3Int, PlacementData> _placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        var positionToOccuply = CalculatePositions(gridPosition, objectSize);
        var data = new PlacementData(positionToOccuply, id, placedObjectIndex);
        if (positionToOccuply.Any(pos => !_placedObjects.TryAdd(pos, data)))
        {
            throw new Exception("Dictionary already contains this cell position");
        }
    }

    private static List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal1 = new();
        for (var x = 0; x < objectSize.x; x++)
        {
            for (var y = 0; y < objectSize.y; y++)
            {
                returnVal1.Add(gridPosition + new Vector3Int(x,0,y));
            }
        }
        return returnVal1;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        var positionToOccupy = CalculatePositions(gridPosition, objectSize);
        return positionToOccupy.All(pos => !_placedObjects.ContainsKey(pos));
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in _placedObjects[gridPosition].OccupiedPositions)
        {
            _placedObjects.Remove(pos);
        }
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (_placedObjects.TryGetValue(gridPosition, out var o) == false)
            return -1;
        return o.PlacedObjectIndex;
    }
}


public class PlacementData
{
    public List<Vector3Int> OccupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.OccupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
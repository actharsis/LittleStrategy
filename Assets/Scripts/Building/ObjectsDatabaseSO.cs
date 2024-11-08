using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;


    public ObjectData GetObjectByID(int id)
    {
        foreach (var obj in objectsData.Where(obj => obj.ID == id))
        {
            return obj;
        }

        return new(); // This cannot happen
    }

}

public enum BuildingType
{
    None,
    CommandCenter,
    Barrack
}

[System.Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public BuildingType BuildingType { get; private set; }

    [field: SerializeField]
    [TextArea(3, 10)]
    public string Description;

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public List<ResourceRequirement> ResourceRequirements { get; private set; }

    [field: SerializeField]
    public List<BuildingType> BuildRequirements { get; private set; }

    [field: SerializeField]
    public List<BuildBenefits> Benefits { get; private set; }
}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceManager.ResourceType resource;
    public int amount;
}


[System.Serializable]
public class BuildBenefits
{
    public enum BenefitType
    {
        Housing
    }


    public string benefit;
    public Sprite benefitIcon;
    public BenefitType benefitType;
    public int benefitAmount;
}
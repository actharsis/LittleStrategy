using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; set; }

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

    public event Action OnResourceChanged;
    public event Action OnBuildingsChanged;

    private int _credits = 500; //test value
    public TextMeshProUGUI creditsUI;

    public List<BuildingType> ExistingBuildings;

    public enum ResourceType
    {
        Credits,
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateBuildingChanged(BuildingType buildingType, bool isNew)
    {
        if (isNew)
        {
            ExistingBuildings.Add(buildingType);
        }
        else
        {
            ExistingBuildings.Remove(buildingType);
        }

        OnBuildingsChanged?.Invoke();
    }

    private void UpdateUI()
    {
        creditsUI.text = _credits.ToString();
    }

    public void IncreaseResource(ResourceType resource, int amountToIncrease)
    {
        switch (resource)
        {
            case ResourceType.Credits:
            {
                _credits += amountToIncrease;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }

        OnResourceChanged?.Invoke();
    }

    public void DecreaseResource(ResourceType resource, int amountToDecrease)
    {
        switch (resource)
        {
            case ResourceType.Credits:
            {
                _credits -= amountToDecrease;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }

        OnResourceChanged?.Invoke();
    }

    public int GetResourceAmount(ResourceType resource)
    {
        switch (resource)
        {
            case ResourceType.Credits:
                return _credits;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
    }

    internal void DecreaseResources(ObjectData objectData)
    {
        foreach (var req in objectData.ResourceRequirements)
        {
            DecreaseResource(req.resource, req.amount);
        }
    }

    public void OnEnable()
    {
        OnResourceChanged += UpdateUI;
    }

    public void OnDisable()
    {
        OnResourceChanged -= UpdateUI;
    }
}

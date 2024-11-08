using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    public BuyingSystem BuyingSystem;

    private bool IsAvailable;

    public int DatabaseItemId;

    public void Start()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
        HandleResourceChanged();

        ResourceManager.Instance.OnBuildingsChanged += HandleBuildingsChanged;
        HandleBuildingsChanged();
    }

    public void ClickedOnSlot() //remove from unity editor
    {
        if (IsAvailable)
        {
            BuyingSystem.PlacementSystem.StartPlacement(DatabaseItemId);
        }
    }

    public void UpdateAvailabilityUI()
    {
        if (IsAvailable)
        {
            GetComponent<Image>().color = Color.white;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Image>().color = Color.red;
            GetComponent<Button>().interactable = false;
        }
    }

    private void HandleResourceChanged()
    {
        var objectData = DatabaseManager.Instance.DatabaseSO.objectsData[DatabaseItemId];
        IsAvailable = objectData.ResourceRequirements.All(req => ResourceManager.Instance.GetResourceAmount(req.resource) >= req.amount);

        UpdateAvailabilityUI();
    }

    private void HandleBuildingsChanged()
    {
        var objectData = DatabaseManager.Instance.DatabaseSO.objectsData[DatabaseItemId];

        foreach (var dependency in objectData.BuildRequirements)
        {
            if (dependency == BuildingType.None)
            {
                gameObject.SetActive(true);
                return;
            }

            if (ResourceManager.Instance.ExistingBuildings.Contains(dependency)) continue;
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }
}

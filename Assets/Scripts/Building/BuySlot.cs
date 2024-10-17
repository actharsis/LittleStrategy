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
        HandleResourceChanged();
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

    private void OnEnable()
    {
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged()
    {
        var objectData = DatabaseManager.Instance.DatabaseSO.objectsData[DatabaseItemId];
        IsAvailable = objectData.requirements.All(req => ResourceManager.Instance.GetResourceAmount(req.resource) >= req.amount);

        UpdateAvailabilityUI();
    }
}

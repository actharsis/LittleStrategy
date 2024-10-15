using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
    public BuyingSystem BuyingSystem;

    public bool IsAvailable;

    public int DatabaseItemId;

    public void Start()
    {
        UpdateAvailabilityUI();
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
}

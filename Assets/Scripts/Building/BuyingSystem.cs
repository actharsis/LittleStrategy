using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyingSystem : MonoBehaviour
{
    public GameObject BuildingPanel;
    public GameObject UnitsPanel;

    public Button BuildingsButton;
    public Button UnitsButton;

    public PlacementSystem PlacementSystem;

    private void Start()
    {
        UnitsButton.onClick.AddListener(UnitCategorySelect);
        BuildingsButton.onClick.AddListener(BuildingCategorySelect);

        UnitsPanel.SetActive(false);
        BuildingPanel.SetActive(true);
    }

    private void BuildingCategorySelect()
    {
        UnitsPanel.SetActive(false);
        BuildingPanel.SetActive(true);
    }

    private void UnitCategorySelect()
    {
        UnitsPanel.SetActive(true);
        BuildingPanel.SetActive(false);
    }
}

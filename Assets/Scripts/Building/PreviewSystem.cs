using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] 
    private float previewYOffset = 0.06f;

    private GameObject _previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
    }
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        _previewObject = Instantiate(prefab);
        PreparePreview(_previewObject);
    }

    internal void StartShowingRemovePreview()
    {
        ApplyFeedbackToCursor(false);
    }

    private void PreparePreview(GameObject previewObject)
    {
        // Change the materials of the prefab (and its children) to semi-transparent

        var renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            var materials = rend.materials;
            for (var i = 0; i < materials.Length; i++)
            {
                // Getting the current material color
                var color = materials[i].color;
     
                // changing its alpha
                color.a = 0.5f;

                // setting the modified color
                materials[i].color = color;


                materials[i] = previewMaterialInstance;
            }

            rend.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        if (_previewObject != null)
        {
            Destroy(_previewObject);
        }
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (_previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
      
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        var c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private static void ApplyFeedbackToCursor(bool validity)
    {
        var c = validity ? Color.green : Color.red;
        c.a = 1f;
     
    }

    private void MovePreview(Vector3 position)
    {
        _previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

}

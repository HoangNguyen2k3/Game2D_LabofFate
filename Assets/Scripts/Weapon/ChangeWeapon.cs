using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject fireWeapon;
    [SerializeField] private GameObject iceWeapon;
    [SerializeField] private GameObject thunderWeapon;
    private ManagerLevelGame manageLevel;

    private bool isEventRegistered = false;

    void Start()
    {
        fireWeapon.SetActive(true);
        iceWeapon.SetActive(false);
        thunderWeapon.SetActive(false);
        TryInitializeManageLevel();

        if (manageLevel != null)
        {
            UpdateWeapon(manageLevel.current_map_element.Value);
        }
    }

    private void Update()
    {
        if (manageLevel == null || !isEventRegistered)
        {
            TryInitializeManageLevel();
        }
    }

    private void TryInitializeManageLevel()
    {
        var foundManager = FindFirstObjectByType<ManagerLevelGame>();

        if (foundManager != null && foundManager != manageLevel)
        {
          
            if (manageLevel != null && isEventRegistered)
            {
                manageLevel.current_map_element.OnValueChanged -= OnMapElementChanged;
            }

            manageLevel = foundManager;

            manageLevel.current_map_element.OnValueChanged += OnMapElementChanged;
            isEventRegistered = true;

            UpdateWeapon(manageLevel.current_map_element.Value);
        }
    }

    private void OnMapElementChanged(char oldValue, char newValue)
    {
        UpdateWeapon(newValue);
    }

    private void UpdateWeapon(char mapElement)
    {
        switch (mapElement)
        {
            case 'f':
                fireWeapon.SetActive(true);
                iceWeapon.SetActive(false);
                thunderWeapon.SetActive(false);
                break;
            case 'i':
                iceWeapon.SetActive(true);
                fireWeapon.SetActive(false);
                thunderWeapon.SetActive(false);
                break;
            case 't':
                thunderWeapon.SetActive(true);
                fireWeapon.SetActive(false);
                iceWeapon.SetActive(false);
                break;
            default:
                Debug.LogWarning($"Unknown map element: {mapElement}");
                break;
        }
    }

    private void OnDestroy()
    {
        if (manageLevel != null && isEventRegistered)
        {
            manageLevel.current_map_element.OnValueChanged -= OnMapElementChanged;
        }
    }
}

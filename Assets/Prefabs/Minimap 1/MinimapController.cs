using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public static MinimapController Instance; 

    [SerializeField]
    private Vector2 worldSize; 

    [SerializeField]
    private RectTransform contentRectTransform; 

    [SerializeField]
    private RectTransform startRectTransform;

    [SerializeField]
    private MinimapIcon minimapIconPrefab;

    public Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectsLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        UpdateMiniMapIcons();
    }

    public void RegisterMinimapWorldObject(MinimapWorldObject miniMapWorldObject)
    {
        var minimapIcon = Instantiate(minimapIconPrefab, startRectTransform);
        minimapIcon.Image.sprite = miniMapWorldObject.MinimapIcon;
        miniMapWorldObjectsLookup[miniMapWorldObject] = minimapIcon;
    }

    public void RemoveMinimapWorldObject(MinimapWorldObject minimapWorldObject)
    {
        if (miniMapWorldObjectsLookup.TryGetValue(minimapWorldObject, out MinimapIcon icon))
        {
            miniMapWorldObjectsLookup.Remove(minimapWorldObject);
            Destroy(icon.gameObject);
        }
    }

    private void UpdateMiniMapIcons()
    {
        foreach (var kvp in miniMapWorldObjectsLookup)
        {
            var miniMapWorldObject = kvp.Key;
            var miniMapIcon = kvp.Value;
            var mapPosition = WorldPositionToMapPosition(miniMapWorldObject.transform.position);
            miniMapIcon.RectTransform.anchoredPosition = mapPosition;
        }
    }

    private Vector2 WorldPositionToMapPosition(Vector3 worldPos)
    {
        float minimapWidth = contentRectTransform.rect.width;
        float minimapHeight = contentRectTransform.rect.height;

        float worldX = worldPos.x;
        float worldY = worldPos.y;

        float normalizedX = (worldX + worldSize.x / 2) / worldSize.x;
        float normalizedY = (worldY + worldSize.y / 2) / worldSize.y;

        float mapX = normalizedX * minimapWidth;
        float mapY = normalizedY * minimapHeight;

        return new Vector2(mapX, mapY);
    }
}

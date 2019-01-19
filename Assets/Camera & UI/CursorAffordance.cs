using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkableSFX = null;
    [SerializeField] Texture2D unknownSFX = null;
    [SerializeField] Texture2D TargetSFX = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
    CameraRaycaster cameraRaycaster;

    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += onLayerChanged;
    }

    void onLayerChanged(Layer layer)
    {
        print("I handled it");
        switch (layer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkableSFX, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(TargetSFX, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownSFX, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Don't know what cursor to show");
                break;
        }
    }
}

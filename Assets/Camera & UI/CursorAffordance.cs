using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkableSFX;
    [SerializeField] Texture2D unknownSFX;
    [SerializeField] Texture2D TargetSFX;

    CameraRaycaster cameraRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (cameraRaycaster.layerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkableSFX, Vector2.zero, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(TargetSFX, Vector2.zero, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownSFX, Vector2.zero, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownSFX, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}

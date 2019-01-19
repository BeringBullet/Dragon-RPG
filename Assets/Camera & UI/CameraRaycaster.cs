using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    public delegate void OnLayerChange(Layer layer);
    public event OnLayerChange onLayerChange;

    RaycastHit currentHit;
    public RaycastHit CurrentHit
    {
        get { return currentHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                currentHit = hit.Value;
                if (layerHit != layer)
                {
                    layerHit = layer;
                    onLayerChange(layerHit);
                }
                return;
            }
        }

        // Otherwise return background hit
        currentHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
        onLayerChange(layerHit);
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}

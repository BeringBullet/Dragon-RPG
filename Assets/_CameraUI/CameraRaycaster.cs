using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;
using System;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
    
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        float maxRaycastDepth = 100f; // Hard coded value
        const int WALKABLE_LAYER = 8;

        Rect screenRect = new Rect(0, 0, Screen.height, Screen.width);

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverWalkable;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

		void Update()
        {
            //screenRect = new Rect(0, 0, Screen.height, Screen.width);

            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Implement UI interation
            }
            else
            {
                PreformRayCast();
            }
        }

        private void PreformRayCast()
        {
           // if (screenRect.Contains(Input.mousePosition))
            //{
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastforEnemy(ray)) { return; }
                if (RaycastForWaltable(ray)) { return; }
           // }
        }

        private bool RaycastforEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray,out hitInfo, maxRaycastDepth);
            if (hitInfo.collider == null) return false;
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();

            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
            }

            return enemyHit;
        }

        private bool RaycastForWaltable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask WalkableLayerMask = 1 << WALKABLE_LAYER;
            bool WalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, WalkableLayerMask);
            if (WalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverWalkable(hitInfo.point);
            }
            return WalkableHit;
        }
    }
}
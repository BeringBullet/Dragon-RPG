using UnityEngine;

namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        // Update is called once per frame 
        void LateUpdate() => transform.LookAt(Camera.main.transform);
    }
}
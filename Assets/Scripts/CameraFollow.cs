using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }
    private void LateUpdate()
    {
        this.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

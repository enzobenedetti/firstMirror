using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CatchHider : NetworkBehaviour
{
    public bool isHider;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") || other.gameObject == gameObject) return;
        if (!other.gameObject.GetComponent<CatchHider>().isHider) return;
        Destroy(other.gameObject);
        FindObjectOfType<CanvasManager>().gameStarted = false;
    }
}

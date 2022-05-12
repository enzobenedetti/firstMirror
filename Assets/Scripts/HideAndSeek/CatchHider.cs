using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CatchHider : NetworkBehaviour
{
    public bool isHider;
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("aaaaaa");
        if (!other.gameObject.CompareTag("Player") || other.gameObject == gameObject) return;
        Debug.Log("bbbbb");
        if (!other.gameObject.GetComponent<CatchHider>().isHider) return;
        Debug.Log("ccccc");
        Destroy(other.gameObject);
    }
}

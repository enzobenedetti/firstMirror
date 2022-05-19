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
        CallEndGame();
        FindObjectOfType<CanvasManager>().gameStarted = false;
    }

    [Command]
    public void CallEndGame()
    {
        foreach (CatchHider hider in FindObjectsOfType<CatchHider>())
        {
            hider.ResetGame();
        }
    }

    [ClientRpc]
    public void ResetGame()
    {
        if (isLocalPlayer && transform.GetChild(1).gameObject.TryGetComponent(out Light o))
        {
            TimerHider.ResetTimer();
            Destroy(gameObject.GetComponentInChildren<Light>().gameObject);
            GetComponent<PlayerMovement>().enabled = false;
        }
            
    }
}

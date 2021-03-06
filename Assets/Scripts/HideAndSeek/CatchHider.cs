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
        if (!FindObjectOfType<CanvasManager>().gameStarted) return;
        if (!other.gameObject.CompareTag("Player") || other.gameObject == gameObject) return;
        if (!other.gameObject.GetComponent<CatchHider>().isHider) return;
        CallEndGame();
    }

    [Command (requiresAuthority = false)]
    public void CallEndGame()
    {
        foreach (CatchHider hider in FindObjectsOfType<CatchHider>())
        {
            hider.ResetGame();
        }
        FindObjectOfType<CanvasManager>().gameStarted = false;
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

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerStart : NetworkBehaviour
{
    public PlayerMovement playerMovement;
    private Vector3 spawnedPosition;
    public Material hiderMaterial;
    public Material seekerMaterial;
    private CatchHider _catchHider;
    public GameObject lightLamp;
    public GameObject hiderLamp;
    private GameObject _cam;
    
    [SyncVar]private int indexPlayer = -1;
    // Start is called before the first frame update
    void Awake()
    {
        _catchHider = GetComponent<CatchHider>();
        spawnedPosition = transform.position;
        playerMovement.enabled = false;
    }
    
    [Command(requiresAuthority = false)]
    public void SetIndex(int numbPlayer)
    {
        if (indexPlayer != -1) return;
        indexPlayer = numbPlayer;
        Debug.Log("index" + indexPlayer);
    }

    [ClientRpc]
    public void RpcBeginGame(int hider)
    {
        _catchHider.isHider = hider == indexPlayer;
        if (transform.position != spawnedPosition)
        {
            transform.position = spawnedPosition;
            transform.rotation = Quaternion.identity;
        }
        GetComponent<MeshRenderer>().material = _catchHider.isHider ? hiderMaterial : seekerMaterial;
        if (!isLocalPlayer) return;

        if (_catchHider.isHider)
        {
            GameObject lamp = Instantiate(hiderLamp, transform);
            playerMovement.enabled = true;
        }
		
        playerMovement.SetCameraChild();
    }

    [ClientRpc]
    public void RpcBeginChase()
    {
        if (!isLocalPlayer || _catchHider.isHider) return;
        playerMovement.enabled = true;
        GameObject lamp = Instantiate(lightLamp, transform);
        _catchHider.isHider = false;
    }
}

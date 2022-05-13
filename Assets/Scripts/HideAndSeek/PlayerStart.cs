using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerStart : NetworkBehaviour
{
    public PlayerMovement playerMovement;
    public Material hiderMaterial;
    public Material seekerMaterial;
    private CatchHider _catchHider;
    public GameObject lightLamp;
    public GameObject hiderLamp;
    private GameObject _cam;
    // Start is called before the first frame update
    void Awake()
    {
        _catchHider = GetComponent<CatchHider>();
        playerMovement.enabled = false;
    }

    [ClientRpc]
    public void RpcBeginGame()
    {
        GetComponent<MeshRenderer>().material = isClientOnly ? hiderMaterial : seekerMaterial;
        if (!isLocalPlayer) return;

        if (!isClientOnly)
        {
            GameObject lamp = Instantiate(hiderLamp, transform);
            _catchHider.isHider = true;
            playerMovement.enabled = true;
        }
		
        playerMovement.SetCameraChild();
    }

    [ClientRpc]
    public void RpcBeginChase()
    {
        if (!isLocalPlayer || !isClientOnly) return;
        playerMovement.enabled = true;
        GameObject lamp = Instantiate(lightLamp, transform);
        _catchHider.isHider = false;
    }
}

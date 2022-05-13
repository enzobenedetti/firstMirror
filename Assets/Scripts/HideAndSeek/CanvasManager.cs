using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : NetworkBehaviour
{
    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = !isClientOnly;
    }
    
    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        Debug.Log("uyfufyuf");
        foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
        {
            player.RpcBeginGame();
        }
    }
}

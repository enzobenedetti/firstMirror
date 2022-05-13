using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : NetworkBehaviour
{
    public Button startButton;

    public Text playerNumber;
    [SyncVar] public int playersNumber;

    [SyncVar] private bool _gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = !isClientOnly;
    }
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        ChangePlayersNumber(1);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        ChangePlayersNumber(-1);
    }

    [Command(requiresAuthority = false)]
    void ChangePlayersNumber(int modificator)
    {
        playersNumber += modificator;
    }

    private void Update()
    {
        playerNumber.text = "Player in room : " + playersNumber;
        if (_gameStarted) gameObject.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        Debug.Log("uyfufyuf");
        foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
        {
            player.RpcBeginGame();
        }
        
        TimerHider.StartTimer();
        _gameStarted = true;
    }
}

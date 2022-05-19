using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CanvasManager : NetworkBehaviour
{
    public Button startButton;

    public Text playerNumber;
    public Text gameGoal;

    public GameObject lobbyHolder;
    public GameObject gameHolder;
    [SyncVar] public int playersNumber;
    private int hiderIndex;

    [SyncVar] public bool gameStarted = false;
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
        foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
        {
            player.SetIndex(playersNumber - 1);
        }
    }

    private void Update()
    {
        playerNumber.text = "Player in room : " + playersNumber;
        foreach (CatchHider catchHider in FindObjectsOfType<CatchHider>())
        {
            if (!catchHider.isLocalPlayer) continue;
            if (catchHider.isHider)
            {
                gameGoal.text = "Hide!";
            }
            else gameGoal.text = "Chase Mr. Blue!";
        }
        if (gameStarted)
        {
            lobbyHolder.SetActive(false);
            gameHolder.SetActive(true);
        }
        else
        {
            lobbyHolder.SetActive(true);
            gameHolder.SetActive(false);
        }
    }

    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        hiderIndex = Random.Range(0, playersNumber);
        Debug.Log("hider is :" + hiderIndex);
        foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
        {
            player.RpcBeginGame(hiderIndex);
        }
        
        TimerHider.StartTimer();
        gameStarted = true;
    }
}

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
    public Text timeLeft;
    [SyncVar] private string timeText;

    public GameObject lobbyHolder;
    public GameObject gameHolder;
    public GameObject quitCanvas;
    [SyncVar] public int playersNumber;
    private int hiderIndex;

    [SyncVar] public bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        startButton.interactable = !isClientOnly;
    }

    private void OnConnectedToServer()
    {
        quitCanvas.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    void ChangePlayersNumber()
    {
        playersNumber = NetworkManager.singleton.numPlayers;
        foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
        {
            player.SetIndex(playersNumber - 1);
        }
    }

    private void Update()
    {
        ChangePlayersNumber();
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
            int timerText = 60 - (int)(TimerHider.timer);
            UpdateTime(timerText);
            timeLeft.text = timeText;
        }
        else
        {
            lobbyHolder.SetActive(true);
            gameHolder.SetActive(false);
        }
    }

    [Command(requiresAuthority = false)]
    void UpdateTime(int timer)
    {
        timeText = timer.ToString();
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
        LaunchAudio();
        gameStarted = true;
    }

    [ClientRpc]
    void LaunchAudio()
    {
        FindObjectOfType<AudioSource>().Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

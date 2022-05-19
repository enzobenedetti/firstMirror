using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TimerHider : NetworkBehaviour
{
    private float _timeToHide = 10f;
    private static bool _hided;
    private float _timeToSeek = 60f;

    private static bool _timerOn;
    private static float _timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timerOn) return;
        _timer += Time.deltaTime;
        if (_timer >= _timeToHide && !_hided)
        {
            _hided = true;
            foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
            {
                player.RpcBeginChase();
            }
        }
        if (_timer >= _timeToSeek)
        {
            FindObjectOfType<CatchHider>().CallEndGame();
        }
    }

    public static void StartTimer()
    {
        _timerOn = true;
    }

    public static void ResetTimer()
    {
        _timerOn = false;
        _timer = 0f;
        _hided = false;
    }
}

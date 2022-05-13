using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TimerHider : NetworkBehaviour
{
    private float _timeTohide = 10f;
    private bool _hided;
    private float _timeToseek = 60f;

    private static bool _timerOn;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timerOn) return;
        _timer += Time.deltaTime;
        if (_timer >= _timeTohide && !_hided)
        {
            _hided = true;
            foreach (PlayerStart player in FindObjectsOfType<PlayerStart>())
            {
                player.RpcBeginChase();
            }
        }
    }

    public static void StartTimer()
    {
        _timerOn = true;
    }
}

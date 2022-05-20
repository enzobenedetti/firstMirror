using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class PlayerMovement : NetworkBehaviour
{
	public float speed = 10f;
	private CharacterController _charaCont;
	private GameObject _cam;

	private void Awake()
	{
		_charaCont = GetComponent<CharacterController>();
	}

	private void Update()
    {
	    if (!isLocalPlayer) return;
	    if (_cam == null) SetCameraChild();
	    
	    MovePlayer(Input.GetAxisRaw("Horizontal") * Time.deltaTime, Input.GetAxisRaw("Vertical") * Time.deltaTime);

	    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
	    
	    _cam.transform.position = transform.position + Vector3.up * 5f;
    }
    
    [Command]
    void MovePlayer(float horizontalMove, float verticalMove)
    {
	    transform.LookAt(transform.position + new Vector3(horizontalMove, 0f, verticalMove));
	    _charaCont.Move(new Vector3(horizontalMove * speed, 0f, verticalMove * speed));
    }
    
    public void SetCameraChild()
    {
	    _cam = FindObjectOfType<Camera>().gameObject;
	    _cam.transform.position = transform.position + Vector3.up * 5f;
    }
}

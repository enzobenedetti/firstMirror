using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Mirror;
using UnityEditor.Timeline.Actions;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class PlayerMovement : NetworkBehaviour
{
	public float speed = 10f;
	public GameObject lightLamp;
	public GameObject hiderLamp;
	private CharacterController _charaCont;
	private CatchHider _catchHider;
	private GameObject _cam;

	private void Awake()
	{
		_charaCont = GetComponent<CharacterController>();
		_catchHider = GetComponent<CatchHider>();
	}

	private void Start()
	{
		if (!isLocalPlayer) return;

		if (isClientOnly)
		{
			Debug.Log("pppopopopop");
			GameObject lamp = Instantiate(lightLamp, transform);
			_catchHider.isHider = false;
		}
		else
		{
			GameObject lamp = Instantiate(hiderLamp, transform);
			_catchHider.isHider = true;
		}
		
		SetCameraChild();
	}

	void SetCameraChild()
	{
		_cam = FindObjectOfType<Camera>().gameObject;
		_cam.transform.position = transform.position + Vector3.up * 5f;
	}

	#region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    {
	    
    }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient()
    {
    }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() {}

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion

    private void Update()
    {
	    if (!isLocalPlayer) return;

	    if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
	    {
		    MovePlayer(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	    }
	    
	    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
	    
	    _cam.transform.position = transform.position + Vector3.up * 5f;
    }
    
    [Command]
    void MovePlayer(float horizontalMove, float verticalMove)
    {
	    //transform.position += new Vector3(horizontalMove * speed * Time.deltaTime, 0f, verticalMove * speed * Time.deltaTime);
	    transform.LookAt(transform.position + new Vector3(horizontalMove, 0f, verticalMove));
	    _charaCont.Move(new Vector3(horizontalMove * speed * Time.deltaTime, 0f, verticalMove * speed * Time.deltaTime));
    }
}

using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

namespace Test
{
	public class PlayerMovement : NetworkBehaviour
	{
		public float speed = 3f;
		private double _netTime = -1f;
		private double _deltaNetTime = 0f;
		public GameObject bulletPrefab;
		[SyncVar(hook = "SetColor")]
		public Color color = Color.white;

		private Material cachedMaterial;
	
		#region Start & Stop Callbacks

		/// <summary>
		/// This is invoked for NetworkBehaviour objects when they become active on the server.
		/// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
		/// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
		/// </summary>
		public override void OnStartServer()
		{
			_netTime = NetworkTime.time;
			color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
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
		public override void OnStopClient() { }

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
			_deltaNetTime = NetworkTime.time - _netTime;
			_netTime = NetworkTime.time;
	    
			if (isLocalPlayer)
			{
				if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
				{
					MovePlayer(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
				}

				if (Input.GetButtonDown("Jump"))
				{
					ShootBullet();
				}
			}
		}

		[Command]
		void MovePlayer(float horizontalMove, float verticalMove)
		{
			transform.position +=
				new Vector3(horizontalMove * speed * (float)_deltaNetTime, 0f, verticalMove * speed * (float)_deltaNetTime);
			transform.LookAt(transform.position + new Vector3(horizontalMove, 0f, verticalMove));
		}

		[Command]
		void ShootBullet()
		{
			Debug.Log("Piou piou");
			GameObject newBullet = Instantiate(bulletPrefab, transform.forward*1.5f + transform.position, transform.rotation);
			NetworkServer.Spawn(newBullet);
		}
    
		void SetColor(Color oldColor, Color newColor)
		{
			if (cachedMaterial == null) cachedMaterial = GetComponentInChildren<MeshRenderer>().material;
			cachedMaterial.color = newColor;
		}

		private void OnDestroy()
		{
			Destroy(cachedMaterial);
		}
	}
}

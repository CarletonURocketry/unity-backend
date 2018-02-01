using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Rocket : MonoBehaviour {
	
	private SocketIOComponent socket;
	private Transform parent;
	private Vector3 targetPos;
	private Vector3 targetRot;
	private float thrust;

	// Use this for initialization
	void Start () {
		socket = this.GetComponent<SocketIOComponent>();
		parent = this.GetComponent<Transform> ();
		socket.On("rocket_state", RocketState);
	}

	public void RocketState(SocketIOEvent e)
	{
		JSONObject pos = e.data.GetField ("position");
		JSONObject rot = e.data.GetField ("attitude");
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;
		float roll = 0.0f;
		float pitch = 0.0f;
		float yaw = 0.0f;
		float.TryParse (pos.GetField ("x").ToString(), out x);
		float.TryParse (pos.GetField ("y").ToString(), out y);
		float.TryParse (pos.GetField ("z").ToString(), out z);
		float.TryParse (rot.GetField ("roll").ToString(), out roll);
		float.TryParse (rot.GetField ("pitch").ToString(), out pitch);
		float.TryParse (rot.GetField ("yaw").ToString(), out yaw);
		float.TryParse (e.data.GetField("thrust").ToString(), out thrust);
		targetPos = new Vector3 (x, z, y);
		targetRot = new Vector3 (roll, yaw, pitch);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 oldpos = parent.position;
		parent.position = targetPos;
		parent.localRotation = Quaternion.Euler (targetRot);
		Color c = new Color (1.0f, 1.0f * (thrust), 0);
		Debug.Log (thrust);
		if (Vector3.Magnitude (oldpos - parent.position) < 1.0) {
			Debug.DrawLine (oldpos, parent.position, c, 100.0f, false);
		}
	}
	
}

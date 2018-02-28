using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {
	static SocketIOComponent socket;
	public GameObject playerPrefab;

    Dictionary<string, GameObject> players;

	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("spawn player", OnSpawned);
        socket.On("disconnected", OnDisconnected);
        socket.On("move", OnMove);
        players = new Dictionary<string, GameObject>();
	}
	
	// Tells us we are connected
	void OnConnected (SocketIOEvent e) {
		Debug.Log ("We are Connected");
		socket.Emit ("playerhere");
	}

	void OnSpawned(SocketIOEvent e){
		Debug.Log ("Player Spawned!" + e.data);
		var player = Instantiate (playerPrefab);
        players.Add(e.data["id"].ToString(), player);
        Debug.Log("Count " + players.Count);
	}

    void OnDisconnected (SocketIOEvent e)
    {
        Debug.Log("player disconnected " + e.data);
        var id = e.data["id"].ToString();
        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }

    void OnMove(SocketIOEvent e)
    {
        Debug.Log("Networked player is moving " + e.data);

        var id = e.data["id"].ToString();
        var player = players[id]; 
        Debug.Log(id);
        var pos = new Vector3(GetFloatFromJson(e.data, "x"), 0,GetFloatFromJson(e.data, "y"));
        var h = GetFloatFromJson(e.data, "h");
        var v = GetFloatFromJson(e.data, "v");
        Debug.Log("pos: " + pos);
        var netMove = player.GetComponent<CharacterMovement>();

        netMove.NetworkMovement(pos, h, v);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }
}

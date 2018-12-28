using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated; // added DKE
using BeardedManStudios.Forge.Networking.Unity;     // added DKE
using UnityEngine.SceneManagement;                  // added DKE
using Goodgulf;                                     // Added DKE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerReady : PlayerReadyBehavior {// changed to PlayerReadyBehavior DKE

    private int playerCount = 0;

    private PlayersInfo players;

    // Use this for initialization
    void Start () {

        GameObject go = new GameObject("Players");
        players = go.AddComponent<PlayersInfo>();

        DontDestroyOnLoad(players);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PlayerDetails(string playerName, string tankName)
    {
        Debug.Log("PlayerDetails() playerName="+playerName);
        Debug.Log("PlayerDetails() tankName=" + tankName);

        networkObject.SendRpc(RPC_SEND_PLAYER_DETAILS, Receivers.All, playerName, tankName);
    }



    // Receive the RPC
    public override void SendPlayerDetails(RpcArgs args)
    {
        MainThreadManager.Run(() =>
        {
            string playerName = args.GetNext<string>();
            string tankName = args.GetNext<string>();

            uint playerID = args.Info.SendingPlayer.NetworkId;

            PlayerInfo pi = new PlayerInfo();
            pi.PlayerID = playerID;
            pi.PlayerName = playerName;
            pi.TankName = tankName;

            players.AddPlayerInfo(pi); // We're currently adding the players to the list in both client(s) and server.


            if (networkObject.IsServer)
            {
        
                playerCount++;

                if(playerCount == 2)    // Now fixed to 2 players
                {

                    Debug.Log("Another client <" + playerName+" ,"+playerID + "> is ready.");

                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else Debug.Log("Another client <"+playerName+"> is ready.");

        });
    }
}

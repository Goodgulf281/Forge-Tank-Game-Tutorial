using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections.Generic;
using Goodgulf;                                          // Added DKE
using UnityEngine;                                       // Added DKE
using System.Linq;                                       // Added DKE
using UnityEngine.SceneManagement;                       // Added DKE
using System.IO;                                         // Added DKE
using System;                                            // Added DKE
using BeardedManStudios.Forge.Networking;                // Added DKE


namespace AuthMovementExample
{
    /// <summary>
    /// The singleton Game object which oversees the whole game state
    /// </summary>
    public class GameManager : GameManagerBehavior
    {
        // Singleton instance
        public static GameManager Instance;

        private PlayersInfo playersInfo;                // Added DKE
        private int playerCount = 0;                    // Added DKE
        private List<StartPosition> startPositions;        // Added DKE
        

        // List of players
        private readonly Dictionary<uint, PlayerBehavior> _playerObjects = new Dictionary<uint, PlayerBehavior>();

        private bool _networkReady;


        static void WriteString(string line)
        {
            string path = "C:/Temp/log.txt";

            File.AppendAllText(path, DateTime.Now.ToString() + ": " + line + Environment.NewLine);
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(Instance);


            GameObject go = GameObject.Find("Players");                                         // Added DKE
            if (go != null)                                                                     // Added DKE
            {                                                                                   // Added DKE
                playersInfo = go.GetComponent<PlayersInfo>();                                   // Added DKE
                if (playersInfo == null)                                                        // Added DKE
                    Debug.LogError("GameManager: No PlayersInfo component in Scene");           // Added DKE
                else WriteString("PlayersInfo found");                                          // Added DKE
            }                                                                                   // Added DKE
            else Debug.LogError("GameManager: No Players Object in Scene");                     // Added DKE

            // Add all potential start positions in a list
            startPositions = new List<StartPosition>();                                                                                                         // Added DKE
            SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => startPositions.AddRange(g.GetComponentsInChildren<StartPosition>()));      // Added DKE



        }
        
        protected override void NetworkStart()
        {
            base.NetworkStart();

            if (NetworkManager.Instance.IsServer)
            {
                WriteString("GameManager.NetworkStart(): IsServer=true");
                WriteString("GameManager.NetworkStart(): Networker player count=" + NetworkManager.Instance.Networker.Players.Count);

                int index = 0; // count the number of players being instantiated

                for (int i = 0; i < NetworkManager.Instance.Networker.Players.Count; i++)
                {

                    // Retrieve player from connected Players
                    NetworkingPlayer player = NetworkManager.Instance.Networker.Players[i];

                    if (playersInfo != null)
                    {
                        // Can we match connected player with PlayerInfo created in selection screen?
                        PlayerInfo pi = playersInfo.GetPlayerInfo(player.NetworkId);

                        if (pi != null)
                        {
                            Debug.Log("Found player " + pi.PlayerName + " + " + pi.TankName);
                            WriteString("Found player " + pi.PlayerName + " + " + pi.TankName);

                            if (pi.TankName == "Sherman")
                            {
                                index = 0;
                                WriteString("GameManager.NetworkStart(): tank=Sherman");

                            }
                            else
                            {
                                index = 1;
                                WriteString("GameManager.NetworkStart(): tank=Tiger");
                            }

                            // Now find startposition
                            Vector3 startHere;
                            if (playerCount < startPositions.Count)
                            {
                                startHere = startPositions[playerCount].transform.position;
                                playerCount++;
                            }
                            else startHere = new Vector3(0, 0, 0);

                            WriteString("GameManager.NetworkStart():Startvector=" + startHere.ToString());

                            PlayerBehavior p = NetworkManager.Instance.InstantiatePlayer(index, startHere);    // Changed DKE to include index and start position
                            p.networkObject.ownerNetId = player.NetworkId;
                            _playerObjects.Add(player.NetworkId, p);

                        }
                        else
                        {
                            // This will fire for the server
                            Debug.LogWarning("GameManager.NetworkStart(): Could not find Player Info for " + player.NetworkId);
                            WriteString("GameManager.NetworkStart(): Could not find Player Info for " + player.NetworkId);
                        }
                    }
                    else WriteString("GameManager.NetworkStart():playerInfo==null");


                }

                NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
                {
                    // Remove the player from the list of players and destroy it
                    PlayerBehavior p = _playerObjects[player.NetworkId];
                    _playerObjects.Remove(player.NetworkId);
                    p.networkObject.Destroy();
                };
                
            }
            else
            {
                // This is a local client - it needs to listen for input
                WriteString("GameManager.NetworkStart(): IsServer=false");
                NetworkManager.Instance.InstantiateInputListener();
            }


            _networkReady = true;
        }
		
		// Force NetworkStart to happen - a work around for NetworkStart not happening
        // for objects instantiated in scene in the latest version of Forge
        private void FixedUpdate()
        {
            if (!_networkReady && networkObject != null)
            {
                NetworkStart();
            }
        }
    }
}
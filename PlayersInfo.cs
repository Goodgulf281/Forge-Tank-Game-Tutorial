using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goodgulf
{

    //
    // This class is used to keep track of all players in the game. It's created in the tank selection screen and persistent through the scenes after that.
    //

    public class PlayerInfo
    {
        private uint playerID;
        private string playerName;
        private string tankName;

        public uint PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                playerID = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
            }
        }

        public string TankName
        {
            get
            {
                return tankName;
            }
            set
            {
                tankName = value;
            }
        }
    }


    public class PlayersInfo : MonoBehaviour
    {

        public List<PlayerInfo> players = new List<PlayerInfo>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddPlayerInfo (PlayerInfo pi)
        {
            players.Add(pi);

            // for debug
            //GameObject go = new GameObject(pi.PlayerName);
            //go.transform.SetParent(this.transform);

        }

        public PlayerInfo GetPlayerInfo(uint playerID)
        {
            return players.Find( x => x.PlayerID == playerID);
        }

    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectTank : MonoBehaviour {

    public Camera mainCamera;
    public Text userName;
    public PlayerReady playerReady;

    public string selectedTank;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...

            //Debug.Log("Click");

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // the object identified by hit.transform was clicked

                //Debug.Log("Hit");

                if (hit.transform.gameObject.GetComponentInParent<AutoRotateTanks>()!=null) {

                    //Debug.Log("Stop rotation");

                    hit.transform.gameObject.GetComponentInParent<AutoRotateTanks>().speed = 0;

                    GameObject go = hit.transform.gameObject.GetComponentInParent<AutoRotateTanks>().gameObject;
                    selectedTank = go.name;

                }

            }
        }
    }

    public void StartButtonClick()
    {
        string playerName = userName.text.ToString();

        if(playerName.Length>1 && selectedTank.Length>1)
        {

            Debug.Log(playerName + " selected " + selectedTank);

            if(playerReady!=null)
            {
                Debug.Log("Send RPC");
                playerReady.PlayerDetails(playerName, selectedTank);

            }

        }


    }


}

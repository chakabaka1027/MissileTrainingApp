using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneNavigator : MonoBehaviour  {


    public void LoadNextScene(string sceneName){

    //if you are loading into a new scene and youve started a multiplayer session, destroy the instance of the session
        if (GameObject.Find("LobbyManager") != null) {
            NetworkManager.singleton.StopClient ();
            NetworkManager.singleton.StopHost ();
 
            NetworkLobbyManager.singleton.StopClient ();
            NetworkLobbyManager.singleton.StopServer ();
            Network.Disconnect();
            NetworkServer.DisconnectAll();
        }

        SceneManager.LoadScene(sceneName);
        if(sceneName == "AR Model"){
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.AR;
        } else if(sceneName == "3D Model"){
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.nonAR;
        } else {
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.other;
        }
    }




    public void CloseApp(){
        Application.Quit();
    }
}

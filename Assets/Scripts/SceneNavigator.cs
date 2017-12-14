using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneNavigator : MonoBehaviour  {

    
    public void LoadNextScene(string sceneName){
        if(sceneName == "Sign In" && FindObjectOfType<LrsCommunicator>() != null){
            FindObjectOfType<LrsCommunicator>().ReportLogOut();
            Destroy(FindObjectOfType<LrsCommunicator>().gameObject);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void AssignViewType(string viewType){
        if (viewType == "AR Model") {
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.AR;
        } else if (viewType == "3D Model") {
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.nonAR;
        } else {
            FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.other;
        }    
    }

    public void ReturnHome(string sceneName){
        if (GameObject.Find("LobbyManager") != null) {
            //if(!Network.isServer){
                Debug.Log("server disconnect");
                NetworkManager.singleton.StopClient ();
                NetworkManager.singleton.StopHost ();
 
                NetworkLobbyManager.singleton.StopClient ();
                NetworkLobbyManager.singleton.StopServer ();
                NetworkServer.DisconnectAll();
                //NetworkServer.SetAllClientsNotReady();

            //} else if(!Network.isClient){
            //    Debug.Log("client disconnect");
            //    NetworkManager.singleton.StopClient ();
            //    //NetworkManager.singleton.StopHost ();
 
            //    NetworkLobbyManager.singleton.StopClient ();
            //    //NetworkLobbyManager.singleton.StopServer ();
            //    Network.Disconnect();
            //}

            if(GameObject.Find("LobbyManager") != null){
                Destroy(GameObject.Find("LobbyManager")); 
            }

        }

        SceneManager.LoadScene(sceneName);
       
        FindObjectOfType<LrsCommunicator>().viewType = LrsCommunicator.ViewType.other;
        

    }

    public void LoadSoloGame(string sceneName){
        SceneManager.LoadScene(sceneName);

        if(GameObject.Find("LobbyManager") != null){
            Destroy(GameObject.Find("LobbyManager")); 
        }
    }

    public void CloseApp(){
        FindObjectOfType<LrsCommunicator>().ReportLogOut();

        Application.Quit();
    }
}

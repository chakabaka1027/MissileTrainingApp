using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManagerSetup : MonoBehaviour {

    private static LobbyManagerSetup lobbyManager;
    public Button soloButton;
    public Text titleText;
    LrsCommunicator lrsCommunicator;

    string viewType;

    public static LobbyManagerSetup Instance{
        get{ 
            return lobbyManager; 
        }
    }

    void Awake(){
        DestroyDuplicates();
    }

    void Start(){
        lrsCommunicator = FindObjectOfType<LrsCommunicator>();
        ChangeTitleText();
        AssignSoloScene(viewType);
        AssignMultiplayerScene();
    }

    void DestroyDuplicates(){
        if (lobbyManager == null)
            lobbyManager = this;
        else if (lobbyManager != this) 
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AssignMultiplayerScene(){
        string currentScene = SceneManager.GetActiveScene().name;
        if(lrsCommunicator.viewType == LrsCommunicator.ViewType.nonAR){
            FindObjectOfType<NetworkLobbyManager>().playScene = "3D Model";
        } else if(lrsCommunicator.viewType == LrsCommunicator.ViewType.AR){
            FindObjectOfType<NetworkLobbyManager>().playScene = "AR Model";
        }     
    }

    void AssignSoloScene(string soloScene){
        soloButton.onClick.AddListener(delegate { FindObjectOfType<SceneNavigator>().LoadSoloGame(soloScene + " Solo"); }); 
    }

    void ChangeTitleText(){
        if(lrsCommunicator.viewType == LrsCommunicator.ViewType.nonAR){
            viewType = "3D Model";
        } else if(lrsCommunicator.viewType == LrsCommunicator.ViewType.AR){
            viewType = "AR Model";
        }     

        titleText.text = viewType + " Lobby";
    }

}

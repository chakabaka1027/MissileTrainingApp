using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SignIn : MonoBehaviour {
	
    [HideInInspector]
    public bool submittedPlayerInfo = false;

    LrsCommunicator lrsScript;

    GameObject emailErrorMessage;
    InputField playerName;
    InputField playerEmail;
    Button submitButton;

    EventSystem system;

	// Use this for initialization
	void Start () {
		playerName = GameObject.Find("NameField").GetComponent<InputField>();
		playerEmail = GameObject.Find("EmailField").GetComponent<InputField>();
        submitButton = GameObject.Find("SubmitButton").GetComponent<Button>();
        emailErrorMessage = GameObject.Find("Email Error Message");
        emailErrorMessage.SetActive(false);
        
        system = EventSystem.current;

        lrsScript = FindObjectOfType<LrsCommunicator>();        
	}
	
    void Update(){

        //tab between text fields
        if (Input.GetKeyDown(KeyCode.Tab) && system.currentSelectedGameObject != null){
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
         
            if (next != null){
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null){
                    //if it's an input field, also set the text caret
                    inputfield.OnPointerClick(new PointerEventData(system));
                }
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            } else {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null){
                    //if it's an input field, also set the text caret
                    inputfield.OnPointerClick(new PointerEventData(system));  
                }
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
         
        }
        if(playerName.text.Length > 0 && playerEmail.text.Length > 0){
            submitButton.interactable = true;
        } else {
            submitButton.interactable = false;
        }
    }

	public void SubmitPlayerInformation(){
        lrsScript.playerName = playerName.text;
        lrsScript.email = playerEmail.text;
        lrsScript.ReportLogIn();

        if(lrsScript.lrsError){
            emailErrorMessage.SetActive(true);
        } else if (!lrsScript.lrsError){
            SceneManager.LoadScene("Menu");
        }
    }

    public void DemoModeSignIn(){
        SceneManager.LoadScene("Menu");
    }

}
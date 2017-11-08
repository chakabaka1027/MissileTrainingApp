using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    Toggle quizCompletedToggle;
    Button quizButton;

    public Color completedColor;

	// Use this for initialization
	void Start () {
        quizCompletedToggle = GameObject.Find("Completed Quiz Toggle").GetComponent<Toggle>();
        quizButton = GameObject.Find("Quiz").GetComponent<Button>();

		if(FindObjectOfType<LrsCommunicator>() != null){
            if(FindObjectOfType<LrsCommunicator>().wasSuccessful){
                quizCompletedToggle.isOn = true;
                quizCompletedToggle.gameObject.transform.Find("Background").GetComponent<Image>().color = completedColor;
                quizButton.interactable = false;
            } else if (!FindObjectOfType<LrsCommunicator>().wasSuccessful){
                quizCompletedToggle.isOn = false;
            }
        }
	}

}

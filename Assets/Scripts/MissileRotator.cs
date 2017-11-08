using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileRotator : MonoBehaviour {

    public GameObject guts;
    public GameObject externals;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Rotate(){
        float sliderValue = GetComponent<Slider>().value;
        guts.transform.rotation = Quaternion.Euler(sliderValue * -360, 0, 0);
        externals.transform.rotation = Quaternion.Euler(sliderValue * -360, 0, 0);
    }
}

// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AddedTimeControl : MonoBehaviour
{
    [SerializeField] Slider mySlider;
    [SerializeField] Text myText;

    public Text debugText;

    bool isPaused = false;
    bool isDebugEnabled = false;

    void Update() 
    {
        // sets text value above slider to slider value
        myText.text = mySlider.value.ToString("0.00"); 

        if (isDebugEnabled == true) {
            debugText.text = "Debug Enabled!";
        } else if (isDebugEnabled == false) {
            debugText.text = "Debug Disabled!";
        }
    }

    public void changeSpeed() 
    {
        // changes time based on slider value
        if (isPaused == false) 
        {
            Time.timeScale = mySlider.value; 
        }
    }

    public void Pause() 
    {
        // if pause is false, set time to 0 & change pause to true
        if (isPaused == false) 
        {
            Time.timeScale = 0f; 
            isPaused = true;
            isDebugEnabled = false;
        } 
          
        // if pause is true, set time to current slider value & change pause to false
        else if (isPaused == true) 
        {
            Time.timeScale = mySlider.value; 
            isPaused = false;
        }
    }

    public void Debug()
    {
        // if debug is false & pause is false, set time to 100 & change debug to true
        if (isDebugEnabled == false && isPaused == false) 
        {
            Time.timeScale = 100f; 
            isDebugEnabled = true;
        } 
        
        // if debug is true, change time to current slider value & change debug to false
        else if (isDebugEnabled == true) 
        {
            Time.timeScale = mySlider.value; 
            isDebugEnabled = false;
        }
    }
}

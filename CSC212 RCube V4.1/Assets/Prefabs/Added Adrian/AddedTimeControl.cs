// Copyright 2023
// Created by Adrian Damik

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Manages the speed control features for the simulator.
/// </summary>
public class AddedTimeControl : MonoBehaviour
{
    [SerializeField] Slider mySlider;
    [SerializeField] Text myText;

    /// <summary>
    /// Text box to display current debug speed state.
    /// </summary>
    public Text debugText;

    /// <summary>
    /// Used to keep track whether or not the simulator is paused.
    /// </summary>
    bool isPaused = false;

    /// <summary>
    /// Used to keep track whether or not the debug speed is enabled.
    /// </summary>
    bool isDebugEnabled = false;

    /// <summary>
    /// Updates the text box above the speed slider to display the current speed, and checks to see whether or not the debug function is enabled for another text display.
    /// </summary>
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

    /// <summary>
    /// Adjusts the time scale of the simulation to match the current value of the slider.
    /// </summary>
    public void changeSpeed() 
    {
        // changes time based on slider value
        if (isPaused == false) 
        {
            Time.timeScale = mySlider.value; 
        }
    }

    /// <summary>
    /// Pauses the simulation if the UI pause button is pressed. If pause is currently enabled, pressing the button again will unpause the simulator.
    /// </summary>
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

    /// <summary>
    /// Enables the debug speed if the UI debug button is pressed. If debug speed is currently enabled, pressing the button again will disable the debug speed.
    /// </summary>
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

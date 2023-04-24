// Copyright 2023
// Created by Adrian Damik

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managed the quit function
/// </summary>
public class Quit : MonoBehaviour
{
    
    /// <summary>
    /// Checks to see if the Escape button has been pressed. If so, then the application is closed. This function was created for the exe.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

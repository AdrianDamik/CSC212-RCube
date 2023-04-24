// Copyright 2023
// Created by Adrian Damik

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created 2/16/23
// This work is based on the following video: https://www.youtube.com/watch?v=YUcvy9PHeXs

/// <summary>
/// Keeps track of the amount of movements the cube model does when shuffling and solving.
/// </summary>
public class AddedCounter : MonoBehaviour
{

    /// <summary>
    /// Keeps track of the occurance of the AddedCounter script when functions from the script are called in different scripts.
    /// </summary>
    public static AddedCounter instance;

    /// <summary>
    /// UI Text used to displaying counter.
    /// </summary>
    public Text counterText;

    /// <summary>
    /// Counter used to keep track of the current amount of moves.
    /// </summary>
    int counter = 0;

    /// <summary>
    /// Instance is set when the simulator is first initialized.
    /// </summary>
    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Sets current steps to 0 upon starting the simulation.
    /// </summary>
    void Start()
    {
        counterText.text = counter.ToString() + " Steps";
    }

    /// <summary>
    /// Adds a step to the counter each time it's called. This function is called inside of other scripts to keep track of the amount of moves.
    /// </summary>
    public void AddStep()
    {
        counter += 1;
        counterText.text = counter.ToString() + " Steps";
    }

    /// <summary>
    /// Resets the current value of the counter to zero.
    /// </summary>
    public void ResetSteps()
    {
        counter = 0;
    }
}

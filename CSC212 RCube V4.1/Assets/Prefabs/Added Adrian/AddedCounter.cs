// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created 2/16/23
// This work is based on the following video: https://www.youtube.com/watch?v=YUcvy9PHeXs

public class AddedCounter : MonoBehaviour
{

    public static AddedCounter instance;

    public Text counterText;
    int counter = 0;

    private void Awake() {
        instance = this;
    }

    // sets current steps to 0 upon starting the simulation
    void Start()
    {
        counterText.text = counter.ToString() + " Steps";
    }

    // function for adding steps for each move (this will have to be called for each individual step make)
    public void AddStep()
    {
        counter += 1;
        counterText.text = counter.ToString() + " Steps";
    }

    // function for reseting current amount of steps displayed
    public void ResetSteps()
    {
        counter = 0;
    }
}

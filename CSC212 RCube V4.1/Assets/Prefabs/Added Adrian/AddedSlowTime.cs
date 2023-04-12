// Copyright 2023
// Adrian Damik, Elijah Gray, & Aryan Pothanaboyina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class AddedSlowTime : MonoBehaviour
{

    [SerializeField] Slider mySlider;
    
    bool isPaused = false;
    bool isSlow = false;
    bool isVerySlow = false;
    bool isFast = false;
    bool isVeryFast = false;

    public void changeSpeed() 
    {
      Time.timeScale = mySlider.value;
    }

       public void Slow_1() {
        if (isSlow == false) {
            isPaused = false;
            isVerySlow = false;
            isFast = false;
            isVeryFast = false;

            Time.timeScale = 0.5f;
            isSlow = true;

          } else if (isSlow == true) {
            Time.timeScale = 1f;
            isSlow = false;
          }
       }

       public void Slow_2() {
        if (isVerySlow == false) {
            isPaused = false;
            isSlow = false;
            isFast = false;
            isVeryFast = false;

            Time.timeScale = 0.25f;
            isVerySlow = true;

          } else if (isVerySlow == true) {
            Time.timeScale = 1f;
            isVerySlow = false;
          }
       }

       public void Fast_1() {
        if (isFast == false) {
            isPaused = false;
            isSlow = false;
            isVerySlow = false;
            isVeryFast = false;

            Time.timeScale = 1.5f;
            isFast = true;

          } else if (isFast == true) {
            Time.timeScale = 1f;
            isFast = false;
          }
       }

       public void Fast_2() {
        if (isVeryFast == false) {
            isPaused = false;
            isSlow = false;
            isVerySlow = false;
            isFast = false;

            Time.timeScale = 2f;
            isVeryFast = true;

          } else if (isVeryFast == true) {
            Time.timeScale = 1f;
            isVeryFast = false;
          }
       }

       public void Pause() {
        if (isPaused == false) {
            Time.timeScale = 0f;
            isPaused = true;

          } else if (isPaused == true) {
            Time.timeScale = 1f;
            isPaused = false;
          }
       }
}
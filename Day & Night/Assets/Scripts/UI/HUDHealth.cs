using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealth : MonoBehaviour {
    public Slider slider;

    void Start() {
        SetMaxHealth(100);
    }

    public void SetMaxHealth(int HP) {
        slider.maxValue = HP;
        slider.value = HP;
    }

    public void SetHealth(int health) {
        slider.value = health;
    }
}
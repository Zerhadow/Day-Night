using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public GameObject healthBarUI;
    public Slider slider;
    public GameObject enemy;
    EnemyController stats;
    
    // Start is called before the first frame update
    void Start()
    {
        stats = enemy.GetComponent<EnemyController>();
        slider.value = stats.currHP;
        slider.maxValue = stats.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = stats.currHP;

        if(stats.currHP == 0) return;

        if(stats.currHP < stats.maxHP) healthBarUI.SetActive(true);
    }
}
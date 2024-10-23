using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSliderHandler : MonoBehaviour
{
    [SerializeField] private GameMaster master_;
    
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(master_.SetSpeedMultiplier);

        slider.value = PlayerPrefs.GetFloat("GameSpeed", 1);
    }
}

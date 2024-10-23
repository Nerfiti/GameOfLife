using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{
    private Setting[] settings_;
    void Start()
    {
        settings_ = GetComponentsInChildren<Setting>();
    }

    public void SaveAll()
    {
        foreach (var setting in settings_)
        {
            setting.Save();
        }
        
        PlayerPrefs.Save();
    }
}

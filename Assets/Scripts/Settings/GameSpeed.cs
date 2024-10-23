using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

class GameSpeedSetting : MonoBehaviour, Setting
{
    [SerializeField] private Slider slider_;

    void Start()
    {
        slider_.value = PlayerPrefs.GetFloat("GameSpeed", 1);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("GameSpeed", slider_.value);
    }
}
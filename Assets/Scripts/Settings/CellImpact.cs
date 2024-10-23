using UnityEngine;
using UnityEngine.UI;

class CellImpactSetting : MonoBehaviour, Setting
{
    [SerializeField] private InputField impact_;

    public void Start()
    {
        impact_.text = PlayerPrefs.GetFloat("CellImpact", 0.05f).ToString();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("CellImpact", float.Parse(impact_.text));
    }
}
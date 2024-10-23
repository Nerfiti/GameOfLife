using UnityEngine;
using UnityEngine.UI;

class MaxScoreSetting : MonoBehaviour, Setting
{
    [SerializeField] private InputField score_;

    void Start()
    {
        score_.text = PlayerPrefs.GetInt("MaxScore", 100).ToString();
        Validate();
    }

    public void Validate()
    {
        var value = 0;
        try
        { value = int.Parse(score_.text); }
        catch
        { value = 0; }

        if (value <= 0)
        {
            score_.text = "100";
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("MaxScore", int.Parse(score_.text));
    }
}
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

class BoardSizeSetting : MonoBehaviour, Setting
{    
    [SerializeField] private InputField width_;
    [SerializeField] private InputField height_;

    private const int default_width_ = 20;
    private const int default_height_ = 40;

    void Start()
    {
        width_.text = PlayerPrefs.GetInt("BoardWidth", default_width_).ToString();
        height_.text = PlayerPrefs.GetInt("BoardHeight", default_height_).ToString();

        ValidateHeight();
        ValidateWidth();
    }

    public void ValidateWidth()
    {
        var value = 0;

        try 
        { value = int.Parse(width_.text); }
        catch (System.Exception) 
        { width_.text = default_width_.ToString(); }

        if (value <= 0)
        {
            width_.text = default_width_.ToString();
        }
    }

    public void ValidateHeight()
    {
        var value = 0;

        try 
        { value = int.Parse(height_.text); }
        catch (System.Exception) 
        { width_.text = default_height_.ToString(); }

        if (value <= 0)
        {
            height_.text = default_height_.ToString();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("BoardWidth", int.Parse(width_.text));
        PlayerPrefs.SetInt("BoardHeight", int.Parse(height_.text));
    }
}
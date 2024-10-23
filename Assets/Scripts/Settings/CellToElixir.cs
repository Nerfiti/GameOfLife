using UnityEngine;
using UnityEngine.UI;

class CellToElixirSetting : MonoBehaviour, Setting
{
    [SerializeField] private InputField cell_to_elixir_;

    private float default_value = 0.01f;

    void Start()
    {
        cell_to_elixir_.text = PlayerPrefs.GetFloat("CellToElixir", default_value).ToString();
    }

    public void Validate()
    {
        float value = 0;
        try
        { value = float.Parse(cell_to_elixir_.text); }
        catch
        { value = 0; }

        if (value <= 0)
        {
            cell_to_elixir_.text = default_value.ToString();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("CellToElixir", float.Parse(cell_to_elixir_.text));
    }
}
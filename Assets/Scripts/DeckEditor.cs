using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckEditor : MonoBehaviour
{
    [SerializeField] private RawImage pixel_canvas_;
    [SerializeField] private Button save_button_;
    [SerializeField] private Button exit_button_;
    [SerializeField] private Button reset_button_;
    [SerializeField] private Color first_color_ = Color.black;
    [SerializeField] private Color second_color_ = Color.white;

    [SerializeField] private InputField filename_;

    [SerializeField] private EditDeckNotification notifications_;

    private Texture2D pixel_texture_;

    void Start()
    {
        pixel_texture_ = new Texture2D(10, 10);
        pixel_texture_.filterMode = FilterMode.Point;
        pixel_texture_.wrapMode   = TextureWrapMode.Clamp;
        pixel_canvas_.texture = pixel_texture_;

        Reset();

        save_button_.onClick.AddListener(SaveTexture);
        exit_button_.onClick.AddListener(LoadMainMenu);
        reset_button_.onClick.AddListener(Reset);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(pixel_canvas_.rectTransform, Input.mousePosition, null, out localPoint);
            Vector2 texturePoint = new Vector2(localPoint.x / pixel_canvas_.rectTransform.rect.width, localPoint.y / pixel_canvas_.rectTransform.rect.height);

            int x = Mathf.FloorToInt(texturePoint.x * pixel_texture_.width);
            int y = Mathf.FloorToInt(texturePoint.y * pixel_texture_.height);

            if (x >= 0 && x < pixel_texture_.width && y >= 0 && y < pixel_texture_.height)
            {
                bool first_color = pixel_texture_.GetPixel(x, y) != first_color_;
                pixel_texture_.SetPixel(x, y, first_color ? first_color_ : second_color_);
                pixel_texture_.Apply();
            }
        }
    }

    void Reset()
    {
        for (int x = 0; x < pixel_texture_.width; ++x)
            for (int y = 0; y < pixel_texture_.height; ++y)
                pixel_texture_.SetPixel(x, y, second_color_);

        pixel_texture_.Apply();
    }

    void SaveTexture()
    {
        string name = filename_.text;
        filename_.text += "(1)";
        if (name.Length == 0)
        {
            notifications_.Show("Enter a label");
            return;
        }
        name = Path.Combine(Application.persistentDataPath, name + ".pattern");

        int cell_count = 0;
        List<Vector2Int> cells = new List<Vector2Int>();
        for (int x = 0; x < pixel_texture_.width; ++x)
        {
            for (int y = 0; y < pixel_texture_.height; ++y)
            {
                if (pixel_texture_.GetPixel(x, y) == first_color_)
                {
                    ++cell_count;
                    cells.Add(new Vector2Int(x, y));
                }
            }
        }

        Pattern pattern = new Pattern(cell_count);
        while (cell_count > 0)
        {
            --cell_count;
            pattern.cells_x[cell_count] = cells[0].x;
            pattern.cells_y[cell_count] = cells[0].y;
            cells.Remove(cells[0]);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(name, FileMode.Create);

        formatter.Serialize(stream, pattern);
        stream.Close();

        notifications_.Show("Success");
}

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}

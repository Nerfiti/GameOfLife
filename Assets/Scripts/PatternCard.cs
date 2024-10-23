using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PatternCard : MonoBehaviour
{
    public Pattern pattern {get; private set;}
    private RawImage pattern_preview_;

    public int cost { get; private set; }

    public void Initialize(Pattern pattern, Color color)
    {
        cost = 0;
        this.pattern = pattern;

        pattern_preview_ = GetComponentInChildren<RawImage>();
        this.pattern = pattern;

        var real_size = pattern.GetSize();
        var max_component = Math.Max(real_size.x, real_size.y);
        var size = new Vector2Int(max_component + 2, max_component + 2);

        Texture2D preview = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
        for (int x = 0; x < size.x; ++x)
            for (int y = 0; y < size.y; ++y)
                preview.SetPixel(x, y, Color.white);

        Vector2Int min_coords = pattern.GetCorner(true);

        for (int i = 0; i < pattern.cells_x.Length; ++i)
        {
            var cell = new Vector2Int(pattern.cells_x[i], pattern.cells_y[i]);
            var position = cell - min_coords;
            position.x += (max_component - real_size.x) / 2 + 1;
            position.y += (max_component - real_size.y) / 2 + 1;
            preview.SetPixel(position.x, position.y, color);

            ++cost;
        }

        GetComponentInChildren<Text>().text = cost.ToString();

        preview.Apply();

        preview.filterMode = FilterMode.Point;
        preview.wrapMode   = TextureWrapMode.Clamp;

        pattern_preview_.texture = preview;
    }
}
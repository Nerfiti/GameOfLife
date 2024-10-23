using System;
using UnityEngine;

[Serializable]
public class Pattern
{
    public int[] cells_x;
    public int[] cells_y;

    public Pattern(int size)
    {
        cells_x = new int[size];
        cells_y = new int[size];
    }

    public Vector2Int GetCorner(bool minimal)
    {
        if (cells_x.Length == 0)
            return new Vector2Int(0, 0);

        Vector2Int max_coord = new Vector2Int(cells_x[0], cells_y[0]);
        Vector2Int min_coord = max_coord;

        for (int i = 0; i < cells_x.Length; ++i)
        {
            max_coord.x = Math.Max(max_coord.x, cells_x[i]);
            max_coord.y = Math.Max(max_coord.y, cells_y[i]);
            min_coord.x = Math.Min(min_coord.x, cells_x[i]);
            min_coord.y = Math.Min(min_coord.y, cells_y[i]);
        }
        
        return minimal ? min_coord : max_coord;
    }

    public Vector2Int GetSize()
    {
        if (cells_x.Length == 0)
            return new Vector2Int(0, 0);

        Vector2Int max_coord = new Vector2Int(cells_x[0], cells_y[0]);
        Vector2Int min_coord = max_coord;

        for (int i = 0; i < cells_x.Length; ++i)
        {
            max_coord.x = Math.Max(max_coord.x, cells_x[i]);
            max_coord.y = Math.Max(max_coord.y, cells_y[i]);
            min_coord.x = Math.Min(min_coord.x, cells_x[i]);
            min_coord.y = Math.Min(min_coord.y, cells_y[i]);
        }
        
        return max_coord - min_coord + new Vector2Int(1, 1);
    }

    public void Center()
    {
        Vector2Int center = (GetCorner(true) + GetCorner(false)) / 2;
        for (int i = 0; i < cells_x.Length; ++i)
        {
            cells_x[i] -= center.x;
            cells_y[i] -= center.y;
        }
    }
}

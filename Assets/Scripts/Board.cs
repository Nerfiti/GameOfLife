using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2Int board_size;
    public GameObject board_field;

    public int first_player_border;
    public int second_player_border;

    public RectInt[] hot_zone_squares;

    void Awake()
    {
        board_size.x = PlayerPrefs.GetInt("BoardWidth", board_size.x);
        board_size.y = PlayerPrefs.GetInt("BoardHeight", board_size.y);

        float x_factor = (float)board_size.x / 20;
        float y_factor = (float)board_size.y / 40;

        for (int i = 0; i < hot_zone_squares.Length; ++i)
        {
            hot_zone_squares[i].x = (int)(x_factor * hot_zone_squares[i].x);
            hot_zone_squares[i].y = (int)(y_factor * hot_zone_squares[i].y);

            hot_zone_squares[i].width  = (int)(x_factor * hot_zone_squares[i].width);
            hot_zone_squares[i].height = (int)(y_factor * hot_zone_squares[i].height);
        }
    }

    public bool IsCellInHotZone(Vector2Int position)
    {
        int x = position.x;
        int y = position.y;
        foreach (var rect in hot_zone_squares)
        {
            int x_min = rect.x;
            int x_max = rect.x + rect.width;
            int y_min = rect.y;
            int y_max = rect.y + rect.height;

            if (y >= y_min && y < y_max && x >= x_min && x < x_max)
                return true;
        }
        
        return false;
    }
}
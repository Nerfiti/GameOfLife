using UnityEngine;

public class Cell : MonoBehaviour
{
    private GameMaster observer_;

    public GameObject cell_object { get; private set; }
    public Vector2Int position_on_board { get; private set; }

    public void Initialize(GameObject obj, Vector2Int position, GameMaster observer)
    {
        cell_object = obj;
        position_on_board = position;
        observer_ = observer;
    }

    void OnMouseDown()
    {
        observer_.SpawnPattern(position_on_board);
    }
}
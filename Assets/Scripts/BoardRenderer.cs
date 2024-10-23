using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] private GameObject cell_;
    [SerializeField] private Material empty_cell_material_;
    [SerializeField] private Material first_player_material_;
    [SerializeField] private Material second_player_material_;
    [SerializeField] private Material hot_zone_material_;

    [SerializeField] private Board board_;

    private Cell[,] cells_;
    private Renderer[,] cell_renderers_;

    void Start()
    {
        cells_ = new Cell[board_.board_size.x, board_.board_size.y];
        cell_renderers_ = new Renderer[board_.board_size.x, board_.board_size.y];

        var localScale = new Vector3(1.0f / board_.board_size.x, 1.0f, 1.0f / board_.board_size.y);
        for (int x = 0; x < board_.board_size.x; ++x)
        {
            for (int y = 0; y < board_.board_size.y; ++y)
            {
                var cell = Instantiate(cell_, board_.board_field.transform);
                
                Vector3 plane_scale = board_.board_field.transform.localScale;
                var new_scale = Vector3.Scale(cell.transform.localScale, localScale);
                cell.transform.localScale = new_scale;

                var new_position = new Vector3( (2 * x - board_.board_size.x + 1) * cell.transform.lossyScale.x / 2 / plane_scale.x, 
                                                (               1               ) * cell.transform.lossyScale.y / 2 / plane_scale.y,
                                                (2 * y - board_.board_size.y + 1) * cell.transform.lossyScale.z / 2 / plane_scale.z);
                cell.transform.localPosition = new_position;

                cell_renderers_[x, y] = cell.GetComponent<Renderer>();
                var cell_component = cell.AddComponent<Cell>();
                cell_component.Initialize(cell, new Vector2Int(x, y), FindAnyObjectByType<GameMaster>());

                cells_[x, y] = cell_component;

                if (board_.IsCellInHotZone(new Vector2Int(x, y)))
                {
                    cell = Instantiate(cell_, board_.board_field.transform);
                    cell.transform.localScale = new_scale;
                    cell.transform.localPosition = new_position;
                    cell.GetComponent<Renderer>().material = hot_zone_material_;

                    Destroy(cell.GetComponent<BoxCollider>());
                }

            }
        }
    }

    public void SetState(Vector2Int position, GameMaster.CellState state)
    {
        switch (state)
        {
            case GameMaster.CellState.empty_cell:
                cell_renderers_[position.x, position.y].material = empty_cell_material_;
                break;
            case GameMaster.CellState.first_player_cell:
                cell_renderers_[position.x, position.y].material = first_player_material_;
                break;
            case GameMaster.CellState.second_player_cell:
                cell_renderers_[position.x, position.y].material = second_player_material_;
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameMaster : MonoBehaviour
{
    private const float step_delta_time_ = 0.1f;

    public enum CellState
    {
        first_player_cell,
        second_player_cell,
        empty_cell
    };

    private CellState[,] current_state_;
    private CellState[,] next_state_;
    
    HashSet<Vector2Int> alive_cells_;
    HashSet<Vector2Int> cells_to_check_;

    private bool is_simulation_on_ = true;

    [SerializeField] private BoardRenderer renderer_;

    [SerializeField] private Board board_;

    private Pattern first_selected_pattern_ = null;
    private Pattern second_selected_pattern_ = null;

    [SerializeField] public CardButtonSpawner cards;
    public bool first_player_move;

    private int firsts_in_hot_zone_ = 0;
    private int seconds_in_hot_zone_ = 0;

    private int firsts_on_board_ = 0;
    private int seconds_on_board_ = 0;

    public float first_score_ {get; private set;}
    public float second_score_ {get; private set;}

    public int max_score_;

    [SerializeField] private float cell_impact = 0.01f;

    [SerializeField] private GameOverScreen game_over_;

    [SerializeField] private BotManager bot_manager_;
    [SerializeField] private float elixir_factor_;

    void Awake()
    {
        first_score_ = second_score_ = 0;
        current_state_ = new CellState[board_.board_size.x, board_.board_size.y];
        next_state_ = new CellState[board_.board_size.x, board_.board_size.y];
        
        alive_cells_ = new HashSet<Vector2Int>();
        cells_to_check_ = new HashSet<Vector2Int>();
        Time.fixedDeltaTime = 0.1f;

        if (PlayerPrefs.GetInt("StartWithPause", 0) == 1)
            Pause(true);

        max_score_ = PlayerPrefs.GetInt("MaxScore", max_score_);
        cell_impact = PlayerPrefs.GetFloat("CellImpact", cell_impact);
        elixir_factor_ = PlayerPrefs.GetFloat("CellToElixir", elixir_factor_);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (is_simulation_on_)
            GameStep();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown("p"))
            is_simulation_on_ = !is_simulation_on_;
        if (Input.GetKeyDown("c"))
            Clear();
    }

    void GameStep()
    {
        bot_manager_.AddElixir(firsts_on_board_ * elixir_factor_, true);
        bot_manager_.AddElixir(seconds_on_board_ * elixir_factor_, false);

        if (Math.Max(first_score_, second_score_) >= max_score_)
        {
            GameOverScreen.GameState state;
            if (first_score_ > max_score_ && second_score_ > max_score_)
                state = GameOverScreen.GameState.draw;
            else if (first_score_ > max_score_)
                state = GameOverScreen.GameState.first_player_win;
            else
                state = GameOverScreen.GameState.second_player_win;

            game_over_.ShowScreen(state);
            Pause(true);
            FindObjectOfType<BotManager>().PauseAllBots(true);
            return;
        }
        first_score_  += firsts_in_hot_zone_ * cell_impact;
        second_score_ += seconds_in_hot_zone_ * cell_impact;

        FilterCells();

        foreach (var cell in cells_to_check_)
        {
            var cur_cell = current_state_[cell.x, cell.y];

            bool first_player = cur_cell == CellState.first_player_cell;
            bool second_player = cur_cell == CellState.second_player_cell;
            bool is_alive = first_player || second_player;

            var firsts = CountNeighbors(cell, CellState.first_player_cell);
            var seconds = CountNeighbors(cell, CellState.second_player_cell);
            var neighbors = firsts + seconds;

            if (is_alive && (neighbors < 2 || neighbors > 3))
            {
                SetState(cell, CellState.empty_cell, true);
            }
            else if (neighbors == 3)
            {
                CellState new_state = firsts > 1 ? CellState.first_player_cell : CellState.second_player_cell;
                SetState(cell, new_state, true);

                if (!is_alive)
                    alive_cells_.Add(cell);
            }
            else
            {
                SetState(cell, current_state_[cell.x, cell.y], true);
            }
        }

        var temp = current_state_;
        current_state_ = next_state_;
        next_state_ = temp;
        for (int x = 0; x < board_.board_size.x; ++x)
            for (int y = 0; y < board_.board_size.y; ++y)
                next_state_[x, y] = CellState.empty_cell;
    }

    int CountNeighbors(Vector2Int cell, CellState type)
    {
        int count = 0;
        
        int left  = Math.Max(cell.x - 1, 0);
        int right = Math.Min(cell.x + 1, board_.board_size.x - 1);
        int down  = Math.Max(cell.y - 1, 0);
        int up    = Math.Min(cell.y + 1, board_.board_size.y - 1);

        for (int x = left; x <= right; ++x)
        {
            for (int y = down; y <= up; ++y)
            {
                if (x == cell.x && y == cell.y)
                    continue;

                if (current_state_[x, y] == type)
                    ++count;
            }
        }

        return count;
    }

    void FilterCells()
    {
        cells_to_check_.Clear();
        
        int counter = 0;

        foreach (var cell in alive_cells_)
        {
            int left  = Math.Max(cell.x - 1, 0);
            int right = Math.Min(cell.x + 1, board_.board_size.x - 1);
            int down  = Math.Max(cell.y - 1, 0);
            int up    = Math.Min(cell.y + 1, board_.board_size.y - 1);

            for (int x = left; x <= right; ++x)
            {
                for (int y = down; y <= up; ++y)
                {
                    cells_to_check_.Add(new Vector2Int(x, y));
                    ++counter;
                }
            }
        }
    }

    void Clear()
    {
        for (int x = 0; x < board_.board_size.x; ++x)
        {
            for (int y = 0; y < board_.board_size.y; ++y)
            {
                var position = new Vector2Int(x, y);
                SetState(position, CellState.empty_cell, false);
                SetState(position, CellState.empty_cell, true);
            }
        }

        alive_cells_.Clear();
        cells_to_check_.Clear();
    }

    public void SpawnPattern(Vector2Int position)
    {
        var pattern = first_player_move ? first_selected_pattern_ : second_selected_pattern_;

        if (pattern == null)
            return;

        var pattern_y_offset = pattern.GetCorner(false).y;

        if (first_player_move && position.y + pattern_y_offset >= board_.first_player_border)
            position.y = board_.first_player_border - pattern_y_offset - 1;

        if (!first_player_move && position.y - pattern_y_offset <= board_.second_player_border)
            position.y = board_.second_player_border + pattern_y_offset - 1;

        int cost = 0;
        for (int i = 0; i < pattern.cells_x.Length; ++i)
        {
            var cell_in_figure = new Vector2Int(pattern.cells_x[i], pattern.cells_y[i]);
            ++cost;

            var cell_position = position;
            if (first_player_move)
                cell_position += cell_in_figure;
            else
                cell_position -= cell_in_figure;

            if (board_.IsCellInHotZone(cell_position))
                return;
        }

        if (cost > cards.Elixir())
            return;

        cards.UpdateHand();

        for (int i = 0; i < pattern.cells_x.Length; ++i)
        {
            var cell_in_figure = new Vector2Int(pattern.cells_x[i], pattern.cells_y[i]);

            var cell_position = position;
            var state = CellState.empty_cell;
            if (first_player_move)
            {
                cell_position += cell_in_figure;
                state = CellState.first_player_cell;
            }
            else
            {
                cell_position -= cell_in_figure;
                state = CellState.second_player_cell;
            }
            SetState(cell_position, state);
        }

        if (first_player_move)
            first_selected_pattern_ = null;
        else
            second_selected_pattern_ = null;
    }

    private void SpawnCell(Vector2Int position, CellState state)
    {
        renderer_.SetState(position, state);
    }

    private void SetState(Vector2Int position, CellState state, bool next_state = false)
    {
        if (position.x < 0 || position.x >= board_.board_size.x || 
            position.y < 0 || position.y >= board_.board_size.y)
            return;

        CellState cur_state = current_state_[position.x, position.y];
        if (board_.IsCellInHotZone(position))
            UpdateZoneInfo(cur_state, state);
        
        if (cur_state == CellState.first_player_cell)
            --firsts_on_board_;
        if (cur_state == CellState.second_player_cell)
            --seconds_on_board_;
        if (state == CellState.first_player_cell)
            ++firsts_on_board_;
        if (state == CellState.second_player_cell)
            ++seconds_on_board_;

        if (state == CellState.empty_cell)
            alive_cells_.Remove(position);
        else
            alive_cells_.Add(position);

        if (next_state)
            next_state_[position.x, position.y] = state;
        else
            current_state_[position.x, position.y] = state;

        SpawnCell(position, state);
    }
    
    private void UpdateZoneInfo(CellState old_state, CellState new_state)
    {
        if (old_state == CellState.first_player_cell)
            --firsts_in_hot_zone_;
        else if (old_state == CellState.second_player_cell)
            --seconds_in_hot_zone_;

        if (new_state == CellState.first_player_cell)
            ++firsts_in_hot_zone_;
        else if (new_state == CellState.second_player_cell)
            ++seconds_in_hot_zone_;
    }

    public void SetSpeedMultiplier(float factor)
    {
        Time.fixedDeltaTime = step_delta_time_ / factor;
    }
    
    public void SwitchPause()
    {
        is_simulation_on_ = !is_simulation_on_;
    }

    public void Pause(bool pause)
    {
        is_simulation_on_ = !pause;
    }

    public void Reset()
    {
        Clear();
        firsts_in_hot_zone_ = 0;
        seconds_in_hot_zone_ = 0;

        first_score_ = 0;
        second_score_ = 0;

        Pause(false);
    }

    public void SelectPattern(Pattern pattern, bool first_player)
    {
        if (first_player)
            first_selected_pattern_ = pattern;
        else
            second_selected_pattern_ = pattern;
    }

    public void SpawnPatternInRandomPosition(bool first_player)
    {
        int min_y = first_player ? 0 : board_.second_player_border;
        int max_y = first_player ? board_.first_player_border : board_.board_size.x;
        // max_y = max_y / 3 * 2; //Test for bot

        Vector2Int position = new Vector2Int(UnityEngine.Random.Range(0, board_.board_size.x),
                                             UnityEngine.Random.Range(min_y, max_y));

        SpawnPattern(position);
    }
}

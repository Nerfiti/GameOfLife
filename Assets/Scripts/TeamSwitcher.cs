using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSwitcher : MonoBehaviour
{
    [SerializeField] private Camera first_player_camera_;
    [SerializeField] private Camera second_player_camera_;

    [SerializeField] private Button switch_team_button_;
    
    [SerializeField] private GameMaster game_master_;

    [SerializeField] private BotManager bot_manager_;

    void Start()
    {
        first_player_camera_.enabled = true;
        second_player_camera_.enabled = false;
    }

    public void ChangeTeam()
    {
        first_player_camera_.enabled = !first_player_camera_.enabled;
        second_player_camera_.enabled = !second_player_camera_.enabled;

        game_master_.first_player_move = !game_master_.first_player_move;
    }

    public bool FirstPlayer()
    {
        return first_player_camera_.enabled;
    }
}

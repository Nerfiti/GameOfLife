using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSetter : MonoBehaviour
{
    [SerializeField] private BotManager manager_;

    [SerializeField] private Bot first_player_;
    [SerializeField] private Bot second_player_;

    public enum GameMode
    {
        player_vs_player,
        player_vs_bot,
        bot_vs_bot
    }

    public void SetPlayerVsPlayer()
    {
        SetMode(GameMode.player_vs_player);
    }

    public void SetPlayerVsPC()
    {
        SetMode(GameMode.player_vs_bot);
    }

    public void SetPCVsPC()
    {
        SetMode(GameMode.bot_vs_bot);
    }

    public void SetMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.player_vs_player:
            {
                first_player_.forbid_change_pause_state = true;
                second_player_.forbid_change_pause_state = true;
                break;
            }
            case GameMode.player_vs_bot:
            {
                first_player_.forbid_change_pause_state = true;
                second_player_.forbid_change_pause_state = false;
                break;
            }
            case GameMode.bot_vs_bot:
            {
                first_player_.forbid_change_pause_state = false;
                second_player_.forbid_change_pause_state = false;
                break;
            }
        }
    }
}

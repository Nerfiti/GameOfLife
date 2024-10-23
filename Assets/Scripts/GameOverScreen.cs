using TMPro;
using UnityEngine;
using UnityEngine.UI;

class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen_;
    [SerializeField] private GameObject victory_icon_;

    public enum GameState
    {
        first_player_win,
        second_player_win,
        draw,
        error
    };

    [SerializeField] private string first_player_win_string_;
    [SerializeField] private string second_player_win_string_;
    [SerializeField] private string draw_string_;
    [SerializeField] private string error_string_;

    public void ShowScreen(GameState state)
    {
        var target = screen_.GetComponentInChildren<Text>();
        victory_icon_.SetActive(true);

        switch (state)
        {
            case GameState.first_player_win:
            {
                target.text = first_player_win_string_;
                break;
            }
            case GameState.second_player_win:
            {
                target.text = second_player_win_string_;
                break;
            }
            case GameState.draw:
            {
                victory_icon_.SetActive(false);
                target.text = draw_string_;
                break;
            }
            case GameState.error:
            {
                target.text = error_string_;
                break;
            }
        }

        screen_.SetActive(true);
    }
}
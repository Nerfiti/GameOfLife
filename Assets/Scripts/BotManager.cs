using System;
using System.Collections.ObjectModel;
using UnityEngine;

class BotManager : MonoBehaviour
{
    private Bot[] bots_;
    private int cur_index_;

    [SerializeField] TeamSwitcher switcher_;

    void Start()
    {
        Invoke("Refresh", 0);
    }

    public void Refresh()
    {
        bots_ = GameObject.FindObjectsOfType<Bot>();
        cur_index_ = 0;

        foreach (var bot in bots_)
        {
            if (bot.forbid_change_pause_state)
                cur_index_ = Array.IndexOf(bots_, bot);
        }

        bots_[cur_index_].TurnOnCards(true);

        if (switcher_.FirstPlayer() != bots_[cur_index_].FirstPlayer())
            switcher_.ChangeTeam();

        PauseAllBots(true);
    }

    public void PauseAllBots(bool pause)
    {
        if (bots_ == null)
            Refresh();
        foreach (var bot in bots_)
            bot.Pause(pause);
    }

    public void SwitchBot()
    {
        if (bots_.Length == 0)
        {
            switcher_.ChangeTeam();
            return;
        }

        var old_bot = bots_[cur_index_];

        cur_index_ = (cur_index_ + 1) % bots_.Length;

        var new_bot = bots_[cur_index_];

        old_bot.TurnOnCards(false);
        new_bot.TurnOnCards(true);

        if (old_bot.FirstPlayer() != new_bot.FirstPlayer())
            switcher_.ChangeTeam();
    }

    public void AddElixir(float elixir, bool first_player)
    {
        if (bots_ == null)
            Refresh();
        foreach (var bot in bots_)
            if (bot.FirstPlayer() == first_player)
                bot.AddElixir(elixir);
    }
}
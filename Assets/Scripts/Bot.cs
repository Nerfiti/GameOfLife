using UnityEngine;

class Bot : MonoBehaviour
{
    [SerializeField] private CardButtonSpawner cards_;

    [SerializeField] private float min_delay_;
    [SerializeField] private float max_delay_;

    public bool pause;
    
    [SerializeField] public bool forbid_change_pause_state;

    void Start()
    {
        ScheduleNextStep();
    }

    void ScheduleNextStep()
    {
        float delay = UnityEngine.Random.value * (max_delay_ - min_delay_) + min_delay_;
        Invoke("MakeStep", delay);
    }

    void MakeStep()
    {
        if (pause)
            return;

        cards_.ActivateRandomCard();

        ScheduleNextStep();
    }

    public void Pause(bool pause)
    {
        if (forbid_change_pause_state)
            return;

        this.pause = pause;

        if (!pause)
            ScheduleNextStep();
    }

    public void TurnOnCards(bool on)
    {
        cards_.TurnOnButtons(on);
    }

    public bool FirstPlayer()
    {
        return cards_.FirstPlayer();
    }

    public void AddElixir(float elixir)
    {
        cards_.AddElixir(elixir);
    }
}
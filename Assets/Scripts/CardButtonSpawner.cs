using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CardButtonSpawner : MonoBehaviour
{
    [SerializeField] private Deck deck_;

    [SerializeField] private GameObject[] hand_containers_;
    [SerializeField] private GameObject stock_container_;

    [SerializeField] private GameObject slider_container_;
    [SerializeField] private Slider elixir_;

    [SerializeField] private Text elixir_text_;

    [SerializeField] private float select_scale_factor_;

    private Button[] hand_;

    private Queue<Button> cards_;

    private int selected_id_ = -1;
    private bool visible_;

    [SerializeField] public bool first_player_;

    private GameMaster master_;

    void Start()
    {
        Initialize();
        master_ = GameObject.FindAnyObjectByType<GameMaster>();
    }

    void Initialize()
    {
        int[] order_array = Enumerable.Range(0, Deck.hand_size).ToArray();
        order_array = order_array.OrderBy(item => UnityEngine.Random.value).ToArray();

        cards_ = new Queue<Button>();
        foreach (var i in order_array)
        {
            deck_.hand[i].transform.localPosition = new Vector3(0, 0, 0);
            deck_.hand[i].transform.localScale = new Vector3(1, 1, 1);
            cards_.Enqueue(deck_.hand[i]);
        }

        SpawnCards();
    }

    void SpawnCards()
    {
        var size = hand_containers_.Length;

        hand_ = new Button[size];
        for (int i = 0; i < size; ++i)
        {
            var index = i;

            SetNewCard(i);
            hand_[index].onClick.AddListener(() => SelectPattern(index));
        }

        elixir_.transform.SetParent(slider_container_.transform, false);
        elixir_.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void UpdateHand()
    {
        hand_[selected_id_].transform.localScale /= select_scale_factor_;
        elixir_.value = Math.Max(elixir_.value - hand_[selected_id_].GetComponent<PatternCard>().cost, elixir_.minValue);
        elixir_text_.text = ((int)elixir_.value).ToString();

        SetInStock(selected_id_); 
        SetNewCard(selected_id_);

        selected_id_ = -1;
    }

    void SetInStock(int index)
    {
        hand_[index].transform.SetParent(stock_container_.transform, false);
        hand_[index].gameObject.SetActive(false);
        hand_[index].onClick.RemoveAllListeners();
        cards_.Enqueue(hand_[index]);
    }

    void SetNewCard(int index)
    {
        hand_[index] = cards_.Dequeue();
        hand_[index].transform.SetParent(hand_containers_[index].transform, false);
        hand_[index].onClick.AddListener(() => SelectPattern(index));
        
        if (visible_)
            hand_[index].gameObject.SetActive(true);
    }

    void SelectPattern(int index)
    {
        var pattern = hand_[index].GetComponent<PatternCard>().pattern;
        master_.SelectPattern(pattern, first_player_);
        master_.cards = this;

        if (selected_id_ != -1)
            hand_[selected_id_].transform.localScale /= select_scale_factor_;

        hand_[index].transform.localScale *= select_scale_factor_;
        selected_id_ = index;
    }

    public void TurnOnButtons(bool on)
    {
        visible_ = on;
        foreach (var card in hand_)
            card.gameObject.SetActive(on);

        elixir_.gameObject.SetActive(on);
    }

    public void ActivateCard(int index)
    {
        var old_selected = selected_id_;
        var old_move = master_.first_player_move;
        var old_deck = master_.cards;
        master_.first_player_move = first_player_;
        master_.cards = this;

        SelectPattern(index);
        master_.SpawnPatternInRandomPosition(first_player_);

        master_.cards = old_deck;
        master_.first_player_move = old_move;
        
        if (selected_id_ != -1)
            hand_[selected_id_].transform.localScale /= select_scale_factor_;
        selected_id_ = old_selected;
    }

    public void ActivateRandomCard()
    {
        int index = UnityEngine.Random.Range(0, hand_containers_.Length);
        ActivateCard(index);
    }

    public bool FirstPlayer()
    {
        return first_player_;
    }

    public int Elixir()
    {
        return (int)elixir_.value;
    }

    public void AddElixir(float elixir)
    {
        elixir = Math.Min(elixir, elixir_.maxValue - elixir_.value);
        elixir_.value += elixir;
        elixir_text_.text = ((int)elixir_.value).ToString();
    }
}

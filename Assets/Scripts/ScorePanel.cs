using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private GameMaster game_master_;
    
    [SerializeField] private Text first_score_;
    [SerializeField] private Slider first_slider_;

    [SerializeField] private Text second_score_;
    [SerializeField] private Slider second_slider_;

    void FixedUpdate()
    {
        float first_score = Math.Min(game_master_.first_score_, game_master_.max_score_);
        float second_score = Math.Min(game_master_.second_score_, game_master_.max_score_);

        first_score_.text  =  "First player score:\n" +  first_score.ToString("F1") + " / " + game_master_.max_score_;
        second_score_.text = "Second player score:\n" + second_score.ToString("F1") + " / " + game_master_.max_score_;

        first_slider_.value = game_master_.first_score_ / game_master_.max_score_;
        second_slider_.value = game_master_.second_score_ / game_master_.max_score_;
    }
}

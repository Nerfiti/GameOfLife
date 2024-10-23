using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public const int hand_size = 8;

    public int deck_size {get; private set;}

    public Button[] hand {get; private set;}
    public List<Button> not_in_hand_cards {get; private set;}

    [SerializeField] private Button card_prefab_;

    [SerializeField] private Color color_;

    void Awake()
    {
        hand = new Button[hand_size];
        not_in_hand_cards = new List<Button>();

        List<Pattern> patterns = new List<Pattern>();
        BinaryFormatter formatter = new BinaryFormatter();

        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.pattern");
        foreach (var file in files)
        {
            FileStream stream = new FileStream(file, FileMode.Open);
            patterns.Add(formatter.Deserialize(stream) as Pattern);
            stream.Close();
        }

        TextAsset[] patterns_text = Resources.LoadAll<TextAsset>("Patterns");
        foreach (var pattern_text in patterns_text)
        {
            byte[] bytes = pattern_text.bytes;

            MemoryStream stream = new MemoryStream(bytes);
            patterns.Add(formatter.Deserialize(stream) as Pattern);
            stream.Close();
        }

        for (int i = 0; i < hand_size; ++i)
        {
            hand[i] = Instantiate(card_prefab_, transform);
            hand[i].gameObject.SetActive(false);

            patterns[i].Center();
            hand[i].gameObject.GetComponent<PatternCard>().Initialize(patterns[i], color_);
        }

        for (int i = hand_size; i < patterns.Count; ++i)
        {
            var card = Instantiate(card_prefab_, transform);
            card.gameObject.SetActive(false);

            patterns[i].Center();
            card.gameObject.GetComponent<PatternCard>().Initialize(patterns[i], color_);
            not_in_hand_cards.Add(card);
        }
    }
};
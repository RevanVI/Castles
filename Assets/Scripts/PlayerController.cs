using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cardPlaces;

    [SerializeField]
    private List<Card> _cards;

    [SerializeField]
    private bool _possibleMageUse = false;

    private void Awake()
    {
        _cards = new List<Card>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool IsCardMatch(Card card)
    {
        if ((_cards.Count == 0 && card.Value != 1) ||
            (_cards.Count > 0 && _cards[_cards.Count - 1].Value > card.Value && !_possibleMageUse))
            return false;
        else
            return true;
    }


    public void InsertCard(Card card)
    {
        if (IsCardMatch(card))
        {
            int index = -1;
            for (int i = _cards.Count - 1; i >= 0; --i)
                if (_cards[i].Value < card.Value)
                {
                    index = i;
                    break;
                }
            index += 1;

            Debug.Log($"InsertCard index {index}");

            _cards.Insert(index, card);
            _cards[index].SetStatus(Card.EStatus.Player, _cardPlaces[index]);
            _cards[index].MoveToBoardPlace();

            if (index != _cards.Count && index != 0)
                for (int i = index + 1; i < _cards.Count; ++i)
                {
                    _cards[i].SetStatus(Card.EStatus.Player, _cardPlaces[i]);
                    _cards[index].MoveToBoardPlace();
                }
        }
        else
            Debug.Log("Card not match");
    }
}

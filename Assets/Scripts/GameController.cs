using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private int _maxValue;
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private List<Card> _cards;

    [SerializeField]
    private List<GameObject> _playerCards;
    [SerializeField]
    private GameObject _board;
    [SerializeField]
    private GameObject _cardHandler;
    [SerializeField]
    private GameObject _bin;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        if (_board.transform.childCount != _maxValue - 1)
        {
            throw new System.Exception("Count of board places and cards are not the same");
        }

        //instantiate board cards
        for (int i = 0; i < _maxValue - 1; ++i)
        {
            GameObject gameObject = Instantiate(_cardPrefab, _cardHandler.transform);
            Card newCard = gameObject.GetComponent<Card>();
            newCard.SetDefaultBoardPlace(_board.transform.GetChild(i).gameObject);
            _cards.Add(newCard);
        }
        //instantiate player cards
        for (int i = 0; i < 4; ++i)
        {
            GameObject gameObject = Instantiate(_cardPrefab, _cardHandler.transform);
            Card newCard = gameObject.GetComponent<Card>();
            newCard.SetDefaultBoardPlace(_playerCards[i]);
            _cards.Add(newCard);
        }
    
        //Setup cards for players
        for (int i = 1; i <= 4; ++i)
        {
            _cards[i - 1].SetData(-i);
        }

        //build values array
        List<int> values = new List<int>();
        for (int i = 2; i <= _maxValue; ++i)
        {
            values.Add(i);
        }

        //setup cards
        for(int i = 0; i < _cards.Count - 4; ++i)
        {
            int ind = Random.Range(0, values.Count);
            _cards[i].SetData(values[ind]);
            values.RemoveAt(ind);
            _cards[i].MoveToDefaultBoardPlace();
        }
        //setup player cards
        for (int i = _cards.Count - 4; i < _cards.Count; ++i)
        {
            _cards[i].MoveToDefaultBoardPlace();
        }

    }
}


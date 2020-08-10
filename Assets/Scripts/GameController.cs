using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private int _maxValue;
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private List<Card> _cards;

    [SerializeField]
    private List<PlayerController> _players;
    [SerializeField]
    private List<GameObject> _playerCardPlaces;
    [SerializeField]
    private GameObject _board;
    [SerializeField]
    private GameObject _cardHandler;
    [SerializeField]
    private GameObject _bin;

    private Card _activeCard = null;
    private int _playerNoTurn = 0;

    [SerializeField]
    private float _secsToTake = 1f;

    public UnityEvent OnTurnEnd;

    // Start is called before the first frame update
    void Start()
    {
        OnTurnEnd = new UnityEvent();
        Initialize();

        InputController.Instance.OnClick.AddListener(ProcessClick);
        InputController.Instance.OnHold.AddListener(ProccesHold);
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
            newCard.SetDefaultBoardPlace(_playerCardPlaces[i]);
            _cards.Add(newCard);
        }
    
        //Setup cards for players
        for (int i = 1; i <= 4; ++i)
        {
            _cards[_cards.Count - 5 + i].SetData(-i);
            _players[i - 1].InsertCard(_cards[_cards.Count - 5 + i]);
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

    public void ProcessClick(InputController.MouseData data)
    {
        Ray ray = Camera.main.ScreenPointToRay(data.StartPosition);
        LayerMask layerMask;
        if (_activeCard == null)
        {
            layerMask = LayerMask.GetMask("Cards");
        }
        else
            layerMask = LayerMask.GetMask("ActiveCard");
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, Camera.main.farClipPlane, layerMask);
        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * Camera.main.farClipPlane, Color.green, 10f);
        Debug.Log($"ProcessClick ActiveCard {_activeCard != null} Hit {hitInfo.collider != null}");
        if (hitInfo.collider != null)
        {
            Card card = hitInfo.collider.gameObject.GetComponent<Card>();
            Debug.Log($"On card click {card.gameObject.name}");
            card.Flip();
            if (_activeCard != null)
            {
                _activeCard = null;
                EndTurn();
            }
            else
                _activeCard = card;
        }
    }

    public void ProccesHold(InputController.MouseData data)
    {
        if (_activeCard == null)
            return;
        //check availability
        Ray ray = Camera.main.ScreenPointToRay(data.StartPosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, Camera.main.farClipPlane, LayerMask.GetMask("ActiveCard"));
        //Debug.Log($"ProcessHold Hit {hitInfo.collider != null}");
        if (hitInfo.collider != null)
        {
            Card card = hitInfo.collider.gameObject.GetComponent<Card>();
            card.SetProgressValue(Mathf.Clamp01(data.Time / _secsToTake));
            //Debug.Log($"On card click {card.gameObject.name}");
            if (data.Time > _secsToTake)
            {
                Debug.Log($"Hold ended {card.gameObject.name}");
                _players[_playerNoTurn].InsertCard(card);
                //card.Flip();
                _activeCard = null;
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {

    }
}


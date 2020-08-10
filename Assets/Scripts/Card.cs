using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum EType
    {
        Common = 0,
        Tower = 1,
        Crow = 2,
        Mage = 3,
        Princess = 4,
        Prince = 5,
        King = 6,
        Queen = 7,
        Gun = 8,
        Player = 9,
    }

    public enum EStatus
    {
        Free = 1,
        Player = 2,
        Removed = 3,
    }

    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SOCardProperties _cardProperties;

    [SerializeField]
    private Sprite _image;
    [SerializeField]
    private Sprite _backImage;
    [SerializeField]
    private Text _valueText;
    [SerializeField]
    private GameObject _boardPlace;
    [SerializeField]
    private GameObject _defaultBoardPlace;
    private bool _isFaceDown = true;

    [SerializeField]
    private ProgressBar _progressBar;

    [SerializeField]
    private int _value;
    public int Value
    {
        get
        {
            return _value;
        }
    }

    private EType _type;
    public EType Type
    {
        get
        {
            return _type;
        }
    }

    private EStatus _status;
    public EStatus Status
    {
        get
        {
            return _status;
        }
    }

    public void SetData(int value)
    {
        _backImage = _cardProperties.BackImage;
        _isFaceDown = true;
        _spriteRenderer.sprite = _backImage;
        int index = _cardProperties.SpecialCards.IndexOf(new SOCardProperties.SpecialCardData(value));
        if (index != -1)
        {
            _image = _cardProperties.SpecialCards[index].Image;
            _type = _cardProperties.SpecialCards[index].Type;
        }
        else
        {
            _image = _cardProperties.CommonImage;
            _type = EType.Common;
        }
        if (value < 0 && value > -5)
            _value = 1;
        else
            _value = value;
        _valueText.text = _value.ToString();
    }

    public void SetStatus(EStatus status, GameObject place)
    {
        _status = status;
        if (status == EStatus.Free)
        {
            _boardPlace = _defaultBoardPlace;
        }
        else
            _boardPlace = place;
    }

    public void SetDefaultBoardPlace(GameObject boardPlace)
    {
        _defaultBoardPlace = boardPlace;
    }

    public void MoveToDefaultBoardPlace()
    {
        transform.position = _defaultBoardPlace.transform.position;
    }

    public void MoveToBoardPlace()
    {
        transform.position = _boardPlace.transform.position;
    }

    public void Flip()
    {
        if (_isFaceDown)
        {
            _spriteRenderer.sprite = _image;
            gameObject.layer = 9;
        }
        else
        {
            _spriteRenderer.sprite = _backImage;
            gameObject.layer = 8;
        }
        _isFaceDown = !_isFaceDown;
    }

    public void SetProgressValue(float val)
    {
        _progressBar.SetValue(val);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isFaceDown = true;
        _backImage = _cardProperties.BackImage;
        _spriteRenderer.sprite = _backImage;
        _progressBar.SetMode(false);
        _progressBar.SetMaxValue(1f);
        _progressBar.SetValue(0f);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

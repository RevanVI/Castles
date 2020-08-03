using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private SOCardProperties _cardProperties;

    [SerializeField]
    private Sprite _image;
    [SerializeField]
    private Sprite _backImage;

    [SerializeField]
    private GameObject _boardPlace;

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
    }

    public void SetDefaultBoardPlace(GameObject boardPlace)
    {
        _boardPlace = boardPlace;
    }

    public void MoveToDefaultBoardPlace()
    {
        transform.position = _boardPlace.transform.position;
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

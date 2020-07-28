using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardProperties", menuName = "CardProperties", order = 1)]
public class SOCardProperties : ScriptableObject
{
    public Sprite CommonImage;
    public Sprite BackImage;

    [SerializeField]
    public List<SpecialCardData> SpecialCards;

    [System.Serializable]
    public class SpecialCardData: System.IComparable
    {
        public Sprite Image;
        public int Value;
        public Card.EType Type;

        public SpecialCardData(int value)
        {
            Value = value;
        }

        public int CompareTo(object o)
        {
            SpecialCardData other = o as SpecialCardData;
            if (other == null)
                throw new System.Exception("object is not SpecialCardData class");
            return Value.CompareTo(other.Value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GestureEvent: UnityEvent<InputController.GestureData>
{ }

public class MouseEvent : UnityEvent<InputController.MouseData>
{ }


public class KeyEvent: UnityEvent<KeyCode>
{ }

public class InputController : MonoBehaviour
{
    private static InputController _instance;
    public static InputController Instance
    {
        get
        {
            return _instance;
        }
    }

    public class GestureData
    {
        public int fingerId;
        public GestureType Type;
        public Vector2 startPosition;
        public Vector2 endPosition;
        public Vector2 deltaPosition;
        public float time;
    }

    public class MouseData
    {
        public int ButtonId;
        public float Time;
        public Vector2 StartPosition;
        public Vector2 EndPosition;
    }

    public enum GestureType
    {
        None = 0,
        Tap = 1,
        Swipe = 2,
        Drag = 3,
    }

    [SerializeField]
    private bool _isMoving;
    private float minDistance = 50f;
    private float maxTime = 0.6f;

    private List<GestureData> _gestures;
    private List<MouseData> _mouseEvents;
    public int count = 0;

    public GestureEvent OnTap;
    public GestureEvent OnDrag;
    public GestureEvent OnSwipe;
    public MouseEvent OnClick;
    public MouseEvent OnHold;
    public MouseEvent OnRelease;

    public KeyEvent OnEsc;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        OnTap = new GestureEvent();
        OnDrag = new GestureEvent();
        OnSwipe = new GestureEvent();
        OnEsc = new KeyEvent();

        OnClick = new MouseEvent();
        OnHold = new MouseEvent();
        OnRelease = new MouseEvent();
}

    void Start()
    {
        _gestures = new List<GestureData>();
        _mouseEvents = new List<MouseData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEsc?.Invoke(KeyCode.Escape);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                MouseData data = new MouseData();
                data.ButtonId = 0;
                data.Time = 0;
                data.StartPosition = Input.mousePosition;
                _mouseEvents.Add(data);
            }
        }
        else if (Input.GetMouseButton(0) && _mouseEvents.Count != 0)
        {
            _mouseEvents[0].Time += Time.deltaTime;
            if (_mouseEvents[0].Time > 0.25f)
                OnHold.Invoke(_mouseEvents[0]);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseData data = _mouseEvents[0];
            _mouseEvents.RemoveAt(0);
            data.EndPosition = Input.mousePosition;
            if (data.Time < 0.25f)
                OnClick.Invoke(data);
            else
                OnRelease.Invoke(data);
        }
    }

    public int FindTouchIndexById(int fingerId)
    {
        for (int i = 0; i < _gestures.Count; ++i)
        {
            if (_gestures[i].fingerId == fingerId)
                return i;
        }
        return -1;
    }
}

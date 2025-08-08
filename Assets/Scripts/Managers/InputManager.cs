using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PointerEventData mousePosition;
    // public bool useCursor = true;
    // private RectTransform cursor;

    [HideInInspector] public UnityEvent onPress = new UnityEvent();
    [HideInInspector] public UnityEvent onRelease = new UnityEvent();

    #region Singleton
    public static InputManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    #region InputDelta
    //if we need to input as X, Y continuously, uncomment this block
    public void OnPointerDown(PointerEventData _eventData)
    {
        mousePosition = _eventData;
        startedPos = mousePosition.position;
        onPress.Invoke();
    }
    public void OnPointerUp(PointerEventData _eventData)
    {
        onRelease.Invoke();
        mousePosition = null;
        delta = Vector2.zero;
        startedPos = Vector2.zero;
        input = Vector2.zero;
    }

    private Vector2 startedPos;
    private Vector2 delta;
    [HideInInspector] public Vector2 input;
    public float maxDistance = 100f; // Change this to according the desired precision 

    private void Update()
    {
        if (mousePosition == null) return;
        delta = mousePosition.position - startedPos;
        delta.x = Mathf.Clamp(delta.x, -maxDistance, maxDistance);
        delta.y = Mathf.Clamp(delta.y, -maxDistance, maxDistance);
        input = delta / maxDistance;
        startedPos = mousePosition.position;
    }

    #endregion
}


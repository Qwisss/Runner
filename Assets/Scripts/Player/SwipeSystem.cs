using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeSystem : MonoBehaviour
{
/*
    public static SwipeSystem instance;
    public enum Direction { Left, Right, Up, Down }
    public delegate void ClickDelegate(Vector2 pos);
    public delegate void MoveDelegate(bool[] swipes);
    public ClickDelegate ClickEvent;
    public MoveDelegate MoveEvent;

    bool[] swipe = new bool[4];
    bool touchMoved;

    const float SWIPE_THRESHOLD = 50;

    Vector2 startTouch;
    Vector2 swipeDelta;
    Vector2 TouchPosition()
    {
        return (Vector2)(Input.mousePosition);
    }

    bool TouchBegan()
    {
        return Input.GetMouseButtonDown(0);
    }
    bool TouchEnded()
    {
        return Input.GetMouseButtonUp(0);
    }
    bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }
        // START FINISH
        if (TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        }
        else if (TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }
        // CALC DISTANCE
        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }
        // CHECK SWIPE
        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                //LEFT/RIGHT
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }
            else
            {
                //UP/Down
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }

    private void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3])
        {
            //Debug.LogError(swipe[0] + "|" + swipe[1] + "|" + swipe[2] + "|" + swipe[3]);
            MoveEvent?.Invoke(swipe);
        }
        else
        {
            //Debug.LogError("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < 4; i++)
        {
            swipe[i] = false;
        }
    }*/
}



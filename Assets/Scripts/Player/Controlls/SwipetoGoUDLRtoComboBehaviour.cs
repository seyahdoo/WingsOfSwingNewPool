using UnityEngine;
using System.Collections;

public class SwipetoGoUDLRtoComboBehaviour : PlayerController {

    
    void Awake()
    {
        base.Initialize();
        Memory = new TouchManager.Direction[2];
        Memory[0] = TouchManager.Direction.Nowhere; //Last
        Memory[1] = TouchManager.Direction.Nowhere; //Before Last
    }

    public override void Swipe(TouchManager.Direction SwipeDirection)
    {
        GoTo(SwipeDirection);
        RememberNDoThingie(SwipeDirection);
    }

    void RememberNDoThingie(TouchManager.Direction Direction)
    {
        //GetDirections
        TouchManager.Direction Last = Memory[0];
        TouchManager.Direction Before = Memory[1];

        if (Before == TouchManager.Direction.Up)
        {
            if(Last == TouchManager.Direction.Up)
            {
                Combo(Direction, Last, 2);
            }
            else if(Last == TouchManager.Direction.Down)
            {
                Combo(Direction, Last, 2);
            }
            
        }
        else if (Before == TouchManager.Direction.Down)
        {
            if(Last == TouchManager.Direction.Up)
            {
                Combo(Direction, Last, 2);
            }

            else if(Last == TouchManager.Direction.Down)
            {
                Combo(Direction, Last, 2);
            }
            
        }


        //WriteNewDirections
        //Memorize Last Directions
        Memory[1] = Last;
        Memory[0] = Direction;
        
    }



}

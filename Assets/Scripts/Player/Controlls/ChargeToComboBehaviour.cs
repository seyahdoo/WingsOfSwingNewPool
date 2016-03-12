using UnityEngine;
using System.Collections;

public class ChargeToComboBehaviour : PlayerController {

    
    public override void Swipe(TouchManager.Direction SwipeDirection)
    {
        if (Immobilized) return;

        GoTo(SwipeDirection);
        HandleCharge(SwipeDirection);
    }
    

    void HandleCharge(TouchManager.Direction Direction)
    {

        //HandleCharging,Combo
        if (ChargeCount >= 0)
        {
            if (Direction == ChargeDirection)
                ChargeCount++;

            if (Direction != ChargeDirection)
            {
                if (ChargeCount >= 1)
                {
                    //Stop go Timer
                    GoTimer.SetActive(false);

                    //Start Combo(Direction, charge count)
                    Combo(Direction, ChargeDirection, ChargeCount);

                    //reset Handler
                    ChargeCount = 0;
                    ChargeDirection = TouchManager.Direction.Nowhere;
                }
                else
                {
                    ChargeCount = 0;
                    ChargeDirection = Direction;
                }

            }
        }
        else
        {
            //what if charge = -1??? 
            ChargeDirection = Direction;
            ChargeCount = 0;
        }


    }

    

}

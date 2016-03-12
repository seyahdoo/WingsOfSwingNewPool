using UnityEngine;
using System.Collections;

public class ClickToGoSwipeToComboBehaviour : PlayerController {


    public override void Click(TouchManager.Direction ClickArea)
    {

        GoTo(ClickArea);

    }

    public override void Swipe(TouchManager.Direction SwipeDirection)
    {

        Combo(SwipeDirection,GoDirection,2);

    }


    //Kraken Test




}

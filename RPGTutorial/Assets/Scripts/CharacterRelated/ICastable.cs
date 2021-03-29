using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable 
{
    string MyTitle
    {
        get;
    }

    Sprite MyIcon
    {
        get;
    }

    float MyCastTime
    {
        get;
    }

    Color MyBarColor
    {
        get;
    }
}

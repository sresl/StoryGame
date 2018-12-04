using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoolGenerator{

    System.Random rnd;

    public BoolGenerator()
    {
        rnd = new System.Random();
    }

    public bool NextBoolean()
    {
        return Convert.ToBoolean(rnd.Next(0, 2));

    }
}

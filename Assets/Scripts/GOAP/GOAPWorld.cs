using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GOAPWorld
{
    private static readonly GOAPWorld instance = new GOAPWorld();
    private static GOAPWorldStates world;

    static GOAPWorld()
    {
        world = new GOAPWorldStates();
    }

    private GOAPWorld()
    {

    }

    public static GOAPWorld Instance
    {
        get { return instance; }
    }

    public GOAPWorldStates GetWorld()
    {
        return world;
    }
}

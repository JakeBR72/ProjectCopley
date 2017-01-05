using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AStarTile {
    public int F;
    public int X, Z, G, H;
    public AStarTile Parent;

    public AStarTile(int x, int z, int g, int h, AStarTile parent)
    {
        X = x;
        Z = z;
        G = g;
        H = h;
        Parent = parent;
    }
}

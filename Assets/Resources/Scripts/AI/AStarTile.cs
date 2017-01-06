using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AStarTile {
    public int F, X, Z, G, H;
    public Vector3 Pos;
    public AStarTile Parent;

    public AStarTile(Vector3 pos, Vector3 startPos, Vector3 endPos, AStarTile parent)
    {
        Pos = pos;
        X = (int)pos.x;
        Z = (int)pos.z;
        G = Mathf.Abs((int)(startPos.x - pos.x)*10) + Mathf.Abs((int)(startPos.z - pos.z) * 10);
        H = Mathf.Abs((int)(endPos.x - pos.x) * 10) + Mathf.Abs((int)(endPos.z - pos.z) * 10);
        F = G + H;
        Parent = parent;
    }
}

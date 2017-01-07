using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JPSNode{
    public Vector3 pos;
    public JPSNode parent;
    public int forced;
    public int Direction;
    public int F;
    private Vector3 Destination;
    private int G, H;

    public JPSNode(Vector3 newPos, Vector3 destination, JPSNode par, int direction)
    {
        Direction = direction;
        Destination = destination;
        pos = newPos;
        parent = par;
        if (par != null)
        {
            G = par.G + (int)Vector3.Distance(par.pos, pos);
        } else
        {
            G = 0;
        }
        H = (int)Vector3.Distance(Destination, pos);
        F = G + H;
    }
}

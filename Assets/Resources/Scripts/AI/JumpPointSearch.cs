using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointSearch : MonoBehaviour {
    public int MovementSpeed = 5;
    public Vector3 Destination;
    public bool HasPath;
    public List<JPSNode> Open = new List<JPSNode>();
    public List<JPSNode> Closed = new List<JPSNode>();
    public List<JPSNode> Path = new List<JPSNode>();
    JPSNode newNode;
    JPSNode currNode;
    Vector3 currPos;
    Grid Grid;
    int StepInPath;
    private void Start()
    {
        Grid = GameObject.Find("_Initializer").GetComponent<Grid>();
    }
    private void Update()
    {
        if(StepInPath != -1)
        {
            Navigate();
        }
    }
    //INITIALIZE
    public void NewPath()
    {
        foreach(GameObject obj in Grid.FloorLayer)
        {
            if (obj.transform.position.x >= 1 && obj.transform.position.x < Grid.MapSize - 1 &&
                obj.transform.position.z >= 1 && obj.transform.position.z < Grid.MapSize - 1)
            {
                obj.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1));
            }
        }
        transform.position = new Vector3(4, 1, 4);
        StepInPath = -1;
        HasPath = false;
        Open = new List<JPSNode>();
        Closed = new List<JPSNode>();
        Path = new List<JPSNode>();
        AddToOpen(transform.position, null, -10);
        currNode = null;
        currPos = new Vector3();
        newNode = null;
        //Do first vert/horiz checks
        FindPath();
        //Path found or not, process colors/path segments
        foreach (JPSNode node in Closed)
        {
            Debug.Log(node.Direction + " " + node.forced + " " + node.pos);
            ColorFloor(node.pos, Color.cyan);
        }
        foreach (JPSNode node in Open)
        {
            ColorFloor(node.pos, Color.red);
        }
        JPSNode Start = null;
        foreach(JPSNode node in Closed)
        {
            if(node.pos == Destination)
            {
                Start = node;
            }
        }
        if (Start != null)
        {
            while (Start.parent != null)
            {
                ColorFloor(Start.pos,Color.green);
                Path.Add(Start);
                Start = Start.parent;
            }
            Path.Add(Start);
            Path.Reverse();
        }else
        {
            Debug.Log("NO PATH");
        }
        foreach(JPSNode node in Path)
        {
            Debug.Log(node.pos);
        }

        StepInPath = 0;
        Navigate();

    }
    ///FIND PATH
    void FindPath()
    {
        while (Open.Count > 0 && !HasPath)
        {
            //Get lowest F node
            currNode = Open[0];
            foreach (JPSNode node in Open)
            {
                if (node.F < currNode.F)
                {
                    currNode = node;
                }
            }
            Open.Remove(currNode);
            Closed.Add(currNode);
            if (currNode.Direction == -10)
            {
                Horiz(currNode,1);
                Horiz(currNode, -1);
                Vert(currNode, 1);
                Vert(currNode, -1);
                Diag(new Vector3(1, 0, 1));
                Diag(new Vector3(1, 0, -1));
                Diag(new Vector3(-1, 0, -1));
                Diag(new Vector3(-1, 0, 1));
            }
            //Do stuff with other directions/forced
            Direct();
        }
    }

    void Navigate()
    {
        if(StepInPath < Path.Count)
        {
            if(transform.position != Path[StepInPath].pos)
            {
                transform.position = Vector3.MoveTowards(transform.position, Path[StepInPath].pos, MovementSpeed*Time.deltaTime);
            }else
            {
                StepInPath++;
            }
        }
    }

    void Direct()
    {
        int Direction = currNode.Direction;
        int Forced = currNode.forced;
        if (Direction == 2)
        {
            if (Forced == 1)
            {
                Vert(currNode, 1);
                Diag(new Vector3(-1, 0, 1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, 1);
                Diag(new Vector3(1, 0, 1));
            }
            else if (Forced == 3)
            {
                Vert(currNode, 1);
                Diag(new Vector3(1, 0, 1));
                Diag(new Vector3(-1, 0, 1));
            }else
            {
                Vert(currNode, 1);
            }
        }
        else if (Direction == 3)
        {

            if (Forced == 1)
            {
                Horiz(currNode, 1);
                Vert(currNode, 1);
                Diag(new Vector3(-1, 0, 1));
                Diag(new Vector3(1, 0, 1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, 1);
                Horiz(currNode, 1);
                Diag(new Vector3(1, 0, 1));
                Diag(new Vector3(1, 0, -1));
            }
            else
            {
                Diag(new Vector3(1, 0, 1));
            }
        }
        else if (Direction == 1)
        {            
            if (Forced == 1)
            {
                Horiz(currNode, 1);
                Diag(new Vector3(1, 0, 1));
            }
            else if (Forced == 2)
            {
                Horiz(currNode, 1);
                Diag(new Vector3(1, 0, -1));
            }
            else if (Forced == 3)
            {
                Horiz(currNode, 1);
                Diag(new Vector3(1, 0, 1));
                Diag(new Vector3(1, 0, -1));
            }else
            {
                Horiz(currNode, 1);
            }
        }
        else if (Direction == -3)
        {
            if(Forced == 1)
            {
                Horiz(currNode, 1);
                Vert(currNode, -1);
                Diag(new Vector3(-1, 0, -1));
                Diag(new Vector3(1, 0, -1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, -1);
                Horiz(currNode, -1);
                Diag(new Vector3(1, 0, -1));
                Diag(new Vector3(1, 0, 1));
            }
            else
            {
                Diag(new Vector3(1, 0, -1));
            }
        }
        else if (Direction == -2)
        {
            
            if (Forced == 1)
            {
                Vert(currNode, -1);
                Diag(new Vector3(-1, 0, -1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, -1);
                Diag(new Vector3(1, 0,-1));
            }
            else if (Forced == 3)
            {
                Vert(currNode, -1);
                Diag(new Vector3(1, 0, -1));
                Diag(new Vector3(1, 0, -1));
            }else
            {
                Vert(currNode, -1);
            }
        }
        else if (Direction == -4)
        {
            if (Forced == 1)
            {
                Horiz(currNode, -1);
                Vert(currNode, -1);
                Diag(new Vector3(-1, 0, -1));
                Diag(new Vector3(1, 0, -1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, -1);
                Diag(new Vector3(-1, 0, -1));
                Diag(new Vector3(-1, 0, 1));
            }
            else
            {
                Diag(new Vector3(-1, 0, -1));
            }
        }
        else if (Direction == -1)
        {            
            if (Forced == 1)
            {
                Horiz(currNode, -1);
                Diag(new Vector3(-1, 0, 1));
            }
            else if (Forced == 2)
            {
                Horiz(currNode, -1);
                Diag(new Vector3(-1, 0, -1));
            }
            else if (Forced == 3)
            {
                Horiz(currNode, -1);
                Diag(new Vector3(-1, 0, 1));
                Diag(new Vector3(-1, 0, -1));
            }else
            {
                Horiz(currNode, -1);
            }
        }
        else if (Direction == 4)
        {
            if (Forced == 1)
            {
                Horiz(currNode, -1);
                Vert(currNode, 1);
                Diag(new Vector3(-1, 0, 1));
                Diag(new Vector3(1, 0, 1));
            }
            else if (Forced == 2)
            {
                Vert(currNode, 1);
                Diag(new Vector3(1, 0, 1));
                Diag(new Vector3(-1, 0, 1));
            }
            else
            {
                Diag(new Vector3(-1, 0, 1));
            }
        }
    }
    void Diag(Vector3 dir)
    {
        //Prime in diagonal slot
        Vector3 DiagPos = currNode.pos + dir;
        while (DiagPos.x >= 1 && DiagPos.x < Grid.MapSize - 1 &&
                DiagPos.z >= 1 && DiagPos.z < Grid.MapSize - 1)
        {
            if (BlockAt(DiagPos) || (BlockAt(DiagPos + new Vector3(-dir.x, 0, 0)) && BlockAt(DiagPos + new Vector3(0, 0, -dir.z))))
            {
                return;
            }
            if (DiagPos == Destination)
            {
                AddNodeToClosed(DiagPos, currNode, 0);
            }
            JPSNode Diag = new JPSNode(DiagPos, Destination, currNode, 3);
            if(BlockAt(DiagPos + new Vector3(-dir.x, 0, 0)))
            {
                Diag.forced = 1;
            }
            if (BlockAt(DiagPos + new Vector3(0, 0, -dir.z)))
            {
                Diag.forced = 2;
            }
            //ColorFloor(DiagPos, Color.green);
            if (dir.x > 0 && dir.z > 0)
            {
                if (Horiz(Diag, 1) || Vert(Diag, 1) || (BlockAt(DiagPos + new Vector3(-dir.x, 0,0)) || BlockAt(DiagPos + new Vector3(0, 0, -dir.z))))
                {
                    Diag.Direction = 3; //up Right
                    Open.Add(Diag);
                    return;
                }
            }
            else if (dir.x > 0 && dir.z < 0)
            {
                if (Horiz(Diag, 1) || Vert(Diag, -1) || (BlockAt(DiagPos + new Vector3(-dir.x, 0, 0)) || BlockAt(DiagPos + new Vector3(0, 0, -dir.z))))
                {
                    Diag.Direction = -3;// down right
                    Open.Add(Diag);
                    return;
                }
            }
            else if (dir.x < 0 && dir.z < 0)
            {
                if (Horiz(Diag, -1) || Vert(Diag, -1) || (BlockAt(DiagPos + new Vector3(-dir.x, 0, 0)) || BlockAt(DiagPos + new Vector3(0, 0, -dir.z))))
                {
                    Diag.Direction = -4; // down left
                    Open.Add(Diag);
                    return;
                }
            }
            else if (dir.x < 0 && dir.z > 0)
            {
                if (Horiz(Diag, -1) || Vert(Diag, 1) || (BlockAt(DiagPos + new Vector3(-dir.x, 0, 0)) || BlockAt(DiagPos + new Vector3(0, 0, -dir.z))))
                {
                    Diag.Direction = 4; // up left
                    Open.Add(Diag);
                    return;
                }
            }
            DiagPos += dir;
        }
    }

    bool Vert(JPSNode parent, int dir)
    {
        bool FoundNode = false;
        currPos = parent.pos + new Vector3(0, 0, dir);
        JPSNode temp = new JPSNode(new Vector3(-1, -1, -1), new Vector3(-1, -1, -1), null, 0);
        while (currPos.z < Grid.MapSize - 1 && currPos.z >= 1)
        {
            if (BlockAt(currPos))
            {
                return false;
            }
            //ColorFloor(currPos, Color.yellow);//color
            if (currPos == Destination)
            {
                AddNodeToClosed(currPos, parent, dir*2);
                HasPath = true;
                return true;
            }
            if (!BlockAt(currPos + new Vector3(0, 0, dir)))
            {
                if (BlockAt(currPos + new Vector3(-1, 0, 0)) && !BlockAt(currPos + new Vector3(-1, 0, dir)))
                {
                    temp = AddToOpen(currPos, parent, dir*2);
                    if (temp != null)
                    {
                        ColorFloor(currPos, Color.red);
                        FoundNode = true;
                        temp.forced += 1;
                    }
                }
                if (BlockAt(currPos + new Vector3(1, 0, 0)) && !BlockAt(currPos + new Vector3(1, 0, dir)))
                {
                    if((temp != null && temp.parent != null))
                    {
                        temp.forced += 2;
                    }else if (temp != null && temp.parent == null)
                    {
                        temp = AddToOpen(currPos, parent, dir * 2);
                        if (temp != null)
                        {
                            ColorFloor(currPos, Color.red);
                            FoundNode = true;
                            temp.forced += 2;
                        }
                    }
                }
                currPos.z += dir;
            }
            else
            {
                return FoundNode;
            }
        }
        return FoundNode;
    }
    bool Horiz(JPSNode parent, int dir)
    {
        bool FoundNode = false;
        currPos = parent.pos + new Vector3(dir, 0,0);
        JPSNode temp = new JPSNode(new Vector3 (-1,-1,-1), new Vector3(-1, -1, -1), null, 0);
        while(currPos.x < Grid.MapSize - 1 && currPos.x >= 1)
        {
            if (BlockAt(currPos))
            {
                return false;
            }
            //ColorFloor(currPos, Color.yellow);//color
            if (currPos == Destination)
            {
                AddNodeToClosed(currPos, parent, dir);
                HasPath = true;
                return true;
            }
            if (!BlockAt(currPos + new Vector3(dir, 0, 0)))
            {
                if (BlockAt(currPos + new Vector3(0, 0, 1)) && !BlockAt(currPos + new Vector3(dir, 0, 1)))
                {
                    temp = AddToOpen(currPos, parent, dir);
                    if (temp != null)
                    {

                        ColorFloor(currPos, Color.red);
                        temp.forced += 1;
                        FoundNode = true;
                    }
                }
                if (BlockAt(currPos + new Vector3(0, 0, -1)) && !BlockAt(currPos + new Vector3(dir, 0, -1)))
                {
                    if ((temp != null && temp.parent != null))
                    {
                        temp.forced += 2;
                    }
                    else if (temp != null && temp.parent == null)
                    {
                        temp = AddToOpen(currPos, parent, dir);
                        if (temp != null)
                        {
                            ColorFloor(currPos, Color.red);
                            FoundNode = true;
                            temp.forced += 2;
                        }
                    }
                }
                if (FoundNode)
                {
                    return FoundNode;
                }
                currPos.x += dir;
            }else
            {
                return FoundNode;
            }
        }
        return FoundNode;
    }
    void ColorFloor(Vector3 pos, Color color)
    {
        Grid.FloorLayer[(int)pos.x, (int)pos.z].GetComponentInChildren<Renderer>().material.SetColor("_Color", color);
    }
    bool BlockAt(Vector3 pos)
    {
        if(pos.x > Grid.MapSize-2 || pos.x < 1 || pos.z > Grid.MapSize - 1 || pos.z < 1)
        {
            return true;
        }
        return (Grid.ObjectLayer[(int)pos.x, (int)pos.z] != null);
    }

    ///ADD TO LISTS
    JPSNode AddToOpen(Vector3 pos, JPSNode par, int direction)
    {
        newNode = new JPSNode(pos, Destination, par, direction);
        foreach (JPSNode node in Open)
        {
            if (node.pos == pos)
            {
                return null;
            }
        }
        foreach (JPSNode node in Closed)
        {
            if (node.pos == pos)
            {
                return null;
            }
        }
        Open.Add(newNode);
        return newNode;
    }
    JPSNode AddNodeToClosed(Vector3 pos, JPSNode par, int direction)
    {
        newNode = new JPSNode(pos, Destination, par, direction);
        foreach (JPSNode node in Open)
        {
            if (node.pos == pos)
            {
                return null;
            }
        }
        foreach (JPSNode node in Closed)
        {
            if (node.pos == pos)
            {
                return null;
            }
        }
        Closed.Add(newNode);
        return newNode;
    }
}

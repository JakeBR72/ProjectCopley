using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {
    public Vector3 StartPos;
    public Vector3 EndPos;
    private Vector3 OldEndPos;
    private Vector3 OldStartPos;
    AStarTile CurrentTile;

    public bool HasPath = false;
    public int StepInPath;
    public bool PathAvailable = true;
    public bool Done = false;

    public List<AStarTile> Open = new List<AStarTile>();
    public List<AStarTile> Closed = new List<AStarTile>();
    public List<AStarTile> Path = new List<AStarTile>();

    private Grid Grid;

    private void Start()
    {
        Grid = GameObject.Find("_Initializer").GetComponent<Grid>();
        Grid.PathFinders.Add(this);
        StartPos = new Vector3((int)transform.position.x, 1, (int)transform.position.z);
        CurrentTile = new AStarTile(StartPos, StartPos, EndPos,null);
        Closed.Add(CurrentTile);
        OldEndPos = EndPos;
        OldStartPos = StartPos;
    }

    private void Update()
    {
        if (!Done)
        {
            if (HasPath)
            {
                Navigate();
            }
            else if (PathAvailable)
            {
                SearchForPath();
            }
        }else
        {            
            if(OldEndPos != EndPos)
            {
                OldEndPos = EndPos;
                ForceRecheck();
            }
        }
    }

    void Navigate()
    {
        if(Mathf.Abs(Vector3.Distance(Path[StepInPath].Pos,transform.position)) > 0.25f )
        {
            MoveToNextNode();
        }
        else
        {
            if(StepInPath < Path.Count-1) {
                //PathFindColor
                Grid.FloorLayer[Path[StepInPath].X,Path[StepInPath].Z].GetComponentInChildren<Renderer>().material.color = new Color(1, 1, 1, 1f);

                StepInPath++;
            }else
            {
                if (transform.position != EndPos)
                {
                    MoveToNextNode();
                }else
                {
                    //PathFindColor
                    Grid.FloorLayer[Path[StepInPath].X, Path[StepInPath].Z].GetComponentInChildren<Renderer>().material.color = new Color(1, 1, 1, 1f);
                    Done = true;
                    EndPos = OldStartPos;
                    StartPos = OldEndPos;
                }
            }
        }
    }
    void MoveToNextNode()
    {
        transform.position = Vector3.MoveTowards(transform.position, Path[StepInPath].Pos, Time.deltaTime);
    }
    void SearchForPath()
    {
        AStarTile LowestFTile = CurrentTile;
        int LowestF = int.MaxValue;
        if(CurrentTile.X - 1 >= 1 && Grid.ObjectLayer[CurrentTile.X - 1,CurrentTile.Z] == null)
        {
            AddToOpen(  new AStarTile(CurrentTile.Pos + new Vector3(-1, 0, 0), StartPos, EndPos, CurrentTile)  );            
        }
        if (CurrentTile.X + 1 < Grid.MapSize-1 && Grid.ObjectLayer[CurrentTile.X + 1, CurrentTile.Z] == null)
        {
            AddToOpen(new AStarTile(CurrentTile.Pos + new Vector3(1, 0, 0), StartPos, EndPos, CurrentTile));
        }
        if (CurrentTile.Z - 1 >= 1 && Grid.ObjectLayer[CurrentTile.X , CurrentTile.Z - 1] == null)
        {
            AddToOpen(new AStarTile(CurrentTile.Pos + new Vector3(0, 0, -1), StartPos, EndPos, CurrentTile));
        }
        if (CurrentTile.Z + 1 < Grid.MapSize-1 && Grid.ObjectLayer[CurrentTile.X, CurrentTile.Z + 1] == null)
        {
            AddToOpen(new AStarTile(CurrentTile.Pos + new Vector3(0, 0, 1), StartPos, EndPos, CurrentTile));
        }
        foreach(AStarTile tile in Open)
        {
            if(tile.F < LowestF)
            {
                LowestF = tile.F;
                LowestFTile = tile;
            }
        }
        if(CurrentTile == LowestFTile)
        {
            PathAvailable = false;
        }
        CurrentTile = LowestFTile;
        Open.Remove(CurrentTile);
        Closed.Add(CurrentTile);
        if(CurrentTile.Pos == EndPos)
        {
            HasPath = true;
            while(CurrentTile.Parent != null)
            {
                Path.Add(CurrentTile);
                CurrentTile = CurrentTile.Parent;
            }            
            Path.Reverse();
            foreach(AStarTile tile in Path)
            {
                Grid.FloorLayer[tile.X,tile.Z].GetComponentInChildren<Renderer>().material.color = new Color(1, 0, 0, 1f);
            }
        }
    }

    void AddToOpen(AStarTile Tile)
    {
        foreach(AStarTile tile in Open)
        {
            if(Tile.Pos == tile.Pos)
            {
                return;
            }
        }
        foreach (AStarTile tile in Closed)
        {
            if (Tile.Pos == tile.Pos)
            {
                return;

            }
        }
        Open.Add(Tile);
    }

    public void ForceRecheck()
    {
        Done = false;
        HasPath = false;
        PathAvailable = true;
        StepInPath = 0;
        Open = new List< AStarTile > ();
        Closed = new List<AStarTile>();
        foreach (AStarTile tile in Path)
        {
            Grid.FloorLayer[tile.X, tile.Z].GetComponentInChildren<Renderer>().material.color = new Color(1, 1, 1, 1f);
        }
        Path = new List<AStarTile>();        
        StartPos = new Vector3((int)transform.position.x, 1, (int)transform.position.z);
        CurrentTile = new AStarTile(StartPos, StartPos, EndPos, null);
        Closed.Add(CurrentTile);
    }
}

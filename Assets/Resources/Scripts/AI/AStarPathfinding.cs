using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {

    public Vector3 CurrentPosition;
    public Vector3 TargetPosition;
    public bool TargetPositionAdded;
    public bool PathFound;
    public AStarTile CurrentTile;

    public List<AStarTile> Open = new List<AStarTile>();
    public List<AStarTile> Closed = new List<AStarTile>();

    public List<AStarTile> Path = new List<AStarTile>();

    private Grid Grid;

    void Start () {
        Grid = GameObject.Find("_Initializer").GetComponent<Grid>();
        CurrentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        CurrentTile = new AStarTile((int)CurrentPosition.x, (int)CurrentPosition.z,0,
            (int)(Mathf.Abs(TargetPosition.x-CurrentPosition.x)*10 + Mathf.Abs(TargetPosition.z - CurrentPosition.z) * 10),
            null);
        AddToClosed(CurrentTile);
    }
	
	void Update () {
        if (!PathFound)
        {
            Path.Clear();
            while (!TargetPositionAdded)
            {
                //Add Adjacent tiles (check if its target position, if so set TargetPositionAdded to true
                int rightI = CurrentTile.X + 1;
                AStarTile newTile;
                if (rightI < Grid.MapSize - 1 && Grid.ObjectLayer[rightI, CurrentTile.Z] == null)
                {
                    newTile = new AStarTile(rightI, CurrentTile.Z, CurrentTile.G + 10,
                                        (((int)TargetPosition.x - rightI) * 10) + (((int)TargetPosition.z - CurrentTile.Z) * 10),
                                        CurrentTile);
                    AddToOpen(newTile);
                }
                int leftI = CurrentTile.X - 1;
                if (leftI > 1 && Grid.ObjectLayer[leftI, CurrentTile.Z] == null)
                {
                    newTile = new AStarTile(leftI, CurrentTile.Z, CurrentTile.G + 10,
                                        (((int)TargetPosition.x - leftI) * 10) + (((int)TargetPosition.z - CurrentTile.Z) * 10),
                                        CurrentTile);
                    AddToOpen(newTile);
                }
                int forwardI = CurrentTile.Z + 1;
                if (forwardI < Grid.MapSize - 1 && Grid.ObjectLayer[CurrentTile.X, forwardI] == null)
                {
                    newTile = new AStarTile(CurrentTile.X, forwardI, CurrentTile.G + 10,
                                        (((int)TargetPosition.x - CurrentTile.X) * 10) + (((int)TargetPosition.z - forwardI) * 10),
                                        CurrentTile);
                    AddToOpen(newTile);
                }
                int backwardI = CurrentTile.Z - 1;
                if (backwardI > 1 && Grid.ObjectLayer[CurrentTile.X, backwardI] == null)
                {
                    newTile = new AStarTile(CurrentTile.X, backwardI, CurrentTile.G + 10,
                                        (((int)TargetPosition.x - CurrentTile.X) * 10) + (((int)TargetPosition.z - backwardI) * 10),
                                        CurrentTile);
                    AddToOpen(newTile);
                }

                int lowestF = int.MaxValue;
                foreach (AStarTile tile in Open)
                {
                    if (tile.X == TargetPosition.x && tile.Z == TargetPosition.z)
                    {
                        TargetPositionAdded = true;
                        AddToClosed(Open[Open.Count - 1]);
                        Open.Clear();
                        return;
                    }
                    if (tile.F < lowestF)
                    {
                        CurrentTile = tile;
                        lowestF = tile.F;
                    }
                }
                AddToClosed(CurrentTile);
                Open.Remove(CurrentTile);
            }
            //Setu Path
            CurrentTile = Closed[Closed.Count - 1];
            Path.Add(CurrentTile);
            while (CurrentTile.Parent != null)
            {
                CurrentTile = CurrentTile.Parent;
                Path.Add(CurrentTile);
            }
            PathFind();
        }
    }
    void PathFind()
    {
        foreach (AStarTile tile in Path)
        {
            Debug.Log(tile.X + " " + tile.Z);
        }
        PathFound = true;
        Open.Clear();
        Closed.Clear();
    }
    void AddToOpen(AStarTile tile)
    {
        if(!Open.Contains(tile) && !Closed.Contains(tile)){
            Open.Add(tile);
        }
    }
    void AddToClosed(AStarTile tile)
    {
        if (!Closed.Contains(tile))
        {
            Closed.Add(tile);
        }
    }
}

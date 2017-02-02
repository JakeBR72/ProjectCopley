using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingController : MonoBehaviour{
    public List<JumpPointSearch> JPS_units = new List<JumpPointSearch>();
   
    public void Add(JumpPointSearch toAdd)
    {
        JPS_units.Add(toAdd);
    }

    public void NewPath()
    {
        foreach(JumpPointSearch jps in JPS_units)
        {
            jps.NewPath();
        }
    }
}

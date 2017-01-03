using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public GameObject Floor;
    public GameObject Wall;

    List<GameObject> FloorLayer = new List<GameObject>();
    List<GameObject> ObjectLayer = new List<GameObject>();
    int MapSize = 5;


    void Start () {
		for(int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if(i == 0 || i == MapSize-1 || j == 0 || j == MapSize-1)
                {

                    FloorLayer.Add(Instantiate(Wall, new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0), transform));
                }
                else
                {
                    FloorLayer.Add(Instantiate(Floor, new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0), transform));
                }
            }
        }
	}
	

	void Update () {
		
	}    
}

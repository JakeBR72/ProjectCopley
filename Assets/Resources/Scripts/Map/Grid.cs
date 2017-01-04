using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public GameObject Floor;
    public GameObject Wall;
    
    public static int MapSize = 5;
    public GameObject[,] FloorLayer = new GameObject[MapSize,MapSize];
    public GameObject[,] ObjectLayer = new GameObject[MapSize,MapSize];


    void Start () {
        
		for(int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if(i == 0 || i == MapSize-1 || j == 0 || j == MapSize-1)
                {
                   FloorLayer[i,j] = (Instantiate(Wall, new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0), transform.GetChild(1))); 
                }
                else
                {
                    FloorLayer[i,j] = (Instantiate(Floor, new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0), transform.GetChild(1)));
                }
            }
        }
	}
	

	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildScript : MonoBehaviour {

    public GameObject Wall;
    private GameObject ObjectLayerObj;
    private MenuManager MM;
    private MouseControls Mouse;
    private Grid Grid;
    private GameObject CurrentObject;
    private Tile CurrentTile;
    private Vector3 MousePos;
	void Start () {
        MM = GameObject.Find("_UI").GetComponent<MenuManager>();
        Mouse = GameObject.Find("_CamController").GetComponent<MouseControls>();
        Grid = GameObject.Find("_Initializer").GetComponent<Grid>();
        ObjectLayerObj = GameObject.Find("_ObjectLayer");
    }
	
	void Update () {
        if (CurrentObject != null)
        {
            CurrentTile = CurrentObject.GetComponentInChildren<Tile>();
            MM.CloseMenu();
            Mouse.enabled = false;

            Ray ray;
            RaycastHit rayHit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out rayHit) && rayHit.collider.transform.tag == "Interactable")
                {
                    MousePos = rayHit.point;
                    MousePos = new Vector3(Mathf.Round(MousePos.x), Mathf.Round(MousePos.y), Mathf.Round(MousePos.z));
                    MousePos += new Vector3(0f, 1, 0f);
                    CurrentObject.transform.position = MousePos;
                }
            }
            if (Input.GetMouseButtonDown(0) && Grid.ObjectLayer[(int)CurrentObject.transform.position.x, (int)CurrentObject.transform.position.z] == null)
            {
                Grid.ObjectLayer[(int)CurrentObject.transform.position.x, (int)CurrentObject.transform.position.z] = CurrentObject;
                
                   
                //Continue Building?             
                switch (CurrentTile.Type) {
                    case TileTypes.Wall:
                        BuildWall();
                        break;

                    default:
                        CurrentObject = null;
                        ResetObjects();
                        break;
                }


            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {                
                Destroy(CurrentObject);
                ResetObjects();
                CurrentObject = null;
            }
        }else
        {
            Mouse.enabled = true;
        }
	}
    void ResetObjects()
    {
        for (int i = 0; i < Grid.MapSize; i++)
        {
            for (int j = 0; j < Grid.MapSize; j++)
            {
                if (Grid.ObjectLayer[i, j] != null)
                {
                    foreach (Transform child in Grid.ObjectLayer[i, j].transform)
                    {
                        child.gameObject.tag = "Interactable";
                    }
                }
            }
        }
    }
    public void BuildWall()
    {
        CurrentObject = Instantiate(Wall,new Vector3(0,-1000,0),new Quaternion(0,0,0,0),ObjectLayerObj.transform);
        for (int i = 0; i < Grid.MapSize; i++)
        {
            for (int j = 0; j < Grid.MapSize; j++)
            {
                if (Grid.ObjectLayer[i, j] != null)
                {
                    foreach (Transform child in Grid.ObjectLayer[i, j].transform)
                    {
                        child.gameObject.tag = "Untagged";
                    }
                }
            }
        }
    }
}

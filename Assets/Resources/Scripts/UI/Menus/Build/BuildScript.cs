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
    private Renderer CurrRend;
    public int Mode;
    
	void Start () {
        MM = GameObject.Find("_UI").GetComponent<MenuManager>();
        Mouse = GameObject.Find("_CamController").GetComponent<MouseControls>();
        Grid = GameObject.Find("_Initializer").GetComponent<Grid>();
        ObjectLayerObj = GameObject.Find("_ObjectLayer");
    }
	
	void Update () {
        if(Mode == 0)
        {
            Build();
        }else if (Mode == 1)
        {
            Destroy();
        }
	}

    void Build()
    {
        if (CurrentObject != null)
        {
            CurrRend = CurrentObject.GetComponentInChildren<Renderer>();
            CurrRend.material.color = new Color(0f, 1f, 1, 0.5f);
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
                    if (Input.GetMouseButton(0) && Grid.ObjectLayer[(int)CurrentObject.transform.position.x, (int)CurrentObject.transform.position.z] == null)
                    {
                        Grid.ObjectLayer[(int)CurrentObject.transform.position.x, (int)CurrentObject.transform.position.z] = CurrentObject;
                        Grid.ForceUpdate();
                        CurrRend.material.color = new Color(1, 1, 1, 1f);

                        //Continue Building Whitelist:             
                        switch (CurrentTile.Type)
                        {
                            case TileTypes.Wall:
                                BuildWall();
                                break;

                            default:
                                CurrentObject = null;
                                ResetObjects();
                                break;
                        }
                    }
                }
                else
                {
                    CurrentObject.transform.position = new Vector3(0, -1000, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(CurrentObject);
                ResetObjects();
                CurrentObject = null;
            }
        }
        else
        {
            Mouse.enabled = true;
        }
    }
    private void Destroy()
    {
        MM.CloseMenu();
        Mouse.enabled = false;
        Ray ray;
        RaycastHit rayHit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out rayHit) && rayHit.collider.transform.tag == "Interactable" && rayHit.collider.transform.position.y == 1)
            {
                if (CurrentObject != null && CurrentObject != rayHit.collider.transform)
                {
                    CurrentObject.GetComponent<Renderer>().material.SetColor("_Emission", new Color(0, 0, 0, 1));
                }
                CurrentObject = rayHit.collider.gameObject;
                CurrentObject.GetComponent<Renderer>().material.SetColor("_Emission", new Color(1, 0, 0, 1));
                if (Input.GetMouseButtonDown(0))
                {
                    Grid.ObjectLayer[(int)CurrentObject.transform.position.x, (int)CurrentObject.transform.position.z] = null;
                    Destroy(CurrentObject);
                    CurrentObject = null;
                    Grid.ForceUpdate();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Mode = 0;
            CurrentObject.GetComponent<Renderer>().material.SetColor("_Emission", new Color(0, 0, 0, 1));
            CurrentObject = null;
            Mouse.enabled = true;
            ResetObjects();
        }
    }
    public void SetMode(int i)
    {
        ResetObjects();
        Mode = i;
    }
    public void Stop()
    {
        Mode = 0;
        if (CurrentObject != null) { 
            CurrentObject.GetComponentInChildren<Renderer>().material.SetColor("_Emission", new Color(0, 0, 0, 1));            
        }
        CurrentObject = null;
    }
    public void BuildWall()
    {
        CurrentObject = Instantiate(Wall,new Vector3(0,-1000,0),new Quaternion(0,0,0,0),ObjectLayerObj.transform);
        PrimeObjects();
    }

    void PrimeObjects()
    {
        foreach(Transform child in CurrentObject.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        for (int i = 0; i < Grid.MapSize; i++)
        {
            for (int j = 0; j < Grid.MapSize; j++)
            {
                if (Grid.ObjectLayer[i, j] != null)
                {
                    foreach (Transform child in Grid.ObjectLayer[i, j].transform)
                    {
                        child.gameObject.tag = "Interactable";
                        child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }
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
                        child.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
        }
    }
}

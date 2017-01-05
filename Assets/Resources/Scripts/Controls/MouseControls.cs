using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour {
    public Vector3 MousePos;
    public GameObject CurrentTarget;
    private Transform LastTransform;
    private Shader LastShader;
    private MenuManager UI;
    private InteractCanvas InteractCanvas;
    public bool enabled = true;
	void Start () {
        UI = GameObject.Find("_UI").GetComponent<MenuManager>();
	}	

	void Update () {
        Cursor.lockState = CursorLockMode.Confined;
        if (enabled)
        {
            Ray ray;
            RaycastHit rayHit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && (Input.GetMouseButtonDown(0)))
            {
                if (Physics.Raycast(ray, out rayHit) && rayHit.collider.transform.tag == "Interactable")
                {
                    if(LastTransform != null && LastTransform != rayHit.collider.transform)
                    {
                        ResetTile();
                    }
                    LastTransform = rayHit.collider.transform;
                    LastShader = LastTransform.GetComponent<Renderer>().material.shader;
                    CurrentTarget = LastTransform.gameObject;
                    MousePos = rayHit.point;
                    MousePos = new Vector3(Mathf.Round(MousePos.x), Mathf.Round(MousePos.y), Mathf.Round(MousePos.z));
                    MousePos += new Vector3(0.5f, 0, 0.5f);
                    //Set Highlight Color
                    LastTransform.GetComponent<Renderer>().material.SetColor("_Emission",new Color(1,1,1,0.25f));
                    Tile CurrentTile = rayHit.collider.gameObject.GetComponentInChildren<Tile>();
                    UI.OpenMenu("Interact");
                    InteractCanvas = UI.returnObj.GetComponent<InteractCanvas>();
                    InteractCanvas.SetTitle(CurrentTile.Type);
                }
            }else if (UI.MenusClosed && LastTransform != null)
            {
                ResetTile();
            }
        }
    }

    void ResetTile()
    {
        LastTransform.GetComponent<Renderer>().material.SetColor("_Emission", new Color(0, 0, 0, 0));
    }
}

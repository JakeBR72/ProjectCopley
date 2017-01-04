using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour {
    public Vector3 MousePos;
    public GameObject CurrentTarget;
    private Transform LastTransform;
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
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out rayHit) && rayHit.collider.transform.tag == "Interactable")
                {
                    if (rayHit.transform != LastTransform && LastTransform != null)
                    {
                        LastTransform.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    }
                    LastTransform = rayHit.collider.transform;
                    CurrentTarget = LastTransform.gameObject;
                    MousePos = rayHit.point;
                    MousePos = new Vector3(Mathf.Round(MousePos.x), Mathf.Round(MousePos.y), Mathf.Round(MousePos.z));
                    MousePos += new Vector3(0.5f, 0, 0.5f);
                    LastTransform.GetComponent<Renderer>().material.shader = Shader.Find("Custom/GlowOutline");
                    if (Input.GetMouseButtonDown(0))
                    {
                        Tile CurrentTile = rayHit.collider.gameObject.GetComponentInChildren<Tile>();
                        UI.OpenMenu("Interact");
                        InteractCanvas = UI.returnObj.GetComponent<InteractCanvas>();
                        InteractCanvas.SetTitle(CurrentTile.Type);
                    }
                }
                else
                {
                    if (LastTransform != null)
                    {
                        LastTransform.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                        MousePos = new Vector3(0, -10000, 0);
                    }
                }
            }
            else
            {
                if (LastTransform != null)
                {
                    LastTransform.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    MousePos = new Vector3(0, -10000, 0);
                }
            }
        }
    }

    public Vector3 GetMousePos()
    {
        return MousePos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour {

    MenuManager MenuManager;

	void Start () {
        MenuManager = GameObject.Find("_UI").GetComponent<MenuManager>();
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.CloseMenu();
        }

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractCanvas : MonoBehaviour {
    public Text Title;
    public Text Description;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void SetTitle(string title)
    {
        Title.text = title;
    }
}

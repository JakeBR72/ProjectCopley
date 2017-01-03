using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public RectTransform CurrentMenu;
    public RectTransform LastMenu;
    public GameObject returnObj;

    public int MoveSpeed = 5;

    public List<GameObject> CanvasList = new List<GameObject>();

	void Start () {
        ResetMenus();
	}
	
	void Update () {
        
            if (CurrentMenu != null && CurrentMenu != LastMenu)
            {
                //if(LastMenu == null || (LastMenu != null && LastMenu.anchoredPosition.x > (int)(LastMenu.rect.width * 0.9f)))
                CurrentMenu.anchoredPosition = Vector2.Lerp(CurrentMenu.anchoredPosition,
                                                    new Vector2(0, 0),
                                                    MoveSpeed*Time.deltaTime);
            }
            if (LastMenu != null)
            {
                LastMenu.anchoredPosition = Vector2.Lerp(LastMenu.anchoredPosition,
                                                new Vector2(LastMenu.rect.width, 0),
                                                MoveSpeed*Time.deltaTime);
            }
    }

    public void OpenMenu(string menuName)
    {
        returnObj = null;
        LastMenu = CurrentMenu;
        foreach(GameObject obj in CanvasList)
        {
            if (obj != null)
            {
                if (obj.name == menuName)
                {
                    CurrentMenu = obj.GetComponent<RectTransform>();
                    if(LastMenu == CurrentMenu)
                    {
                        LastMenu = null;
                    }
                    returnObj = obj;
                }
            }
        }  
    }
    public void CloseMenu()
    {
        LastMenu = CurrentMenu;
    }
    public void ResetMenus()
    {
        foreach(GameObject obj in CanvasList)
        {
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(obj.GetComponent<RectTransform>().rect.width, 0);
        }
    }
}

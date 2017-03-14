using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShotCircle : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot;
    // Update is called once per frame
    void Start()
    {
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}

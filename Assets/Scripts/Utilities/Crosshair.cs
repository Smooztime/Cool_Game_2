using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Texture2D _cursor;

    // Update is called once per frame
    void Update()
    {
        //Change cursor to crosshair
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}

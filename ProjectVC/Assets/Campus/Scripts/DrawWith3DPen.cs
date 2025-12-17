/*
 * Script for a pen to draw and write on a white board texture
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWith3DPen : MonoBehaviour
{

    public GameObject penObject; // The virtual pen object
    public GameObject whiteboardObject; // The 3D whiteboard object
    public Color drawColor = Color.black; //Color of the pen
    public int brushSize = 1; //Thickness of the drawed line
    public float maxDistance = 0.9f; // Distance from the whiteboard at which drawing should occur

    private Texture2D texture;
    private bool isDrawing;
    private Vector2 previousUV;

    //
    void Start()
    {
        Renderer renderer = whiteboardObject.GetComponent<Renderer>();
        texture = new Texture2D(1024, 1024);
        texture.wrapMode = TextureWrapMode.Clamp;
        renderer.material.mainTexture = texture;
        ClearTexture();
    }

    void Update()
    {
        Vector2 currentUV;
        Ray ray = new Ray(penObject.transform.position, -penObject.transform.up);

        //Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 0.1f);

        if (IsPenNearWhiteboard(out currentUV))
        {
            if (!isDrawing)
            {
                isDrawing = true;
                previousUV = currentUV;
            }

            if (currentUV != previousUV)
            {
                DrawLineToTexture(previousUV, currentUV);
                previousUV = currentUV;
            }
        }
        else
        {
            isDrawing = false;
        }
    }
    //Pen should only draw when its close to the board
    bool IsPenNearWhiteboard(out Vector2 uv)
    {
        uv = Vector2.zero;
        Ray ray = new Ray(penObject.transform.position, -penObject.transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance) && hit.transform.gameObject == whiteboardObject)
        {
            //Debug.Log("Hit Detected: " + hit.textureCoord);
            uv = hit.textureCoord;
            return true;
        }
        return false;
    }
    
    //Apply paint to the pen
    void DrawLineToTexture(Vector2 from, Vector2 to)
    {
        int width = (int)(to.x * texture.width);
        int height = (int)(to.y * texture.height);
        int previousWidth = (int)(from.x * texture.width);
        int previousHeight = (int)(from.y * texture.height);

        for (float t = 0.0f; t < 1.0f; t += 1.0f / Mathf.Max(Vector2.Distance(from, to) * 500, 1))
        {
            int x = (int)Mathf.Lerp(previousWidth, width, t);
            int y = (int)Mathf.Lerp(previousHeight, height, t);

            for (int i = -brushSize; i <= brushSize; i++)
            {
                for (int j = -brushSize; j <= brushSize; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    if (nx >= 0 && nx < texture.width && ny >= 0 && ny < texture.height)
                    {
                        texture.SetPixel(nx, ny, drawColor);
                    }
                }
            }
        }
        texture.Apply();
    }

    //Change all painted lines to the color of the board
    void ClearTexture()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }

    //Remove any paint on the board
    public void ClearDrawing()
    {
        ClearTexture();
    }
}

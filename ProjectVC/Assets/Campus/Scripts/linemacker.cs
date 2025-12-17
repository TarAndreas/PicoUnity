using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * Render a line to every point in order
 */
public class linemacker : MonoBehaviour
{
    public List<GameObject> positions = new List<GameObject>();
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = positions.Count;
        for(int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i].transform.position);
        }
    }
}

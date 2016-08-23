using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    Vector3 offset;
    Vector3 PreMouseMPos;
    float zoomSpeed=300;
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float step = zoomSpeed * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - step * Input.GetAxis("Mouse ScrollWheel"), 5,20);
            if(Input.GetAxis("Mouse ScrollWheel")>0)
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition),Input.GetAxis("Mouse ScrollWheel") * step);
        }
        if (Input.GetMouseButton(2))
        {

            if (PreMouseMPos.x == 0)
            {
                PreMouseMPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            }
            else
            {
                Vector3 CurMouseMPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
                Vector3 offset = CurMouseMPos - PreMouseMPos;
                offset = -offset * 0.04f;
                transform.Translate(offset);
                PreMouseMPos = CurMouseMPos;
            }
        }
        else
        {
            PreMouseMPos = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}

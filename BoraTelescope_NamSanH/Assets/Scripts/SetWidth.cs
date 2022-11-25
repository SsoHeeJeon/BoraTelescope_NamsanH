using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetWidth : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    PointerEventData m_PointerEventDatas;
    EventSystem m_EventSystem;

    public int CameraWidth;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        //m_PointerEventData.position = Input.mousePosition;
        m_PointerEventData.position = new Vector2(Screen.width, Screen.height);

        //Set up the new Pointer Event
        m_PointerEventDatas = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        //m_PointerEventData.position = Input.mousePosition;
        m_PointerEventDatas.position = new Vector2(0, Screen.height);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        m_Raycaster.Raycast(m_PointerEventDatas, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //Debug.Log(results.Count);
        if (results.Count >= 2)
        {
            for (int i = 0; i < results.Count; i++)
            {
                //Debug.Log($"{results[i].worldPosition}");
            }
            if (results.Count == 2)
            {
                CameraWidth = (int)(Mathf.Abs(results[1].worldPosition.x - results[0].worldPosition.x));
            } else if(results.Count > 2 && Mathf.Abs(results[1].worldPosition.x - results[0].worldPosition.x) == 0)
            {
                CameraWidth = (int)(Mathf.Abs(results[2].worldPosition.x - results[0].worldPosition.x));
            }
            //Debug.Log(CameraWidth);
        }
        //foreach (RaycastResult result in results)
        //{
        //    Debug.Log($"{result.worldPosition}");
        //}
    }
}

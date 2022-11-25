using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDragRange : MonoBehaviour
{
    public GameManager gamemanager;

    public void ALLFuncDragRange()
    {
        if (Input.GetTouch(0).position.x <= 430 && Input.GetTouch(0).position.x >= 70 && Input.GetTouch(0).position.y <= 350 && Input.GetTouch(0).position.y >= 0)
        {
            gamemanager.touchtime = 0;
            if (gamemanager.joystick.enabled == false)
            {
                gamemanager.Arrow.transform.GetChild(0).gameObject.transform.localPosition = Vector3.zero;
            }

            if (gamemanager.NaviRect.sizeDelta.x == GameManager.barOpen)
            {
                gamemanager.Arrow.gameObject.SetActive(false);
            }
            else if (gamemanager.NaviRect.sizeDelta.x == GameManager.barClose)
            {
                gamemanager.Arrow.gameObject.SetActive(true);
                gamemanager.Arrow.transform.position = gamemanager.Arrowpos_normal;
            }
        }
        else
        {
            if (Input.GetTouch(0).position.y > 150)
            {
                gamemanager.DragArrow();
            }
        }
    }
}

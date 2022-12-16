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
                gamemanager.Arrow.transform.position = gamemanager.Arrowpos_extend;
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

    public void ClearModeNofilterRange()
    {
        if (gamemanager.NaviRect.sizeDelta.x == GameManager.barClose && gamemanager.namsanMode.labeldetail.Detail_Background.transform.localPosition.x != gamemanager.namsanMode.labeldetail.Detail_Close_x)  // 네비게이션 비활성화, 상세설명 활성화
        {
            if (Input.GetTouch(0).position.y < 350)
            {
                if (Input.GetTouch(0).position.x <= 1400 && Input.GetTouch(0).position.x >= 430 && Input.GetTouch(0).position.y > 120)
                {
                    gamemanager.namsanMode.see360.SeeAllRound();
                }
                else
                {
                    //gamemanager.clearMode.DragStop = true;
                }
            }
            else if (Input.GetTouch(0).position.y >= 350)
            {
                if (Input.GetTouch(0).position.x >= 70 && Input.GetTouch(0).position.x <= 1400)
                {
                    gamemanager.namsanMode.see360.SeeAllRound();
                }
                else
                {
                    //gamemanager.clearMode.DragStop = true;
                }
            }
        }
        else if (gamemanager.NaviRect.sizeDelta.x == GameManager.barClose && gamemanager.namsanMode.labeldetail.Detail_Background.transform.localPosition.x == gamemanager.namsanMode.labeldetail.Detail_Close_x)   // 네비게이션 비활성화, 상세설명 비활성화
        {
            if (Input.GetTouch(0).position.x <= 430 && Input.GetTouch(0).position.x >= 70 && Input.GetTouch(0).position.y <= 350 && Input.GetTouch(0).position.y >= 0)
            {
                //gamemanager.clearMode.DragStop = true;
            }
            else
            {
                if (Input.GetTouch(0).position.y > 120)
                {
                    gamemanager.namsanMode.see360.SeeAllRound();
                }
            }
        }
        else if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose && gamemanager.namsanMode.labeldetail.Detail_Background.transform.localPosition.x != gamemanager.namsanMode.labeldetail.Detail_Close_x)  // 네비게이션 활성화, 상세설명 활성화
        {
            if (Input.GetTouch(0).position.x >= 410 && Input.GetTouch(0).position.x <= 1400 && Input.GetTouch(0).position.y > 120)
            {
                gamemanager.namsanMode.see360.SeeAllRound();
            }
            else
            {
                //gamemanager.clearMode.DragStop = true;
            }
        }
        else if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose && gamemanager.namsanMode.labeldetail.Detail_Background.transform.localPosition.x == gamemanager.namsanMode.labeldetail.Detail_Close_x)  // 네비게이션 활성화, 상세설명 비활성화
        {
            if (Input.GetTouch(0).position.x >= 410 && Input.GetTouch(0).position.y > 120)
            {
                gamemanager.namsanMode.see360.SeeAllRound();
            }
            else
            {
                //gamemanager.clearMode.DragStop = true;
            }
        }
    }
}

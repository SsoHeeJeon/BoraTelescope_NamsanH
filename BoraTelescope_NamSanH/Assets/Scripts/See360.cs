using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class See360 : MonoBehaviour
{
    public GameObject obj360;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).position.y > 300 || Input.mousePosition.y > 300)
            {
                SeeAllRound();
            }
        }
    }

    float bx;
    float by;
    float fx;
    float fy;
    string dir_360;
    Vector3 beforepos;
    private float dragmove_t;

    private Vector3 firsttouchmove;
    private Vector3 secondtouchmove;
    public static bool gosecond = false;
    public static bool dontclick = false;

    public void SeeAllRound()
    {
        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            if (Input.touches[0].phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    beforepos = Input.mousePosition;
                    bx = beforepos.x;
                    by = beforepos.y;
                }
                else
                {
                    beforepos = Input.GetTouch(0).position;
                    bx = beforepos.x;
                    by = beforepos.y;
                }
            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x != bx)
                {
                    fx = (bx - Input.mousePosition.x);
                    fy = (by - Input.mousePosition.y);

                    dragmove_t += Time.deltaTime;

                    if ((int)dragmove_t >= 0 && (int)dragmove_t < 1)
                    {
                        firstmoveposition();
                    }
                    exceptfilterWin();
                }
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
            {
                fx = 0;
                fy = 0;
                obj360.transform.rotation = Quaternion.Euler(obj360.transform.rotation.eulerAngles.x, obj360.transform.rotation.eulerAngles.y, 0);
                dontclick = false;
                gosecond = false;
                dragmove_t = 0;
                firsttouchmove = Vector3.zero;
                secondtouchmove = Vector3.zero;
            }
        }else if(Input.touchCount == 0)
        {
            fx = 0;
            fy = 0;
            obj360.transform.rotation = Quaternion.Euler(obj360.transform.rotation.eulerAngles.x, obj360.transform.rotation.eulerAngles.y, 0);
            dontclick = false;
            gosecond = false;
            dragmove_t = 0;
            firsttouchmove = Vector3.zero;
            secondtouchmove = Vector3.zero;
        }
    }

    public void firstmoveposition()
    {
        gosecond = true;
        Invoke("secondmoveposition", 0.01f);
    }

    public void secondmoveposition()
    {
        if (gosecond == true)
        {
            beforepos = Input.mousePosition;

            bx = beforepos.x;
            by = beforepos.y;

            secondtouchmove = Input.mousePosition;

            dragmove_t = 0;
            firsttouchmove = Vector3.zero;
            secondtouchmove = Vector3.zero;
            if (Vector3.Distance(firsttouchmove, secondtouchmove) < 0.05f && dragmove_t >= 0.35f)
            {
                dontclick = true;
                gosecond = false;
            }
        }
    }

    public void exceptfilterWin()
    {
        if (!(Input.mousePosition.x <= 1385f && Input.mousePosition.x >= 550f && Input.mousePosition.y <= 1030f && Input.mousePosition.y >= 890f))
        {
            touchmovecamera();
        }
    }

    public void touchmovecamera()
    {
        obj360.transform.rotation = Quaternion.Euler(obj360.transform.rotation.eulerAngles.x, obj360.transform.rotation.eulerAngles.y + fx * 0.5f, 0);
        //obj360.transform.rotation = Quaternion.Euler(obj360.transform.rotation.eulerAngles.x + fy * 0.1f, obj360.transform.rotation.eulerAngles.y + fx * 0.5f, 0);
        //CameraWindow.transform.position = new Vector3(CameraWindow.transform.position.x + fx, CameraWindow.transform.position.y + fy, CameraWindow.transform.position.z);
    }

    public void Close360()
    {
        //obj360.transform.parent = gamemanager.gameObject.transform;
        obj360.transform.localPosition = Vector3.zero;
        obj360.gameObject.SetActive(false);
        //gamemanager.UI_All.gameObject.SetActive(true);
        //for (int index = 0; index < gamemanager.UI_All.transform.childCount; index++)
        //{
        //    gamemanager.UI_All.transform.GetChild(index).gameObject.SetActive(true);
        //}
        //gamemanager.Arrow.gameObject.SetActive(false);
        //gamemanager.UI_All.transform.GetChild(5).gameObject.SetActive(false);
        //gamemanager.AutoSelectImg.gameObject.SetActive(false);

        //FunctionCustom.SetContentsFunc();
    }
}

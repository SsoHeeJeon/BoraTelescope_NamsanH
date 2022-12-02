using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ScreenCapture : UploadImage
{
    private Camera camera;       //�������� ī�޶�.

    public GameObject flasheffect;
    public GameObject customMark;

    private int resWidth;
    private int resHeight;
    string path;

    public static float count;
    public static bool counttime = false;
    // Use this for initialization
    public void ReadyToCapture()
    {
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        else if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            camera = GameObject.Find("CameraZoom").transform.GetChild(0).gameObject.GetComponent<Camera>();
        }

        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        QRCodeImage.transform.parent.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
        Debug.Log(path);
    }

    public void ClickScreenShot()
    {
        GetBoranum();
        //flasheffect.gameObject.SetActive(false);
        counttime = true;
        //customMark.gameObject.SetActive(true);
        QRCodeImage.transform.parent.gameObject.SetActive(true);
        QRCodeImage.transform.parent.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        QRCodeImage.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        //flasheffect.gameObject.SetActive(false);
        //startflasheffect = false;
        StartCoroutine(CaptureandSave());
    }

    private IEnumerator CaptureandSave()
    {
        yield return new WaitForEndOfFrame();
        
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        string filename;
        name = path + boranum + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        filename = boranum + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        Debug.Log("NOW" + name);
        //startflasheffect = false;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;

        screenShot.ReadPixels(rec, 0, 0);
        //screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        //startflasheffect = false;
        Debug.Log(bytes);
        File.WriteAllBytes(name, bytes);
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.AR_Capture, "AR_Capture", GetType().ToString());
        }
        else if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_Capture, "NamSanH_Capture", GetType().ToString());
        }
        PutImageObject(name, filename);
        camera.targetTexture = null;

        Destroy(screenShot);

        //startflasheffect = false;
    }

    public static bool startflasheffect = false;
    bool changed = true;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1, 1, 1, 0.745f);

    public void playFlashEffect()
    {
        if (startflasheffect == true)
        {
            if (changed)
            {
                flasheffect.GetComponent<Image>().color = flashColor;

                changed = false;
            }
            else
            {
                flasheffect.GetComponent<Image>().color = Color.Lerp(flasheffect.GetComponent<Image>().color, Color.clear, flashSpeed * Time.deltaTime);
                if (flasheffect.GetComponent<Image>().color.a < 0.1f)
                {
                    startflasheffect = false;
                    flasheffect.gameObject.SetActive(false);
                    gamemanager.WaitStartCap();
                }
            }
            //changed = false;
        }
        //else
        //{
        //    flasheffect.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        //}
    }

    //    /***********************************************************************
    //    *                               Button Event Handlers
    //    ***********************************************************************/
    //    #region .
    //    /// <summary> UI ���� ��ü ȭ�� ĸ�� </summary>
    //    public void TakeScreenShotFull()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageWrite, () => StartCoroutine(TakeScreenShotRoutine()));
    //#else
    //        StartCoroutine(TakeScreenShotRoutine());
    //#endif
    //    }

    //    /// <summary> UI ������, ���� ī�޶� �������ϴ� ȭ�鸸 ĸ�� </summary>
    //    public void TakeScreenShotWithoutUI()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageWrite, () => _willTakeScreenShot = true);
    //#else
    //        _willTakeScreenShot = true;
    //#endif
    //    }

    //    private void ReadScreenShotAndShow()
    //    {
    //#if UNITY_ANDROID
    //        CheckAndroidPermissionAndDo(Permission.ExternalStorageRead, () => ReadScreenShotFileAndShow(imageToShow));
    //#else
    //        ReadScreenShotFileAndShow(imageToShow);
    //#endif
    //    }
    //    #endregion
    //    /***********************************************************************
    //    *                               Methods
    //    ***********************************************************************/
    //    #region .

    //    // UI �����Ͽ� ���� ȭ�鿡 ���̴� ��� �� ĸ��
    //    private IEnumerator TakeScreenShotRoutine()
    //    {
    //        yield return new WaitForEndOfFrame();
    //        CaptureScreenAndSave();
    //    }

    //    // UI �����ϰ� ���� ī�޶� �������ϴ� ��� ĸ��
    //    private void OnPostRender()
    //    {
    //        if (_willTakeScreenShot)
    //        {
    //            _willTakeScreenShot = false;
    //            CaptureScreenAndSave();
    //        }
    //    }
    //    /// <summary> ��ũ������ ��� ��ο� �����ϱ� </summary>
    //    private void CaptureScreenAndSave()
    //    {
    //        string totalPath = TotalPath; // ������Ƽ ���� �� �ð��� ���� �̸��� �����ǹǷ� ĳ��

    //        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    //        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

    //        // ���� ��ũ�����κ��� ���� ������ �ȼ����� �ؽ��Ŀ� ����
    //        screenTex.ReadPixels(area, 0, 0);

    //        bool succeeded = true;
    //        try
    //        {
    //            // ������ �������� ������ ���� ����
    //            if (Directory.Exists(FolderPath) == false)
    //            {
    //                Directory.CreateDirectory(FolderPath);
    //            }

    //            // ��ũ���� ����
    //            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG());
    //        }
    //        catch (Exception e)
    //        {
    //            succeeded = false;
    //            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
    //            Debug.LogWarning(e);
    //        }

    //        // ������ �۾�
    //        Destroy(screenTex);

    //        if (succeeded)
    //        {
    //            Debug.Log($"Screen Shot Saved : {totalPath}");
    //            //flash.Show(); // ȭ�� ��½
    //            lastSavedPath = totalPath; // �ֱ� ��ο� ����
    //        }

    //        // ������ ����
    //        RefreshAndroidGallery(totalPath);
    //    }


    //    // ���� �ֱٿ� ����� �̹��� �����ֱ�
    //    /// <summary> ��ηκ��� ����� ��ũ���� ������ �о �̹����� �����ֱ� </summary>
    //    private void ReadScreenShotFileAndShow(Image destination)
    //    {
    //        string folderPath = FolderPath;
    //        string totalPath = lastSavedPath;

    //        if (Directory.Exists(folderPath) == false)
    //        {
    //            Debug.LogWarning($"{folderPath} ������ �������� �ʽ��ϴ�.");
    //            return;
    //        }
    //        if (File.Exists(totalPath) == false)
    //        {
    //            Debug.LogWarning($"{totalPath} ������ �������� �ʽ��ϴ�.");
    //            return;
    //        }

    //        // ������ �ؽ��� �ҽ� ����
    //        if (_imageTexture != null)
    //            Destroy(_imageTexture);
    //        if (destination.sprite != null)
    //        {
    //            Destroy(destination.sprite);
    //            destination.sprite = null;
    //        }

    //        // ����� ��ũ���� ���� ��ηκ��� �о����
    //        try
    //        {
    //            byte[] texBuffer = File.ReadAllBytes(totalPath);

    //            _imageTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
    //            _imageTexture.LoadImage(texBuffer);
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.LogWarning($"��ũ���� ������ �д� �� �����Ͽ����ϴ�.");
    //            Debug.LogWarning(e);
    //            return;
    //        }

    //        // �̹��� ��������Ʈ�� ����
    //        Rect rect = new Rect(0, 0, _imageTexture.width, _imageTexture.height);
    //        Sprite sprite = Sprite.Create(_imageTexture, rect, Vector2.one * 0.5f);
    //        destination.sprite = sprite;
    //    }
    //    #endregion
}
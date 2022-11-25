using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CameraSpincam : MonoBehaviour
{
    //private LogSendServer logsave;
    public RawImage CameraWindow;
    Texture2D pickedImage;
    NetworkStream stream;
    public Thread ReadThread;
    public static Thread staticReadThread;
    private Queue<byte[]> queue_bytes = new Queue<byte[]>();

    //tcp
    const string serverIP = "127.0.0.1";
    const int port = 8000;
    TcpClient Client;
    byte[] recevBuffer;

    public static int camWidth = 0;
    public static int camHeight = 0;
    static int DataSize = 0;

    private bool alreadysend = false;
    public static bool EndThread = false;

    // Start is called before the first frame update
    void Start()
    {
        firstflag = true;
        //logsave = GameObject.Find("GameManager").GetComponent<LogSendServer>();
        alreadysend = false;
        SpinCam();
        pickedImage = new Texture2D(camWidth, camHeight, TextureFormat.RGB24, false);
        DataSize = camWidth * camHeight * 3;    // 데이터 사이즈는 width x height x 3 (색상정보)
        recevBuffer = new byte[DataSize];
        staticReadThread = null;
        EndThread = false;
        if (staticReadThread is null)
        {
            staticReadThread = new Thread(new ThreadStart(ReadImage));
            staticReadThread.Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (queue_bytes.Count > 0)
        {
            SeeCameraImage(queue_bytes.Dequeue());
        }
    }

    public void SpinCam()
    {
        var processList = System.Diagnostics.Process.GetProcessesByName("XRTeleSpinCam");
        if (processList.Length == 0)
        {
            System.Diagnostics.Process.Start(@"C:\XRTelesPinCam\XRTeleSpinCam.exe");
        }
           
        if (Client == null)
            Client = new TcpClient(serverIP, port);
        
        //logsave.WriteLog(LogSendServer.NormalLogCode.Connect_Camera, "Connect_Camera_On", GetType().ToString());

        if (camHeight == 0 && camWidth == 0)
        {
            stream = Client.GetStream();
            recevBuffer = null;
            recevBuffer = new byte[9];
            stream.Read(recevBuffer, 0, recevBuffer.Length);    // 영상 사이즈 정보를 가져옴.
                                                                //Debug.Log(recevBuffer);
            string[] camsize = Encoding.UTF8.GetString(recevBuffer).Split('x');
            camWidth = int.Parse(camsize[0]);
            camHeight = int.Parse(camsize[1]);
        }

        CameraWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(camWidth, camHeight);
    }

    // 기존코드 => 스레드를 사용하지 않고 Update문으로만 돌렸는데 fps가 20아래로 나왔음
    // 스레드를 사용하고 rawImage에 받아온 byte를 뿌려주는 것은 Queue를 사용해서 했더니 150이상의 fps가 나옴
    void StartThreadRead()
    {
        //Debug.Log(ReadThread.ThreadState);
        //ReadThread = new Thread(new ThreadStart(ReadImage));
        //ReadThread.Start();
        //Debug.Log(ReadThread.ThreadState);
    }

    bool firstflag = false;

    void ReadImage()
    {
        while (!EndThread)
        {
            if (Client != null)
            {
                stream = Client.GetStream();
                //recevBuffer = new byte[DataSize];
                stream.Read(recevBuffer, 0, recevBuffer.Length); // stream에 있던 바이트배열 내려서 새로 선언한 바이트배열에 넣기
                if (recevBuffer == null) return;

                queue_bytes.Enqueue(recevBuffer);       // recevBuffer의 크기를 할당해놓으면 stream.Read를 통해 자동으로 저장
                alreadysend = false;
            }
            else if (Client == null)
            {
                if (alreadysend == false)
                {
                    //logsave.WriteErrorLog(LogSendServer.ErrorLogCode.Fail_Connect_Camera, "Fail_Connect_Camera", GetType().ToString());
                    //GameManager.AnyError = true;
                    SceneManager.LoadScene("Loading");
                    alreadysend = true;
                }
            }
        }
    }

    public void SeeCameraImage(byte[] cameradatas)
    {
        pickedImage.LoadRawTextureData(cameradatas);
        pickedImage.Apply();
        CameraWindow.texture = pickedImage;
    }
}

/* This is a simple example to show the steps and one possible way of
 * automatically scanning for and connecting to a device to receive
 * notification data from the device.
 */

//　アタッチするオブジェクトを変更した場合は、
//　　AssetBundleRequest/Plugins/BluetoothDeviceScript.cs
//　の320行目あたりにある、
//　　GameObject foundObject = GameObject.Find ("Fox");
//　のFoxの値をアタッチしたオブジェクト名に変更すること。

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BLE_Connect1 : MonoBehaviour
{
	public string DeviceName = "MyESP32";
	public string ServiceUUID = "2220";
	public string SubscribeCharacteristic = "2221";
	public string WriteCharacteristic = "2222";

	enum States
	{
		None,
		Scan,
		ScanRSSI,
		Connect,
		Subscribe,
		Unsubscribe,
		Disconnect,
	}

	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private string _deviceAddress;
	private bool _foundSubscribeID = false;
	private bool _foundWriteID = false;
	private byte[] _dataBytes = null;
	private bool _rssiOnly = false;
	private int _rssi = 0;

    [SerializeField]
    private UnityEngine.UI.Text stateText;

    [SerializeField]
    public UnityEngine.UI.Text msgText;

    //[SerializeField] 
    //private TMP_Text msgText;


    [SerializeField]
    private Text scanText;

    [SerializeField]
    private Text DeviceNameFoundedText;

    //[SerializeField]
    //private GameObject cube;

    private int MoveValue;



    private Transform cubeTransform;

    private List<string> mLogs = new List<string>();

    public string HibyteString = "";

 //   [SerializeField]
 //   private Transform cameraTransform;
    private Quaternion quatLookAtCam;
    private Quaternion rawQuat;
    [SerializeField]
    private GameObject particle;

    private int CntX = 1;

    public void resetQuat()
    {
        //Quaternion cubeQuat = Quaternion.LookRotation(this.transform.position-cubeTransform.position);
        //quatLookAtCam = cubeQuat * Quaternion.Inverse(rawQuat);
        //quatLookAtCam = Quaternion.Inverse(cubeTransform.rotation);
        quatLookAtCam = Quaternion.Inverse(rawQuat);
        GameObject go = Instantiate(particle);
        GameObject.Destroy(go, 1f);
    }

/* 
    // AssetBundleRequest/Plugins/BluetoothDeviceScript.cs 
    // から送られてくるbyte文字を受け取る部分
    public void GetMsg (string message)
    {
        msgText.text = "ESP32から送られてきた値:" + message;
        //文字列を数字に変換
        MoveValue = Int32.Parse(message);
        transform.Rotate(new Vector3(0, MoveValue/30, 0));

        if (MoveValue == 250)
            {
                SceneManager.LoadScene("04_GameScene");
            }
        
    }
 */

	void Reset ()
	{
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_deviceAddress = null;
		_foundSubscribeID = false;
		_foundWriteID = false;
		_dataBytes = null;
		_rssi = 0;
	}

	void SetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

    void setStateText(string text)
    {
        if (stateText == null) return;
        msgText.text = text;
    }

	void StartProcess ()
	{
		Reset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			
			SetState (States.Scan, 0.1f);
			
		}, (error) => {
			
			BluetoothLEHardwareInterface.Log ("Error during initialize: " + error);
		});
	}

    public AnimalInfo AnimalInfo;
    public GameObject moveCamera;
    private Transform cameraTransform;
    private float time = 0;

	// Use this for initialization
	void Start ()
	{
		StartProcess ();
        //cubeTransform = cube.GetComponent<Transform>();
        quatLookAtCam = Quaternion.identity;

        this.cameraTransform = this.moveCamera.transform;
	}

    void EvaluateBLEStates()
    {
        switch (_state)
        {
            case States.None:
                break;

            case States.Scan:
                BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
                    DeviceNameFoundedText.text += name+"\n";

                    // if your device does not advertise the rssi and manufacturer specific data
                    // then you must use this callback because the next callback only gets called
                    // if you have manufacturer specific data

                    if (!_rssiOnly)
                    {
                        if (name.Contains(DeviceName))
                        {
                            BluetoothLEHardwareInterface.StopScan();

                            // found a device with the name we want
                            // this example does not deal with finding more than one
                            _deviceAddress = address;

                            //BluetoothDeviceScriptにある、グローバル変数に情報を格納。
                            BluetoothDeviceScript.DeviceAddress = address;
                            BluetoothDeviceScript.DeviceName = DeviceName;
                            BluetoothDeviceScript.ServiceUUID = ServiceUUID;
                            BluetoothDeviceScript.SubscribeCharacteristic = SubscribeCharacteristic;
                            BluetoothDeviceScript.WriteCharacteristic = WriteCharacteristic;


                            SetState(States.Connect, 0.5f);
                        }
                    }

                }, (address, name, rssi, bytes) => {

                    // use this one if the device responses with manufacturer specific data and the rssi

                    if (name.Contains(DeviceName))
                    {
                        if (_rssiOnly)
                        {
                            _rssi = rssi;
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.StopScan();

                            // found a device with the name we want
                            // this example does not deal with finding more than one
                            _deviceAddress = address;
                            
                            //BluetoothDeviceScriptにある、グローバル変数に情報を格納。
                            BluetoothDeviceScript.DeviceAddress = address;
                            BluetoothDeviceScript.DeviceName = DeviceName;
                            BluetoothDeviceScript.ServiceUUID = ServiceUUID;
                            BluetoothDeviceScript.SubscribeCharacteristic = SubscribeCharacteristic;
                            BluetoothDeviceScript.WriteCharacteristic = WriteCharacteristic;

                            SetState(States.Connect, 0.5f);
                        }
                    }

                }, _rssiOnly); // this last setting allows RFduino to send RSSI without having manufacturer data

                if (_rssiOnly)
                    SetState(States.ScanRSSI, 0.5f);
                break;

            case States.ScanRSSI:
                break;

            case States.Connect:
                // set these flags
                _foundSubscribeID = false;
                _foundWriteID = false;

                // note that the first parameter is the address, not the name. I have not fixed this because
                // of backwards compatiblity.
                // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {

                    if (IsEqual(serviceUUID, ServiceUUID))
                    {
                        _foundSubscribeID = _foundSubscribeID || IsEqual(characteristicUUID, SubscribeCharacteristic);
                        _foundWriteID = _foundWriteID || IsEqual(characteristicUUID, WriteCharacteristic);

                        // if we have found both characteristics that we are waiting for
                        // set the state. make sure there is enough timeout that if the
                        // device is still enumerating other characteristics it finishes
                        // before we try to subscribe
                        if (_foundSubscribeID)// && _foundWriteID)
                        {
                            _connected = true;
                            SetState(States.Subscribe, 2f);
                        }
                    }
                });
                _state = States.None;
                break;

            case States.Subscribe:
                //setStateText("byteまだきてないよ:No" + CntX.ToString());
                //setStateText("byte length: " + _dataBytes.Length);
                //CntX += 1;



                //ここはぐるぐる回ってないっぽい　1回のみ？
                BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress
                (_deviceAddress, ServiceUUID, SubscribeCharacteristic, null, 
                (address, characteristicUUID, bytes) => 
                {

                    setStateText("byteきたー！！");　//こない。
                    // we don't have a great way to set the state other than waiting until we actually got
                    // some data back. For this demo with the rfduino that means pressing the button
                    // on the rfduino at least once before the GUI will update.
                    _state = States.None;

                    

                    // we received some data from the device
                    _dataBytes = bytes;
                    float qx = System.BitConverter.ToSingle(bytes, 0);
                    float qy = System.BitConverter.ToSingle(bytes, 8);
                    float qz = System.BitConverter.ToSingle(bytes, 4);
                    float qw = System.BitConverter.ToSingle(bytes, 12)*-1;

                    rawQuat = new Quaternion(qx, qy, qz, qw);
                    cubeTransform.rotation = quatLookAtCam*rawQuat;

                    DeviceNameFoundedText.text = qx.ToString() + " " + qy.ToString() + " " + qz.ToString() + " " + qw.ToString()+"\n";
                    DeviceNameFoundedText.text = "raw : "+rawQuat.ToString()+"\n";
                    DeviceNameFoundedText.text += "cube : "+cubeTransform.rotation.ToString();

                    //string str = System.Text.Encoding.ASCII.GetString(_dataBytes);
                    //stateText.text = str;

                });

                break;

            case States.Unsubscribe:
                BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, SubscribeCharacteristic, null);
                SetState(States.Disconnect, 4f);
                break;

            case States.Disconnect:
                if (_connected)
                {
                    BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) => {
                        BluetoothLEHardwareInterface.DeInitialize(() => {

                            _connected = false;
                            _state = States.None;
                        });
                    });
                }
                else
                {
                    BluetoothLEHardwareInterface.DeInitialize(() => {

                        _state = States.None;
                    });
                }
                break;
        }
    }
	// Update is called once per frame
	void Update ()
	{
        this.time += Time.deltaTime;

        float moveRate = Time.deltaTime*3;

        float x_pos = this.cameraTransform.position.x;
        //Debug.Log(this.cameraTransform.position.x);
        if(x_pos > 20){
            x_pos = 0;
        }
        

        //カメラを移動
        this.cameraTransform.position = new Vector3(x_pos + moveRate, this.cameraTransform.position.y, this.cameraTransform.position.z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            resetQuat();
        }

        

        //値を送信 -> 1
        //SendByte ((byte)0x01);

        //_____________________ GetValue ______________________
        //BluetoothDeviceScriptにある、getHistrを呼び出す。呼び出すと値が返ってくる。
        string HiValue = BluetoothDeviceScript.getHistr ();
        Debug.Log(HiValue);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            resetQuat();
        }

        // テキスト表示
        stateText.text = _state.ToString(); 

        //値を送信 -> 1
        //SendByte ((byte)0x01);

        if (HiValue != "")
        {
            //setStateTextを使い、文字を画面上に表示
            setStateText(HiValue + "%");

            //テキストを数値に変換して代入
            MoveValue = Int32.Parse(HiValue);
            

            //値が100になったらシーンを移る
            if (MoveValue == 100 && this.time > 2)
            {
                SceneManager.LoadScene("BLE_Play");
            }

        }
        //______________________________________________________
        
		if (_timeout > 0f)
		{
			_timeout -= Time.deltaTime;
			if (_timeout <= 0f)
			{
				_timeout = 0f;

                EvaluateBLEStates();
			}
		}
	}

	private bool ledON = false;
	public void OnLED ()
	{
		ledON = !ledON;
		if (ledON)
		{
			SendByte ((byte)0x01);
		}
		else
		{
			SendByte ((byte)0x00);
		}
	}
	
	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
	}
	
	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);
		
		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}
	
	void SendByte (byte value)
	{
		byte[] data = new byte[] { value };
		BluetoothLEHardwareInterface.WriteCharacteristic (_deviceAddress, ServiceUUID, WriteCharacteristic, data, data.Length, true, (characteristicUUID) => {
			
			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
	}


    private void Log(string format, params object[] args)    
    {        
        mLogs.Add(string.Format(format, args));
        if (mLogs.Count > 12)
                {            
                    mLogs.RemoveAt(0);
                }
        var builder = new StringBuilder();
        foreach (var log in mLogs)
        {
            builder.AppendLine(log);
        }
        stateText.text = builder.ToString();
    }


	
}
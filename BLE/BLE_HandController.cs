using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;

public class BLE_HandController : MonoBehaviour
{
    private Vector3 dragPos;
    RotateCamera_HS RotateCamera_HS_script;
    AnimalController_HS AnimalController_HS_script;
    BLE_CareMode BLE_CareMode_script;

    // BLE 値送信用変数_________________________________________________________
	public string ServiceUUID = "";
	public string WriteCharacteristic = "";
    private string _deviceAddress;
    // _______________________________________________________________________



    // Start is called before the first frame update
    void Start()
    {
        RotateCamera_HS_script = GameObject.Find("Main Camera").GetComponent<RotateCamera_HS>();
        AnimalController_HS_script = GameObject.Find("AnimationManager").GetComponent<AnimalController_HS>();
        BLE_CareMode_script = GameObject.Find("CareSystem").GetComponent<BLE_CareMode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandPointDown(){
        //RotateCamera_HSのスクリプトを無効にしてカメラが移動するのを止める
        RotateCamera_HS_script.enabled = false;
    }
    public void HandPointUp(){
        //RotateCamera_HSのスクリプトを有効にしてカメラが移動するようにする
        RotateCamera_HS_script.enabled = true;
        AnimalController_HS_script.Eye_StopAnimation();
        
    }

    public void HandDrag(){
        //ドラッグ位置の座標を取得しての座標をその座標にする
        this.dragPos = Input.mousePosition;
        this.transform.position = this.dragPos;
        //Debug.Log(this.dragPos);

        if(this.dragPos.x > 350 && this.dragPos.x < 860 && this.dragPos.y > 660 && this.dragPos.y < 1260){
            //実機へ送信
            SendByte ((byte)20);

            AnimalController_HS_script.Eye_HappyAnimation();
            BLE_CareMode_script.strokeMoodUp();
        }
        else{
            AnimalController_HS_script.Eye_StopAnimation();
        }

        
    }

    // BLE 値送信用_________________________________________________________
    void SendByte (byte value){
		byte[] data = new byte[] { value };

        //BluetoothDeviceScriptにあるグローバル変数から情報を取得。
        ServiceUUID = BluetoothDeviceScript.ServiceUUID;
        WriteCharacteristic = BluetoothDeviceScript.WriteCharacteristic;
        _deviceAddress = BluetoothDeviceScript.DeviceAddress;

        //値を送信
		BluetoothLEHardwareInterface.WriteCharacteristic (_deviceAddress, ServiceUUID, WriteCharacteristic, data, data.Length, true, (characteristicUUID) => {
			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
	}
    // ____________________________________________________________________
}

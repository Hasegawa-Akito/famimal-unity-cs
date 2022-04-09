using System;
using System.Collections;
using System.Security.Cryptography;
//Dictionaryを使うのに必要
using System.Collections.Generic;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BluetoothPlay_BP : MonoBehaviour
{
    public AnimalInfo AnimalInfo;

    Animator AnimationAnimal;

    public GameObject valueText;

    // BLE 値送信用変数_________________________________________________________
	public string ServiceUUID = "";
	public string WriteCharacteristic = "";
    private string _deviceAddress;
    // _______________________________________________________________________

    Dictionary<string, string> eyeTriggers = new Dictionary<string, string>(){
        {"0", "Not Trigger"},
        {"1", "Cry Trigger" },
        {"2", "Happy Trigger" },
        {"3", "Spin Trigger" },
    };
    
    float progressTime = 3.0f;
    float spanTime = 2.0f;
    private int moodValue;

    // Start is called before the first frame update
    void Start()
    {
        this.valueText = GameObject.Find("Text");
        this.AnimationAnimal = GameObject.Find("Pig_LOD0").GetComponent<Animator>();

        //ユーザー情報から機嫌度合いを取得
        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
        this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);
        this.moodValue = this.AnimalInfo.Show_moodInt();
    }

    // Update is called once per frame
    void Update()
    {
        //Bluetoothからの値を取得
        string HiValue = BluetoothDeviceScript.getHistr ();
        this.valueText.GetComponent<Text>().text = HiValue;
        Debug.Log(HiValue);
        
        this.progressTime += Time.deltaTime;

        if(Mathf.FloorToInt(this.progressTime) % 3 == 0){
            SendByte ((byte)this.moodValue);
        }

        
    }

    void EyeMove(string eyeTrigger){
        AnimationAnimal.SetTrigger(eyeTrigger);
        AnimationAnimal.SetTrigger("State Trigger");
        AnimationAnimal.SetTrigger("Fly Trigger");
        AnimationAnimal.SetTrigger("State Trigger");



        this.progressTime = 0;

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

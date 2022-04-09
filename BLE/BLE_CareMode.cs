using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;


public class BLE_CareMode : MonoBehaviour
{
    public AnimalInfo AnimalInfo;

    public GameObject MenuPanel;
    public GameObject MenuButton;
    public GameObject HandImage;
    public GameObject SmileImage;
    public GameObject CryImage;
    public GameObject HappyImage;
    public GameObject HangerImage;
    public GameObject CommandPanel;

    public GameObject ote;
    public GameObject osuwari;
    public GameObject okawari;
    public GameObject huse;
    public GameObject aruku;
    GameObject[] orderBoxs;

    GameObject[] MoodImages;


    private Vector3 firstHandPos;

    private int menuClickNum = 0;
    private int strokeClickNum = 0;
    private int randomNum;
    private int moodImageIndex;

    private string[] foodKinds = {"Apple", "Ham", "Onigiri", "Cake"};
    //GameObjectの配列を作成
    private GameObject[] FoodObject = new GameObject[4];

    AnimalController_HS AnimalController_HS_script;
    
    public bool hanger;

    // BLE 値送信用変数_________________________________________________________
	public string ServiceUUID = "";
	public string WriteCharacteristic = "";
    private string _deviceAddress;
    // _______________________________________________________________________


    // Start is called before the first frame update
    void Start()
    {
        orderBoxs = new GameObject[] { ote, osuwari, osuwari, okawari, huse, huse, aruku};
        MoodImages = new GameObject[] { CryImage, SmileImage, HappyImage };

        //ユーザー情報から機嫌イメージの名前と空腹かどうかを取得
        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
        this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);
        this.hanger = this.AnimalInfo.Show_hangerValue();
        this.moodImageIndex = this.AnimalInfo.Show_moodInt();
        //Debug.Log(this.AnimalInfo.orderNums[0]);


        //機嫌イメージを表示
        MoodImages[this.moodImageIndex].SetActive (true);
        
        //空腹マークを表示または非表示
        this.HangerImage.SetActive (this.hanger);

        
        this.MenuPanel.SetActive (false);

        this.firstHandPos = this.HandImage.transform.position;
        this.HandImage.SetActive (false);

        //食べ物のオブジェクトを配列に入れ、食べ物を全て画面から隠す
        int i = 0;
        foreach (string foodKind in foodKinds)
        {
            this.FoodObject[i] = GameObject.Find(foodKind);
            this.FoodObject[i].SetActive (false);
            i += 1;
        }

        AnimalController_HS_script = GameObject.Find("AnimationManager").GetComponent<AnimalController_HS>();

    }

    void Update()
    {
        //Bluetoothからの値を取得
        string HiValue = BluetoothDeviceScript.getHistr ();
        //Debug.Log(HiValue);

    }

    //散歩ボタンが押された時散歩画面へ遷移
    public void StrollScene(){
        //実機へ送信
        SendByte ((byte)30);

        BackScene.SaveSceneName("BLE_Play");
        SceneManager.LoadScene("StrollScene");
    }

    // //ステータスボタンが押された時
    // public void OnStatusButton (){
    //     SceneManager.LoadScene("ShowStatus");
    // }

    //メニューボタンが押された時
    public void OnMenuButton(){
        this.menuClickNum += 1;

        if(this.menuClickNum%2 == 1){
            this.MenuPanel.SetActive (true);
        }
        else{
            this.MenuPanel.SetActive (false);
        }
    }

    //撫でるボタンが押された時
    public void OnStrokeButton (){
    
        
        this.strokeClickNum += 1;

        if(this.strokeClickNum%2 == 1){
            //元の位置に手を表示させる
            this.HandImage.transform.position = this.firstHandPos;
            this.HandImage.SetActive (true);

            this.menuClickNum += 1;
            this.MenuPanel.SetActive (false);
        }
        else{
            this.HandImage.SetActive (false);
            //オブジェクトをjson形式にしてデータ保存
            string json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
            PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
            PlayerPrefs.Save();
        }
    }

    //カスタムボタンが押された時
    public void OnMoveCustomButton (){
        BackScene.SaveSceneName("BLE_Play");
        SceneManager.LoadScene("MoveCustom");
    }

    public void OnCommandButton(){
        this.CommandPanel.SetActive (true);
        int j = 1;
        foreach (bool orderNum in this.AnimalInfo.orderNums)
        {
            if(orderNum){
                orderBoxs[j-1].SetActive (true);
            }
            j += 1;
        }
    }
    public void OnCloseButton(){
        this.CommandPanel.SetActive (false);
    }

    public void OnOteButton(){
        //実機へ送信
        SendByte ((byte)40);
    }
    public void OnOsuwariButton(){
        //実機へ送信
        SendByte ((byte)50);
    }
    public void OnOkawariButton(){
        //実機へ送信
        SendByte ((byte)60);
    }
    public void OnhuseButton(){
        //実機へ送信
        SendByte ((byte)70);
    }
    public void OnArukuButton(){
        //実機へ送信
        SendByte ((byte)80);
    }
    

    //ご飯ボタンが押された時
    public void OnFoodButton(){

        //実機へ送信
        SendByte ((byte)10);

        //menuボタン,パネルを隠す
        this.MenuButton.SetActive (false);
        this.menuClickNum += 1;
        this.MenuPanel.SetActive (false);

        //目を普通の目に変える
        AnimalController_HS_script.Eye_StopAnimation();

        //HandImageが出ている場合は消す
        if(this.strokeClickNum%2 == 1){
            this.HandImage.SetActive (false);
            this.strokeClickNum += 1;
            
            //オブジェクトをjson形式にしてデータ保存
            string json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
            PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
            PlayerPrefs.Save();
        }

        //ランダムで食べ物の種類を選ぶ
        System.Random random = new System.Random();
        this.randomNum = random.Next(0, 4);
        this.FoodObject[this.randomNum].SetActive (true);
        AnimalController_HS_script.EatControl();

        //1.4秒後に食べ終わり処理実行
        Invoke(nameof(finishEat), 1.4f);

        //空腹状態でご飯をあげた時
        if(this.hanger){

            //機嫌を良くする
            this.AnimalInfo.Change_plus5_moodValue();
            //機嫌イメージを再表示
            MoodImages[this.moodImageIndex].SetActive (false);
            this.moodImageIndex = this.AnimalInfo.Show_moodInt();
            MoodImages[this.moodImageIndex].SetActive (true);
            

            //空腹状態じゃなくする
            this.hanger = false;
            this.AnimalInfo.Change_hangerValue(this.hanger);
            HangerImage.SetActive (false);
            //ご飯を上げた時間をいれる
            this.AnimalInfo.Change_lastMealTime();
            //Debug.Log(this.AnimalInfo.lastMealTime);

            //オブジェクトをjson形式にしてデータ保存
            string json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
            PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
            PlayerPrefs.Save();
        }
        
        
        
        
    }

    //食べ終わった時の処理
    void finishEat(){
        this.FoodObject[this.randomNum].SetActive (false);
       
        AnimalController_HS_script.JumpControl();

        //menuButtonを再表示
        this.MenuButton.SetActive (true);
    }

    public void strokeMoodUp(){
        this.AnimalInfo.Change_plusMini_moodValue();
        //機嫌イメージを再表示
        MoodImages[this.moodImageIndex].SetActive (false);
        this.moodImageIndex = this.AnimalInfo.Show_moodInt();
        MoodImages[this.moodImageIndex].SetActive (true);
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

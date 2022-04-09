using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;



public class StrollAnimalController_SS : MonoBehaviour
{
    Animator AnimationAnimal;

    private GameObject moveAnimal;
    private GameObject moveCamera;

    private Transform animalTransform;
    private Transform cameraTransform;

    private CharacterController animalController;

    private float walkSpeedRate = 0.05f;
    private float rotateSpeedRate = 0.5f;
    private float intervalDistance = 3f;

    public AnimalInfo AnimalInfo;

    //AnimalKindのスクリプトからobjectKindsをひっぱってくる
    string[] objectKinds = new AnimalKind().Show_objectKinds();
    
    private GameObject[] AnimationObjects;

    private float first_handPosition;
    private float handPosition;

    // Start is called before the first frame update
    void Start()
    {
        //ユーザー情報から動物のオブジェクト名を取得
        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
        this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);  
        //string myObjectKind = PlayerPrefs.GetString("objectKind");
        string myObjectKind = this.AnimalInfo.Show_objectKind();

        //配列から対象の動物のインデックスを調べる
        int animalIndex = Array.IndexOf(objectKinds, myObjectKind);

        this.AnimationObjects = new GameObject[objectKinds.Length];
        //動物のオブジェクトを配列に入れ、動物を全て画面から隠す
        int i = 0;
        foreach (string objectKind in objectKinds)
        {
            this.AnimationObjects[i] = GameObject.Find(objectKind);
            this.AnimationObjects[i].SetActive (false);
            i += 1;
        }

        //対象の動物を表示させる
        this.AnimationObjects[animalIndex].SetActive (true);

        this.moveAnimal = this.AnimationObjects[animalIndex];

        //this.moveAnimal = GameObject.Find("Pig_LOD0");
        this.moveCamera = GameObject.Find("Main Camera");

        this.AnimationAnimal = this.moveAnimal.GetComponent<Animator>();

        this.animalController = this.moveAnimal.GetComponent<CharacterController>();

        this.animalTransform = this.moveAnimal.transform;
        this.cameraTransform = this.moveCamera.transform;

        //カメラの位置初期設定
        this.cameraTransform.position = new Vector3(this.animalTransform.position.x, this.animalTransform.position.y+1f, this.animalTransform.position.z)-this.animalTransform.forward*this.intervalDistance;

    }

    // Update is called once per frame
    void Update()
    {

        //ジョイスティックからの値を取得
        float vertical = CrossPlatformInputManager.GetAxis( "Vertical" );
        float horizontal = CrossPlatformInputManager.GetAxis( "Horizontal" );

        // Debug.Log(vertical);
        // Debug.Log(horizontal);
        if(Mathf.Abs(vertical) > 0 || Mathf.Abs(horizontal) > 0){
            WalkControl();
            moveObject(vertical, horizontal);
            
        }
        else{

            //画面を触った時の座標を取得
            if(Input.GetMouseButtonDown(0)){
                this.first_handPosition = Input.mousePosition.x;
            }
            //画面に触れている時のその座標を取得
            else if(Input.GetMouseButton(0)){
                this.handPosition = Input.mousePosition.x;
                //Debug.Log(this.handPosition-this.first_handPosition);
                float distance = this.handPosition-this.first_handPosition;

                //動物を回転
                rotateObject(distance);
                WalkControl();
                
            }
            else{
                EmptyControll();
            }
        }
         
    }

    public void WalkControl(){
        this.AnimationAnimal.Play("Walk");

    }

    public void EmptyControll(){
        this.AnimationAnimal.Play("EmptyState");

    }

    //動物とカメラを動かす関数
    void moveObject(float vertical, float horizontal){
        vertical = vertical + 1f;

        //動物を前進
        this.animalController.Move(this.animalTransform.forward * vertical * this.walkSpeedRate);

        //動物の方向を変更
        Vector3 animalLocalAngle = this.animalTransform.localEulerAngles;
        animalLocalAngle.y += horizontal*this.rotateSpeedRate;
        this.animalTransform.localEulerAngles = animalLocalAngle;

        //カメラを前進
        this.cameraTransform.position = new Vector3(this.animalTransform.position.x, this.animalTransform.position.y+1f, this.animalTransform.position.z)-this.animalTransform.forward*this.intervalDistance;

        //動物を中心にカメラを回転
        Vector3 nowAnimalPos = this.animalTransform.position;
        this.cameraTransform.RotateAround(nowAnimalPos, Vector3.up, horizontal*this.rotateSpeedRate);


    }

    //スクロールで動物とカメラを回転
    void rotateObject(float scrollDistance){
        //動物の方向を変更
        Vector3 animalLocalAngle = this.animalTransform.localEulerAngles;
        animalLocalAngle.y += scrollDistance*this.rotateSpeedRate / 100;
        this.animalTransform.localEulerAngles = animalLocalAngle;

        //動物を中心にカメラを回転
        Vector3 nowAnimalPos = this.animalTransform.position;
        this.cameraTransform.RotateAround(nowAnimalPos, Vector3.up, scrollDistance*this.rotateSpeedRate / 100);
    }

    
}

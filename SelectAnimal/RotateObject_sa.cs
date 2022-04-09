using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class RotateObject_sa : MonoBehaviour
{
    public GameObject rotateAnimal;


    private float rotateSpeedRate = 0.01f;

    private float first_handPosition;
    private float handPosition;

    // Start is called before the first frame update
    void Start()
    {
        //動物選択画面の最初の動物をセット
        this.rotateAnimal = GameObject.Find("Cat_LOD0");
        
        
    }

    // Update is called once per frame
    void Update()
    {   

        //画面を触った時の座標を取得
        if(Input.GetMouseButtonDown(0)){
            this.first_handPosition = Input.mousePosition.x;
        }
        //画面に触れている時のその座標を取得
        else if(Input.GetMouseButton(0)){
            this.handPosition = Input.mousePosition.x;
            //Debug.Log(this.handPosition-this.first_handPosition);
            float distance = this.handPosition-this.first_handPosition;

            this.rotateAnimal.transform.Rotate(new Vector3(0, -distance * rotateSpeedRate, 0), Space.World);
            
        }

        // this.rotateAnimal.transform.Rotate(new Vector3(0, -horizontal * rotateSpeedRate, 0), Space.World);

        
    }

    //回転対称をセットする
    public void RotateAnimalSet(string objectKind){

        this.rotateAnimal = GameObject.Find(objectKind);

    }

    //オブジェクトの回転をリセットする
    public void RotateAnimalReset(){
        
        //初期値角度を(0, 180, 0)としている
        this.rotateAnimal.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    }
}

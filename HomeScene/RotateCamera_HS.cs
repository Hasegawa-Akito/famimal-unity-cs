using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCamera_HS : MonoBehaviour
{
    private float first_handPosition;
    private float handPosition;
    private float rotateRate = 0.005f;

    public GameObject HomeAnimal;

    public AnimalInfo AnimalInfo;

    // Start is called before the first frame update
    void Start()
    {
        string myObjectKind = PlayerPrefs.GetString("objectKind");
        this.HomeAnimal = GameObject.Find(myObjectKind);
    }

    // Update is called once per frame
    void Update()
    {
        //画面を触った時の座標を取得
        if(Input.GetMouseButtonDown(0)){
            this.first_handPosition = Input.mousePosition.x;
        }
        //画面に触れている時のその座標を取得
        if(Input.GetMouseButton(0)){
            this.handPosition = Input.mousePosition.x;
            //Debug.Log(this.handPosition-this.first_handPosition);
            float distance = this.handPosition-this.first_handPosition;

            //動物を中心にカメラを回転
            Vector3 homeAnimalPos = this.HomeAnimal.transform.position;
            transform.RotateAround(homeAnimalPos, Vector3.up, distance*rotateRate);
        }
    }
}

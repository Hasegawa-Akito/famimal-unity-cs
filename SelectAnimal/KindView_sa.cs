using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KindView_sa : MonoBehaviour
{

    string[] animalKinds = new AnimalKind().Show_animalKinds();

    string[] objectKinds = new AnimalKind().Show_objectKinds();

    public string objectKind;
    public string nowAnimalKind;
    public int nowIndex;

    public GameObject mainCamera;
    public GameObject animalKindText;


    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = GameObject.Find("Main Camera");
        this.animalKindText = GameObject.Find("AnimalKind");
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void ShowAnimalKind(){
        //カメラの位置とanimalKindsの配列の番号を紐づける
        float cameraLocation_x = this.mainCamera.transform.position.x;
        this.nowIndex = (int)cameraLocation_x / 10;

        //現在の動物の種類を取得
        this.nowAnimalKind = this.animalKinds[this.nowIndex];
        //動物オブジェクトの名前を取得
        this.objectKind = this.objectKinds[this.nowIndex];

        //現在の動物の種類を表示
        this.animalKindText.GetComponent<Text>().text = this.nowAnimalKind;
    }
}

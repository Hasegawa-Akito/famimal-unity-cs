using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideAnimal_sa : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject PrevButton;
    public GameObject NextButton;

    KindView_sa KindView_sa_script;
    RotateObject_sa RotateObject_sa_script;

    public int animalLen;
    private int nowIndex;

    //AnimalKindのスクリプトからanimalKindsをひっぱってくる
    string[] animalKinds = new AnimalKind().Show_animalKinds();

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = GameObject.Find("Main Camera");
        this.PrevButton = GameObject.Find("Prev");
        this.NextButton = GameObject.Find("Next");

        //KindView_saスクリプトのメソッドを使えるようにする
        KindView_sa_script = GameObject.Find("KindView").GetComponent<KindView_sa>();
        KindView_sa_script.ShowAnimalKind();

        this.animalLen = animalKinds.Length;

        RotateObject_sa_script = GameObject.Find("RotateAnimal").GetComponent<RotateObject_sa>();

        //スタート時は左スライドボタンは非表示 表示動物が一匹の場合は右スライドボタンも非表示
        this.PrevButton.SetActive (true);
        this.NextButton.SetActive (true);
        if(this.nowIndex == 0){
            this.PrevButton.SetActive (false);
        }
        if(this.animalLen-1 == 0){
            this.NextButton.SetActive (false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RightSlide(){
        this.mainCamera.transform.Translate(10, 0, 0);

        //KindView_saのスクリプトのメソッド実行
        KindView_sa_script.ShowAnimalKind();

        //スライド前の動物オブジェクトを初期角度に戻す
        RotateObject_sa_script.RotateAnimalReset();

        //次に動物がいなければスライドボタンを非表示
        this.nowIndex = KindView_sa_script.nowIndex;
        if(this.nowIndex == this.animalLen-1){
            this.NextButton.SetActive (false);
        }
        this.PrevButton.SetActive (true);

        //スライド後の動物オブジェクトの名前を取得
        string objectKind = KindView_sa_script.objectKind;
        //回転対称にスライド後の動物オブジェクトの名前をセット
        RotateObject_sa_script.RotateAnimalSet(objectKind);
    }

    public void LeftSlide(){
        this.mainCamera.transform.Translate(-10, 0, 0);

        //KindView_saのスクリプトのメソッド実行
        KindView_sa_script.ShowAnimalKind();

        //スライド前の動物オブジェクトを初期角度に戻す
        RotateObject_sa_script.RotateAnimalReset();

        //次に動物がいなければスライドボタンを非表示
        this.nowIndex = KindView_sa_script.nowIndex;
        if(this.nowIndex == 0){
            this.PrevButton.SetActive (false);
        }
        this.NextButton.SetActive (true);

        //スライド後の動物オブジェクトの名前を取得
        string objectKind = KindView_sa_script.objectKind;
        //回転対称にスライド後の動物オブジェクトの名前をセット
        RotateObject_sa_script.RotateAnimalSet(objectKind);
    }
}

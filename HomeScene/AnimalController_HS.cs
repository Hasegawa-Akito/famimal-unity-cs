using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimalController_HS : MonoBehaviour
{
    private Animator AnimationAnimal;
    private GameObject HomeAnimal;

    public AnimalInfo AnimalInfo;

    //AnimalKindのスクリプトからobjectKindsをひっぱってくる
    string[] objectKinds = new AnimalKind().Show_objectKinds();
    
    private GameObject[] AnimationObjects;

    
    
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

        this.HomeAnimal = this.AnimationObjects[animalIndex];
        this.AnimationAnimal = this.HomeAnimal.GetComponent<Animator>();


        bool veryHanger = this.AnimalInfo.show_veryHanger();
        if(veryHanger){
            deathAnimation();
            Eye_DeadAnimation();
        }
        else{
            stateAnimation();
        }
              
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void JumpControl(){
        //目を普通の目に変える
        Eye_StopAnimation();

        this.AnimationAnimal.Play("Jump");
        Invoke(nameof(stateAnimation), 0.7f);
    }

    public void stateAnimation(){
        this.AnimationAnimal.Play("Idle_A");
    }

    public void EatControl(){
        this.AnimationAnimal.Play("Eat");
    }
    public void deathAnimation(){
       this.AnimationAnimal.Play("Death");
    }

    public void Eye_HappyAnimation(){
        this.AnimationAnimal.Play("Eyes_Happy");
    }

    public void Eye_StopAnimation(){
        this.AnimationAnimal.Play("Eyes_Blink");
    }

    public void Eye_DeadAnimation(){
        this.AnimationAnimal.Play("Eyes_Dead");
    }

}

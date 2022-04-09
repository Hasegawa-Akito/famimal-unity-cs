using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



//動物情報のインスタンス化用のクラス
[System.Serializable]
public class AnimalInfo
{
    public string animalName;
    public string myName;
    public string objectKind;
    public string animalKind;
    public float friendlyValue = 0;

    public float moodValue = 2;
    public bool hangerValue = true;

    public bool[] orderNums = {false, false, false, false, false, false, false};

    public DateTime lastMealTime = new DateTime(2022, 1, 1, 1, 0, 0, DateTimeKind.Local);
    public string lastMealTimeStr = (new DateTime(2022, 1, 1, 1, 0, 0, DateTimeKind.Local)).ToString();

    string[] moodImages = {"CryImage", "SmileImage", "HappyImage"};

    public void Choose_myName(string chosed_myName){
        this.myName = chosed_myName;
    }
    public void Choose_animalName(string chosed_animalName){
        this.animalName = chosed_animalName;
    }
    public void Choose_objectKind(string chosed_objectKind){
        this.objectKind = chosed_objectKind;
    }
    public void Choose_animalKind(string chosed_animalKind){
        this.animalKind = chosed_animalKind;
    }

    public void Change_lastMealTime(){
        this.lastMealTimeStr = DateTime.Now.ToString();
        this.lastMealTime = DateTime.Parse(this.lastMealTimeStr);;
    }

    public void Change_plus3_friendlyValue(){
        this.friendlyValue += 3;
    }

    public void Change_minus1_friendlyValue(){
        this.friendlyValue -= 1;
        if(this.friendlyValue < 0){
            this.friendlyValue = 0;
        }
        
    }

    public void Change_plus5_moodValue(){
        this.moodValue += 0.5f;
        if(this.moodValue > 2){
            this.moodValue = 2;
            Change_plus3_friendlyValue();
        }
        
    }
    public void Change_plus1_moodValue(){
        this.moodValue += 1;
        if(this.moodValue > 2){
            this.moodValue = 2;
            Change_plus3_friendlyValue();
        }
        
    }
    public void Change_plusMini_moodValue(){
        if(this.moodValue < 2){
            this.moodValue += 0.0007f;
            if(this.moodValue > 2){
                this.moodValue = 2;
                Change_plus3_friendlyValue();
            }
        }
        
    }
    public void Change_minus1_moodValue(){
        this.moodValue -= 1;
        if(this.moodValue < 0){
            this.moodValue = 0;
            Change_minus1_friendlyValue();
        }
        
    }
    public void Change_minus2_moodValue(){
        this.moodValue -= 2;
        if(this.moodValue < 0){
            this.moodValue = 0;
            Change_minus1_friendlyValue();
        }
        
    }

    public void Change_hangerValue(bool hunger){
        this.hangerValue = hunger;
    }

    public void InitialRegister(string chosed_myName, string chosed_animalName, string chosed_objectKind, string chosed_animalKind){
        Choose_myName(chosed_myName);
        Choose_animalName(chosed_animalName);
        Choose_objectKind(chosed_objectKind);
        Choose_animalKind(chosed_animalKind);
    }

    public string Show_myName(){
        return this.myName;
    }
    public string Show_animalName(){
        return this.animalName;
    }
    public string Show_objectKind(){
        return this.objectKind;
    }
    public string Show_animalKind(){
        return this.animalKind;
    }
    public float Show_friendlyValue(){
        return this.friendlyValue;
    }
    //機嫌度合いを表示
    public float Show_moodValue(){
        return this.moodValue;
    }
    //機嫌の種類を表示
    public string Show_moodImage(){
        return moodImages[Mathf.FloorToInt(this.moodValue)];
    }
    //機嫌の度合いを少数切り捨てでint型で表示
    public int Show_moodInt(){
        return Mathf.FloorToInt(this.moodValue);
    }

    public bool Show_hangerValue(){
        DateTime nowTime = DateTime.Now;
        this.lastMealTime = DateTime.Parse(this.lastMealTimeStr);
        int durationDay = (nowTime - this.lastMealTime).Days;
        int durationHour = (nowTime - this.lastMealTime).Hours;

        if(durationDay >= 1){
            Change_hangerValue(true);
            Change_minus2_moodValue();
            return this.hangerValue;
        }
        else if(durationHour >= 8){
            Change_hangerValue(true);

            if(durationHour >= 16){
                Change_minus2_moodValue();
            }
            else{
                Change_minus1_moodValue();
            }

            return this.hangerValue;
        }
        else{
            Change_hangerValue(false);
            return this.hangerValue;
        }
    }

    public bool show_veryHanger(){
        DateTime nowTime = DateTime.Now;
        this.lastMealTime = DateTime.Parse(this.lastMealTimeStr);
        int durationDay = (nowTime - this.lastMealTime).Days;
        int durationHour = (nowTime - this.lastMealTime).Hours;

        if(durationDay >= 1){
            return true;
        }
        else if(durationHour >= 16){
            return true;
        }
        else{
            return false;
        }
    }

}

public class AnimalInfoScript_AI : MonoBehaviour
{
    
}

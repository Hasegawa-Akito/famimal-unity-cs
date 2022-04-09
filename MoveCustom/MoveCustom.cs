using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveCustom : MonoBehaviour
{
    
    public GameObject  FRPanel;
    public GameObject  FLPanel;
    public GameObject  RLPanel;
    public GameObject  RRPanel;

    public GameObject Custom1;
    public GameObject Custom2;
    public GameObject Custom3;
    public GameObject Custom4;
    public GameObject Custom5;
    public GameObject Custom6;
    public GameObject Custom7;
    public GameObject Custom8;

    public GameObject OsuwariMov;
    public GameObject OteMov;
    public GameObject OkawariMov;
    public GameObject ArukuMov;
    public GameObject HuseMov;

    public GameObject Dropdown;
    public GameObject SamplePanel;

    Dictionary<string, GameObject> moveItemDict = new Dictionary<string, GameObject>();

    //ドロップダウン初期値
    private string selectedvalue = "おすわり";


    public GameObject CorrectImage;
    public GameObject ErrorImage;
    public GameObject AnswerPanel;
    public GameObject GoalText;

    GameObject[] customBoxs;

    private int customCount = 0;
    
    public int[] customNums = new int[8];
    CustomKind CustomKind = new CustomKind();

    // Start is called before the first frame update
    void Start()
    {
        customBoxs = new GameObject[] { Custom1, Custom2, Custom3, Custom4, Custom5, Custom6, Custom7, Custom8};
        moveItemDict.Add("おすわり", OsuwariMov);
        moveItemDict.Add("おて", OteMov);
        moveItemDict.Add("おかわり", OkawariMov);
        moveItemDict.Add("あるく", ArukuMov);
        moveItemDict.Add("ふせ", HuseMov);

    }


    public void OnFRButton(){
        this.FRPanel.SetActive (true);
    }
    public void OnFLButton(){
        this.FLPanel.SetActive (true);
    }
    public void OnRLButton(){
        this.RLPanel.SetActive (true);
    }
    public void OnRRButton(){
        this.RRPanel.SetActive (true);
    }
    public void OnCloseButton(){
        this.FRPanel.SetActive (false);
        this.FLPanel.SetActive (false);
        this.RLPanel.SetActive (false);
        this.RRPanel.SetActive (false);
        this.RRPanel.SetActive (false);
    }

    public void Custom(int customNumber){
        if(this.customCount == 8){
            this.customCount = 0;
        }

        customBoxs[this.customCount].SetActive (true);

        string customName = CustomKind.Show_customKinds(customNumber);
        customBoxs[this.customCount].transform.Find("MoveContent").gameObject.GetComponent<Text>().text = customName;
        
        this.customNums[this.customCount] = customNumber;
        //Debug.Log(this.customNums[this.customCount]);

        this.customCount += 1;
    }

    public void OnTry(){
        for(int i=0; i <= 7; i++){
            customBoxs[i].transform.Find("MoveContent").gameObject.GetComponent<Text>().text = "";
            customBoxs[i].SetActive (false);
        }
        this.customCount = 0;

        //Debug.Log(CustomKind.Confirm_custom(this.customNums));
        string goalName = CustomKind.Confirm_custom(this.customNums);
        if(string.IsNullOrEmpty(goalName)){
            this.AnswerPanel.SetActive (true);
            this.ErrorImage.SetActive (true);
        }
        else{
            this.AnswerPanel.SetActive (true);
            this.CorrectImage.SetActive (true);
            this.GoalText.SetActive (true);
            this.GoalText.GetComponent<Text>().text = goalName;

        }

        this.customNums = new int[8];
    }

    public void CloseAnswerButton(){
        this.ErrorImage.SetActive (false);
        this.CorrectImage.SetActive (false);
        this.AnswerPanel.SetActive (false);
        this.GoalText.SetActive (false);
    }

    public void AllDelete(){
        for(int i=0; i <= 7; i++){
            customBoxs[i].transform.Find("MoveContent").gameObject.GetComponent<Text>().text = "";
            customBoxs[i].SetActive (false);
        }
        this.customCount = 0;
        this.customNums = new int[8];
    }
    public void OneDelete(){
        if(this.customCount > 0){
            customBoxs[this.customCount-1].transform.Find("MoveContent").gameObject.GetComponent<Text>().text = "";
            customBoxs[this.customCount-1].SetActive (false);

            this.customCount -= 1;     

            this.customNums[this.customCount] = 0;
        }
        
    }

    public void ViewSample(){
        this.SamplePanel.SetActive(true);
        moveItemDict[this.selectedvalue].SetActive(true);
    }

    public void CloseButton(){
        this.SamplePanel.SetActive(false);
        moveItemDict[this.selectedvalue].SetActive(false);
    }

    public void PlayMov(){
        moveItemDict[this.selectedvalue].SetActive(false);

        //「Dropdown」というGameObjectのDropDownコンポーネントを操作するために取得
        Dropdown ddtmp = this.Dropdown.GetComponent<Dropdown>();
        this.selectedvalue = ddtmp.options[ddtmp.value].text;
        
        //Debug.Log(this.selectedvalue);

        //gameObjectの辞書配列からgameObjectを取り出し表示
        moveItemDict[this.selectedvalue].SetActive(true);
    }
}

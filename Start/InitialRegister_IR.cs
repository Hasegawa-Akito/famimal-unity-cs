using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InitialRegister_IR : MonoBehaviour
{
    
    public AnimalInfo AnimalInfo;

    public GameObject MyNameField;
    public GameObject AnimalNameField;
    public GameObject AttentionPanel;


    // Start is called before the first frame update
    void Start()
    {
        //MonoBehaviourのついてないクラスの参照仕方
        this.AnimalInfo = new AnimalInfo();
        //Debug.Log(this.AnimalInfo.Show_myName());

        this.MyNameField = GameObject.Find("MyNameField");
        this.AnimalNameField = GameObject.Find("AnimalNameField");
        this.AttentionPanel = GameObject.Find("AttentionPanel");

        this.AttentionPanel.SetActive (false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(){
        string MyNameText = this.MyNameField.GetComponent<InputField>().text;
        string AnimalNameText = this.AnimalNameField.GetComponent<InputField>().text;
        //Debug.Log(MyNameText);

        //自分の名前とペットの名前を入力しないと進めない
        if(string.IsNullOrEmpty(MyNameText) || string.IsNullOrEmpty(AnimalNameText)){
            this.AttentionPanel.SetActive (true);
        }
        else{
            this.AnimalInfo.Choose_myName(MyNameText);
            this.AnimalInfo.Choose_animalName(AnimalNameText);
            PlayerPrefs.SetString("myName", MyNameText);
            PlayerPrefs.SetString("animalName", AnimalNameText);
            PlayerPrefs.Save();

            BackScene.SaveSceneName("InitialRegistration");
            SceneManager.LoadScene("SelectAnimal");
        }
        
    }

    //改行された時に無効にする
    public void OnMyNameChanged()
    {
        string value = this.MyNameField.GetComponent<InputField>().text;
        if (value.IndexOf("\n") != -1)
        {
            this.MyNameField.GetComponent<InputField>().text = value.Replace("\n", "");;
        }
    }

    public void OnAnimalNameChanged()
    {
        string value = this.AnimalNameField.GetComponent<InputField>().text;
        if (value.IndexOf("\n") != -1)
        {
            this.AnimalNameField.GetComponent<InputField>().text = value.Replace("\n", "");;
        }
    }

    //警告を閉じる
    public void CloseAttention(){

        this.AttentionPanel.SetActive (false);

    }

    
}

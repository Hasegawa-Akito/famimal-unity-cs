using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowStatus_SS : MonoBehaviour
{
    public AnimalInfo AnimalInfo;

    public GameObject MyNameText;
    public GameObject AnimalNameText;
    public GameObject AnimalKindText;
    public GameObject FriendlyValueText;
    public GameObject NameSetPanel;
    public GameObject MyNameField;
    public GameObject AnimalNameField;
    public GameObject Attention;



    // Start is called before the first frame update
    void Start()
    {
        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
        this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);

        this.MyNameText.GetComponent<Text>().text = this.AnimalInfo.Show_myName();
        this.AnimalNameText.GetComponent<Text>().text = this.AnimalInfo.Show_animalName();
        this.AnimalKindText.GetComponent<Text>().text = this.AnimalInfo.Show_animalKind();
        this.FriendlyValueText.GetComponent<Text>().text = this.AnimalInfo.Show_friendlyValue().ToString();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseButton(){
        SceneManager.LoadScene("HomeScene");
    }

    public void EditAnimalKind(){
        BackScene.SaveSceneName("ShowStatus");
        SceneManager.LoadScene("SelectAnimal");
    }
    public void EditName(){
        this.NameSetPanel.SetActive (true);
        this.MyNameField.GetComponent<InputField>().text = this.AnimalInfo.Show_myName();
        this.AnimalNameField.GetComponent<InputField>().text = this.AnimalInfo.Show_animalName();
    }
    public void OnBackButton(){
        this.NameSetPanel.SetActive (false);
    }
    public void OnSaveButton(){
        string MyNameText = this.MyNameField.GetComponent<InputField>().text;
        string AnimalNameText = this.AnimalNameField.GetComponent<InputField>().text;

        if(string.IsNullOrEmpty(MyNameText) || string.IsNullOrEmpty(AnimalNameText)){
            this.Attention.SetActive (true);
        }
        else{
            this.AnimalInfo.Choose_myName(MyNameText);
            this.AnimalInfo.Choose_animalName(AnimalNameText);

            //オブジェクトをjson形式にしてデータ保存
            string json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
            PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
            PlayerPrefs.Save();

            //ステータス表示画面の名前を更新
            this.MyNameText.GetComponent<Text>().text = MyNameText;
            this.AnimalNameText.GetComponent<Text>().text = AnimalNameText;

            this.NameSetPanel.SetActive (false);
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

    public void CloseAttention(){
        this.Attention.SetActive (false);
    }
}

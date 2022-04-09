using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectAnimal_SA : MonoBehaviour
{
    KindView_sa KindView_sa_script;

    public AnimalInfo AnimalInfo;

    // Start is called before the first frame update
    void Start()
    {
        KindView_sa_script = GameObject.Find("KindView").GetComponent<KindView_sa>();

        //MonoBehaviourのついてないクラスの参照仕方
        this.AnimalInfo = new AnimalInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimalSelect(){
        string objectKind = KindView_sa_script.objectKind;
        string nowAnimalKind = KindView_sa_script.nowAnimalKind;
        PlayerPrefs.SetString("objectKind", objectKind);
        PlayerPrefs.SetString("animalKind", nowAnimalKind);


        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
        //初期設定時かどうか判断
        if(string.IsNullOrEmpty(json_AnimalInfo)){
            string myName = PlayerPrefs.GetString("myName");
            string animalName = PlayerPrefs.GetString("animalName");

            this.AnimalInfo.InitialRegister(myName, animalName, objectKind, nowAnimalKind);
        }
        else{
            this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);
            this.AnimalInfo.Choose_objectKind(objectKind);
            this.AnimalInfo.Choose_animalKind(nowAnimalKind);
        }

        

        //オブジェクトをjson形式にしてデータ保存
        json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
        PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("HomeScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer_SS : MonoBehaviour
{
    private GameObject TimerProgress;
    private Image circleImage;

    public AnimalInfo AnimalInfo;

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.TimerProgress = GameObject.Find("TimerProgress");
        this.circleImage = this.TimerProgress.GetComponent<Image>();
        this.circleImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.time = Time.deltaTime;
        float amount = this.time / 70;
        this.circleImage.fillAmount += amount;

        //Debug.Log(this.circleImage.fillAmount);
        if(this.circleImage.fillAmount >= 1){
            //ユーザー情報を取得
            string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
            this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);

            this.AnimalInfo.Change_plus5_moodValue();

            //オブジェクトをjson形式にしてデータ保存
            json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
            PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
            PlayerPrefs.Save();

            SceneManager.LoadScene("HomeScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_ls : MonoBehaviour
{
    public AnimalInfo AnimalInfo;
    public GameObject moveCamera;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {   
        this.cameraTransform = this.moveCamera.transform;

        //三秒後に実行
        Invoke(nameof(LoadScene), 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;

        //カメラを移動
        this.cameraTransform.position = new Vector3(this.cameraTransform.position.x + time*2, this.cameraTransform.position.y, this.cameraTransform.position.z);
    }

    void LoadScene()
    {
        string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");

        //ユーザーデータが存在しなければ初期設定画面へ
        if(string.IsNullOrEmpty(json_AnimalInfo)){
            SceneManager.LoadScene("InitialRegistration");
        }
        else{
            SceneManager.LoadScene("HomeScene");
        }
        
    }
}

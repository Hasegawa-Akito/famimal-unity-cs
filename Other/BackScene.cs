using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//戻るボタンで前の画面に戻るシステム
public class BackScene : MonoBehaviour
{
    public static string PostSceneName;

    //シーン移動前にそのシーンの名前を保存しておく
    public static void SaveSceneName(string SceneName){

        PostSceneName = SceneName;

    }

    public void Back_Scene(){
        SceneManager.LoadScene(PostSceneName);
    }
}
 
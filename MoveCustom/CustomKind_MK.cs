using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//配列比較に必要
using System.Linq;

//カスタマイズの種類参照ようクラス
[System.Serializable]
public class CustomKind
{
    private Dictionary<int, string>  customKinds = new Dictionary<int, string>(){
        {1, "右前足 あげる"},
        {2, "右前足 さげる" },
        {7, "左前足 あげる" },
        {8, "左前足 さげる" },
        {13, "右後足 あげる"},
        {14, "右後足 さげる" },
        {19, "左後足 あげる" },
        {20, "左後足 さげる" },
    };

    private Dictionary<int, int[]>  goalNums = new Dictionary<int, int[]>(){
        //お手
        {1, new int[8]{1,0,0,0,0,0,0,0} },
        //お座り
        {2, new int[8]{13,19,0,0,0,0,0,0} },
        {3, new int[8]{19,13,0,0,0,0,0,0} },
        //おかわり
        {4, new int[8]{7,0,0,0,0,0,0,0} },
        //ふせ
        {5, new int[8]{1,7,0,0,0,0,0,0} },
        {6, new int[8]{7,1,0,0,0,0,0,0} },
        //歩く
        {7, new int[8]{1,8,20,14,2,7,19,13} },
    };

    private Dictionary<int, string>  goalNames = new Dictionary<int, string>(){
        {1, "おて" },
        {2, "おすわり" },
        {3, "おすわり" },
        {4, "おかわり" },
        {5, "ふせ" },
        {6, "ふせ" },
        {7, "あるく" },
    };


    private int[] a = new int[8]{1,0,0,0,0,0,0,0};

    public string Show_customKinds(int customNumber){
        return this.customKinds[customNumber];
    }
    
    public AnimalInfo AnimalInfo;
    public string Confirm_custom(int[] customNums){
        // Debug.Log(customNums[0]);
        for(int i=1; i <= this.goalNums.Count; i++){
            if(this.goalNums[i].SequenceEqual(customNums)){
                //Debug.Log(goalNames[i]);
                //ユーザー情報を取得
                string json_AnimalInfo = PlayerPrefs.GetString("json_AnimalInfo");
                this.AnimalInfo = JsonUtility.FromJson<AnimalInfo>(json_AnimalInfo);
                this.AnimalInfo.orderNums[i-1] = true;
                json_AnimalInfo = JsonUtility.ToJson(this.AnimalInfo);
                PlayerPrefs.SetString("json_AnimalInfo", json_AnimalInfo);
                PlayerPrefs.Save();

                return goalNames[i];
            }
        }
        return null;
    }

}

public class CustomKind_MK : MonoBehaviour
{
    
}

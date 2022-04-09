using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//動物の種類参照ようクラス
[System.Serializable]
public class AnimalKind
{
    //並べられている順番通りに配列を作成
    private string[] animalKinds = { "ねこ", "いぬ", "うさぎ", "かめ", "ねずみ", "チーター", "ゾウ",
    "ガゼル", "カバ", "ハイエナ", "サイ", "シマウマ", "きつね", "いのしし", "あらいぐま", "おおかみ", "バッファロー",
    "うし", "ロバ", "ブタ", "ひつじ", "トナカイ", "イイズナ"};

    private string[] objectKinds = { "Cat_LOD0", "Dog_LOD0", "Rabbit_LOD0", "Tortoise_LOD0", "Mouse_LOD0",
     "Cheetah_LOD0", "Elephant_LOD0","Gazelle_LOD0", "Hippo_LOD0", "Hyena_LOD0", "Rhino_LOD0", "Zebra_LOD0",
     "Fox_LOD0", "Hog_LOD0", "Raccoon_LOD0", "Wolf_LOD0", "Buffalo_LOD0", "Cow_LOD0", "Donkey_LOD0", "Pig_LOD0",
      "Sheep_LOD0", "Reindeer_LOD0", "SnowWeasel_LOD0"};

    public string[] Show_animalKinds(){
        return this.animalKinds;
    }
    public string[] Show_objectKinds(){
        return this.objectKinds;
    }

    

}

public class AnimalKind_AK : MonoBehaviour
{
    
}

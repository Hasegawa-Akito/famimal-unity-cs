using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController_HS : MonoBehaviour
{
    private Vector3 dragPos;
    RotateCamera_HS RotateCamera_HS_script;
    AnimalController_HS AnimalController_HS_script;
    CareMode_HS CareMode_HS_script;


    // Start is called before the first frame update
    void Start()
    {
        RotateCamera_HS_script = GameObject.Find("Main Camera").GetComponent<RotateCamera_HS>();
        AnimalController_HS_script = GameObject.Find("AnimationManager").GetComponent<AnimalController_HS>();
        CareMode_HS_script = GameObject.Find("CareSystem").GetComponent<CareMode_HS>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandPointDown(){
        //RotateCamera_HSのスクリプトを無効にしてカメラが移動するのを止める
        RotateCamera_HS_script.enabled = false;
    }
    public void HandPointUp(){
        //RotateCamera_HSのスクリプトを有効にしてカメラが移動するようにする
        RotateCamera_HS_script.enabled = true;
        AnimalController_HS_script.Eye_StopAnimation();
        
    }

    public void HandDrag(){
        //ドラッグ位置の座標を取得しての座標をその座標にする
        this.dragPos = Input.mousePosition;
        this.transform.position = this.dragPos;
        //Debug.Log(this.dragPos);

        if(this.dragPos.x > 350 && this.dragPos.x < 860 && this.dragPos.y > 660 && this.dragPos.y < 1260){
            AnimalController_HS_script.Eye_HappyAnimation();
            CareMode_HS_script.strokeMoodUp();
        }
        else{
            AnimalController_HS_script.Eye_StopAnimation();
        }

        

    }
}

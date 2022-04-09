using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    MoveCustom MoveCustom_script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCustom(){
        
        int number = int.Parse(this.transform.Find("Number").gameObject.GetComponent<Text>().text);
        //Debug.Log(number);

        MoveCustom_script = GameObject.Find("CustomSystem").GetComponent<MoveCustom>();
        MoveCustom_script.Custom(number);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    public void PressStart()
    {
        Debug.Log("Press Start");
        //ここに次のシーンに行かせる
        //
        if(!firstPush)
        {
            Debug.Log("次のs-んに行ったで");
            firstPush = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

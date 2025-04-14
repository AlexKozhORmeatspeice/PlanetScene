using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

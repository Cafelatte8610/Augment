using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPrint : MonoBehaviour
{
    public midi_analyz midi_analyz;
    private bool [,] KeyCode;
    // Start is called before the first frame update
    void Start()
    {
        KeyCode=midi_analyz.KeyCode;
    }
    

    private int i=0;
    private float CrTime;
    void Update()
    {
        CrTime += Time.deltaTime;
        if(CrTime > 0.1){
            for(int j=40;j<101;j++){
                if(KeyCode[j,i]){
                    GameObject Notu=(GameObject)Resources.Load("Key");
                    Instantiate (Notu, new Vector3((((j+1)-70)*5),150f,-1f), Quaternion.identity);
                }
            }
            CrTime = 0f;
            i++;
        }
    }
}

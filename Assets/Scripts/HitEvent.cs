using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    int Combo=0,bCombo=0;
    public MicSpectrumAnalyz MicSpectrumanalyz;
    bool[] key;

    // Start is called before the first frame update
    void Start()
    {
        // key=new bool[88];
        // for(int i=0;i<88;i++){
        //     key[i]=true;
        // }   
    }
    // Update is called once per frame
    List<int> Notekey=new List<int>();
    void Update()
    {
        Notekey.Clear();
        key=MicSpectrumanalyz.key_judg;
        if(Combo==0){

        }
        else{
            if(Combo!=bCombo){
                // ちょっとした演出?
            }
        }
        ListPrinter(Notekey);
    }

    public void GetKeyPos(int x){
        Notekey.Add(x);
        // print(x);
        // if(key[x]){
        //     // combo加算
        //     Combo++;
        // }
    }

    void ListPrinter(List<int> A){
        string s=" ";
        for(int i=0;i<A.Count;i++){
            s+=A[i].ToString();
            s+=" , ";
        }
        if(s != " ") print(s);
    }

    // void OnCollisionEnter2D(Collision2D collision) {
    //     if (collision.gameObject.GetComponent<KeyEvent>()) {
    //         int x=collision.gameObject.GetComponent<KeyEvent>().return_key();
    //         if(key[x]){
    //             // combo加算
    //             Combo++;
    //             Destroy(collision.gameObject);
    //         }
    //     }
    // }

    void OnCollisionExit2D(Collision2D collision){
        // combo終わり
        Combo=0;
        bCombo=0;
    }
}

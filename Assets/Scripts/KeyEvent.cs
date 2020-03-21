using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyEvent : MonoBehaviour
{

    public Vector3 tmp;
    bool fin=true;
    public GameObject HitEvent;

    // public MicSpectrumAnalyz MicSpectrumAnalyz;
    void Start(){
        tmp = this.gameObject.transform.position;
        Vector3 moveto= new Vector3(tmp.x,-130,tmp.z);
        var transF=this.gameObject.transform;
        transF.DOMove(moveto,3f).SetEase(Ease.Linear);
    }
    public void GetObj(GameObject Cube){
        HitEvent=Cube;
        // Debug.Log("Corect");
    }
    // public bool[] key= new bool[88];
    void Update(){
        if(HitEvent==null) Destroy(this.gameObject);
        tmp = this.gameObject.transform.position;
        // bool[] Key=MicSpectrumAnalyz.key_judg;
        if(tmp.y<-82f && tmp.y>-88f) {
            // if(Key[(int)(tmp.x/5)+69]){
            //     Destroy(this.gameObject);
            // }
            HitEvent.GetComponent<HitEvent>().GetKeyPos((int)(tmp.x/5)+69,this.gameObject);
            // HitEvent.GetKeyPos((int)(tmp.x/5)+69,this.gameObject);
        }
        if(fin && tmp.y<=-88f){
            fin=false;
            HitEvent.GetComponent<HitEvent>().fin_Combo();
        }
        if(tmp.y<-120f) Destroy(this.gameObject);
    }
}

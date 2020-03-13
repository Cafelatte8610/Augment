using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyEvent : MonoBehaviour
{

    public Vector3 tmp;

    public HitEvent HitEvent;

    void Start(){
        tmp = this.gameObject.transform.position;
        Vector3 moveto= new Vector3(tmp.x,-130,tmp.z);
        var transF=this.gameObject.transform;
        transF.DOMove(moveto,3f).SetEase(Ease.Linear);
    }

    void Update(){
        tmp = this.gameObject.transform.position;
        if(tmp.y<-84f && tmp.y>-86f) {
            HitEvent.GetKeyPos((int)(tmp.x/5)+69);
        }
        
        if(tmp.y<-120f) Destroy(this.gameObject);   
    }

    // public int return_key(){
    //     return (int)(tmp.x/5)+69;
    // }
}

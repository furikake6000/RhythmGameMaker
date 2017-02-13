using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    #region editor params

    public Vector3 startPos, endPos;    //始点終点
    public AnimationCurve xMoveCurve, yMoveCurve;   //X、Y各座標において移動するカーブ
    public float startSize, endSize;    //開始時サイズ、終了時サイズ
    public AnimationCurve sizeCurve;    //サイズ変更カーブ
    public Timing endTime;  //ゴールに着くタイミング
    public float existTime; //存在している時間（単位はMusicalTime）

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //現在の位置を0～1のfloatで取得
        float justTimePos;
        justTimePos = 1.0f - (Music.MusicalTimeFrom(endTime) / existTime);
        //座標移動
        Vector3 newPos = Vector3.zero;
        newPos.x = Mathf.Lerp(startPos.x, endPos.x, xMoveCurve.Evaluate(justTimePos));
        newPos.y = Mathf.Lerp(startPos.y, endPos.y, yMoveCurve.Evaluate(justTimePos));
        transform.position = newPos;
        //サイズ変更
        transform.localScale = Vector3.one * sizeCurve.Evaluate(justTimePos);
    }
}

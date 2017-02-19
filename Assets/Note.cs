using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuriLib;

public class Note : MonoBehaviour {

    #region editor params

    public Vector3 startPos, endPos;    //始点終点
    //public AnimationCurve xMoveCurve, yMoveCurve;   //X、Y各座標において移動するカーブ
    public float startSize = 1.0f, endSize = 1.0f;    //開始時サイズ、終了時サイズ
    //public AnimationCurve sizeCurve;    //サイズ変更カーブ
    public int startSample, endSample; //始点・終点に着く時間（単位はサンプル数）
    public float destroyPostponedTime = 4.0f;   //終点についてから消えるまでの時間（単位は秒）

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //現在の位置を0～1のfloatで取得
        float justTimePos;
        justTimePos = (float)(Music.TimeSamples - startSample) / (float)(endSample - startSample);
        //座標移動
        Vector3 newPos = Vector3.zero;
        newPos.x = MyMathf.LerpUnlimited(startPos.x, endPos.x, justTimePos);
        newPos.y = MyMathf.LerpUnlimited(startPos.y, endPos.y, justTimePos);
        transform.position = newPos;
        //サイズ変更
        float newSize = MyMathf.LerpUnlimited(startSize, endSize, justTimePos);
        transform.localScale = Vector3.one * newSize;

        //消滅
        if(Music.TimeSamples > endSample + destroyPostponedTime * Music.CurrentSource.clip.frequency)
        {
            Destroy(gameObject);
        }
    }
}
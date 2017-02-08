﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    #region editor params

    //グラフィック関係
    public Vector3 NoteStartPos;    //ノーツが流れ始める位置
    public GameObject Notes;    //ノーツのPrefab
    public ParticleSystem TouchEffect;  //タッチした時のエフェクト

    //入力関係
    public KeyCode[] AppliedKeys;   //この鍵盤に対応する（キーボードの）キー一覧
    public bool IsClickEnable;  //クリックによる操作が有効か
    public bool IsTouchEnable;    //タッチ操作が有効か

    #endregion

    #region private params

    Collider2D col;  //当たり判定事前取得用変数

    #endregion

    #region public functions

    public void KeyClicked()
    {
        if (IsClickEnable) KeyPressed();
    }
    public void KeyTouched()
    {
        if (IsTouchEnable) KeyPressed();
    }

    #endregion

    #region private functions

    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //Input関係
        //Key Input処理
        bool isAppliedKeyPushed = false;
        foreach (KeyCode akey in AppliedKeys)
        {
            if (Input.GetKeyDown(akey))isAppliedKeyPushed = true;   //一つでも割当キーが押されたら反応
        }
        if (isAppliedKeyPushed) KeyPressed();

	}

    // Inputイベントが発生したら（キータッチ、タップ、クリックのいずれか有効なものが行われたら）
    void KeyPressed()
    {

    }

    #endregion

}

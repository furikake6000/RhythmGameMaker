﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    #region editor params

    //グラフィック関係
    public Vector3 NoteStartPos;    //ノーツが流れ始める位置
    public GameObject NotePrefab;    //ノーツのPrefab
    public ParticleSystem TouchEffect;  //タッチした時のエフェクト

    //入力関係
    public KeyCode[] AppliedKeys;   //この鍵盤に対応する（キーボードの）キー一覧
    public bool IsClickEnable;  //クリックによる操作が有効か
    public bool IsTouchEnable;    //タッチ操作が有効か

    #endregion

    #region private params

    List<GameObject> Notes = new List<GameObject>();   //ノーツ

    #endregion

    #region public functions

    /// <summary>
    /// オブジェクトクリック時呼び出し関数。
    /// キーボード以外のInputイベントは一括管理され、この関数で各オブジェクトに通知される。
    /// </summary>
    public void KeyClicked()
    {
        if (IsClickEnable) KeyPressed();
    }

    /// <summary>
    /// オブジェクトタッチ時呼び出し関数。
    /// キーボード以外のInputイベントは一括管理され、この関数で各オブジェクトに通知される。
    /// </summary>
    public void KeyTouched()
    {
        if (IsTouchEnable) KeyPressed();
    }

    #endregion

    #region private functions

    /// <summary>
    /// コード読み込み時に呼び出されるStart関数
    /// </summary>
    void Start () {
        if(NotePrefab.GetComponent<Note>() == null)
        {
            //Noteコンポーネントつけ忘れ時の警告機能１
            Debug.LogWarning("Warning: NotePrefab does not have Note component.");
        }
	}

    /// <summary>
    /// 各フレームで呼び出されるUpdate関数
    /// </summary>
    void Update () {
        //Input関係
        //Key Input処理
        bool isAppliedKeyPushed = false;
        foreach (KeyCode akey in AppliedKeys)
        {
            if (Input.GetKeyDown(akey))isAppliedKeyPushed = true;   //一つでも割当キーが押されたら反応
        }
        if (isAppliedKeyPushed) KeyPressed();

        //Notes出力関係
        if (Music.IsJustChangedBar())
        {
            Timing newTime = new Timing(1);
            newTime.Add(Music.Just, Music.CurrentSection);
            CreateNote(newTime);
        }
	}

    /// <summary>
    /// いずれかのInputイベントが発生した時に呼び出される
    /// </summary>
    void KeyPressed()
    {
        //自分と同じ位置までエフェクトを移動
        TouchEffect.transform.position = transform.position;
        //エフェクトをひとつ発する
        TouchEffect.Emit(1);

        //Debugメッセージ
        Debug.Log("Key(" + gameObject.name + ") has pressed!\n");
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    void CreateNote(Timing beatTime)
    {
        GameObject newNoteObject = GameObject.Instantiate(NotePrefab);
        Note newNote = newNoteObject.GetComponent<Note>();
        if (newNote == null)
        {
            //もしNotePrefabにNoteコンポーネントがついていなかった時の措置
            newNote = newNoteObject.AddComponent<Note>();
        }
        newNote.startPos = NoteStartPos;
        newNote.endPos = transform.position;
        newNote.startTime = Music.MusicalTime;  //開始時刻は現在時刻
        newNote.endTime = beatTime.GetMusicalTime(Music.CurrentSection);
        //StartSize、EndSizeはPrefabに紐付けられた情報のまま
    }

    #endregion

}

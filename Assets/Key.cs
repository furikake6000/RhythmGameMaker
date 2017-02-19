using System.Collections;
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
        if (Music.IsJustChangedBeat())
        {
            if(Music.Just.Beat == 2)
            {
                Timing newTime = new Timing(0, 2);
                newTime.Add(Music.Just, Music.CurrentSection);
                CreateNote(newTime);
            }
            
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
        newNoteObject.transform.localScale = Vector3.zero;  //一瞬チラつくのを抑制
        Note newNote = newNoteObject.GetComponent<Note>();
        if (newNote == null)
        {
            //もしNotePrefabにNoteコンポーネントがついていなかった時の措置
            newNote = newNoteObject.AddComponent<Note>();
        }
        newNote.startPos = NoteStartPos;
        newNote.endPos = transform.position;
        newNote.startSample = Music.TimeSamples;
        newNote.endSample = TimingToSample(beatTime);
        //StartSize、EndSizeはPrefabに紐付けられた情報のまま
    }

    /// <summary>
    /// Timing型からtimeSamples(int)への変換
    /// </summary>
    /// <param name="t">Timing型変数</param>
    /// <returns>timeSamples(int) エラーで-1</returns>
    int TimingToSample(Timing t)
    {
        int i;

        for(i=0; i<Music.SectionCount - 1; i++)
        {
            if(t.Bar < Music.GetSection(i + 1).StartBar)
            {
                break;
            }
        }

        //セクション内のTimingのみを抽出
        Timing sectionStart = new Timing(Music.GetSection(i).StartBar);
        t.Subtract(sectionStart, Music.GetSection(i));
        //セクション開始からのUnit数を計算
        int totalUnitFromSectionStart = t.Bar * Music.GetSection(i).UnitPerBar + t.Beat * Music.GetSection(i).UnitPerBeat + t.Unit;
        //セクション開始からのサンプル数を計算
        int samplesFromSectionStart = (int)(totalUnitFromSectionStart * Music.CurrentSource.clip.frequency * 60 / (Music.GetSection(i).Tempo * Music.GetSection(i).UnitPerBeat));
        //セクション開始時のサンプル数を加算
        int samplesFromMusicStart = Music.GetSection(i).StartTimeSamples + samplesFromSectionStart;

        return samplesFromMusicStart;
    }

    #endregion

}

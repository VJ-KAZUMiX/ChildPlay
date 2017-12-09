using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBlaster : MonoBehaviour
{
    /// <summary>
    /// SE 最大同時再生数
    /// </summary>
    private const int MAX_SE = 16;

    /// <summary>
    /// SE の種類。
    /// </summary>
    public enum SoundType
    {
        Bound,
        Split,
        Merge
    }

    /// <summary>
    /// 種類とファイル名の対応
    /// </summary>
    private Dictionary<SoundType, string> type2name = new Dictionary<SoundType, string> () {
        {SoundType.Bound, "taiko03" },
        {SoundType.Split, "hand-drum01" },
        {SoundType.Merge, "clappers01" },
    };

    /// <summary>
    /// SE
    /// </summary>
    [SerializeField]
    private AudioClip[] seClips;

    /// <summary>
    /// ファイル名がキーの SE 辞書
    /// </summary>
    private Dictionary<string, AudioClip> seDic;

    /// <summary>
    /// SE 再生用
    /// </summary>
    private List<AudioSource> seAudioSourceList;

    /// <summary>
    /// 再生ソース指定用
    /// </summary>
    private int seSourceIndex = 0;

    /// <summary>
    /// 再生希望リスト
    /// </summary>
    private List<SoundType> playList = new List<SoundType> () { Capacity = 8 };

    /// <summary>
    /// シングルトン
    /// </summary>
    private static SoundBlaster _instance;
    public static SoundBlaster Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// Unity Awake
    /// </summary>
    private void Awake ()
    {
        if (_instance == null) {
            _instance = this;
        } else {
            Debug.LogError ("SoundBlaster はすでに存在します");
            return;
        }

        seDic = new Dictionary<string, AudioClip> ();
        foreach (AudioClip audioClip in seClips) {
            seDic[audioClip.name] = audioClip;
        }

        // SE 再生用
        seAudioSourceList = new List<AudioSource> () { Capacity = MAX_SE };
        for (int i = 0; i < MAX_SE; i++) {
            seAudioSourceList.Add (gameObject.AddComponent<AudioSource> ());
        }
    }

    /// <summary>
    /// Unity Update
    /// </summary>
    private void Update ()
    {
        PlayPlayList ();
    }

    /// <summary>
    /// 再生予約
    /// </summary>
    /// <param name="soundType"></param>
    public void Play(SoundType soundType)
    {
        if (playList.Contains (soundType)) {
            return;
        }
        playList.Add (soundType);
    }

    /// <summary>
    /// 再生する
    /// </summary>
    private void PlayPlayList ()
    {
        for (int i = 0, len = playList.Count; i < len; i++) {
            SoundType soundType = playList[i];
            string name = type2name[soundType];
            if (!seDic.ContainsKey (name)) {
                Debug.LogErrorFormat ("対応する SE がありません: {0}, {1}", soundType, name);
                continue;
            }
            AudioClip audioClip = seDic[name];

            AudioSource audioSource = seAudioSourceList[seSourceIndex++];
            seSourceIndex %= MAX_SE;
            audioSource.clip = audioClip;
            audioSource.Play ();
        }

        playList.Clear ();
    }
}

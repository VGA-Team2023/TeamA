// 日本語対応

using CriWare;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CriAudioManager
{
    /// <summary>インスタンス</summary>
    private static CriAudioManager _instance = null;

    /// <summary>インスタンス</summary>
    public static CriAudioManager Instance
    {
        get
        {
            _instance ??= new CriAudioManager();
            return _instance;
        }
    }

    private CriAudioManager()
    {
        _masterVolume = new Volume();
        _bgm = new CriSingleChannel(_masterVolume);
        _se = new CriMultiChannel(_masterVolume);
    }

    /// <summary>マスターのボリューム</summary>
    private Volume _masterVolume = default;

    /// <summary>BGMを流すチャンネル</summary>
    private CriSingleChannel _bgm = default;

    /// <summary>SEを流すチャンネル</summary>
    private CriMultiChannel _se = default;

    /// <summary>マスターボリューム</summary>
    public IVolume MasterVolume => _masterVolume;

    /// <summary>BGMのチャンネル</summary>
    public ICustomChannel BGM => _bgm;

    /// <summary>SEのチャンネル</summary>
    public ICustomChannel SE => _se;

    /// <summary>SEのPlayerとPlayback</summary>
    private struct CriPlayerData
    {
        /// <summary>再生中の音声のPlayback</summary>
        public CriAtomExPlayback Playback { get; set; }

        /// <summary>再生中のCueに関する情報</summary>
        public CriAtomEx.CueInfo CueInfo { get; set; }

        public CriAtomEx3dSource Source { get; set; }

        public float LastUpdateTime { get; set; }

        public bool IsLoop => CueInfo.length < 0;

        /// <summary>ポジションを更新する & 進行方向の予想ベクトルを返す</summary>
        /// <param name="nextPos">次のポジション</param>
        /// <returns>一秒間に進む予想ベクトル</returns>
        public void UpdateCurrentVector(Vector3 nextPos)
        {
            //前回のアップデートからの経過時間
            var elapsed = Playback.GetTime() - LastUpdateTime;

            //ポジションからベクトルを算出
            CriAtomEx.NativeVector nativePos = Source.GetPosition();
            Vector3 currentPos = new Vector3(nativePos.x, nativePos.y, nativePos.z);
            Vector3 movedVec = nextPos - currentPos;
            movedVec /= elapsed;

            LastUpdateTime = Playback.GetTime();
            Source.SetPosition(nextPos.x, nextPos.y, nextPos.z);
            Source.SetVelocity(movedVec.x, movedVec.y, movedVec.z);
            Source.Update();
        }
    }

    /// <summary>チャンネルを作るために必要な情報をまとめたクラス</summary>
    private abstract class AbstractCriChannel
    {
        /// <summary>AudioPlayer</summary>
        protected CriAtomExPlayer _player = new CriAtomExPlayer();

        /// <summary>キューのPlayback</summary>
        protected ConcurrentDictionary<int, CriPlayerData> _cueData = new ConcurrentDictionary<int, CriPlayerData>();

        /// <summary>現在までの最大の_cuDataのカウント</summary>
        protected int _currentMaxCount = 0;

        /// <summary>_cueDataのリムーブされたインデックス</summary>
        protected ConcurrentBag<int> _removedCueDataIndex = new ConcurrentBag<int>();

        /// <summary>リスナー</summary>
        protected CriAtomEx3dListener _listener = default;

        /// <summary>ボリューム</summary>
        protected Volume _volume = new Volume();

        /// <summary>マスターボリューム</summary>
        protected Volume _masterVolume = null;

        /// <summary>CancellationTokenSource</summary>
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        protected AbstractCriChannel(in Volume masterVolume)
        {
            _masterVolume = masterVolume;

            _volume.OnVolumeChanged += UpdateVolume;
            _masterVolume.OnVolumeChanged += UpdateMasterVolume;
        }

        ~AbstractCriChannel()
        {
            _tokenSource.Cancel();
            _volume.OnVolumeChanged -= UpdateVolume;
            _masterVolume.OnVolumeChanged -= UpdateMasterVolume;
            _player.Dispose();
        }

        private void UpdateVolume(float volume)
        {
            _player.SetVolume(volume * _masterVolume);

            foreach (var data in _cueData)
            {
                _player.Update(data.Value.Playback);
            }
        }

        private void UpdateMasterVolume(float masterVolume)
        {
            _player.SetVolume(_volume * masterVolume);

            foreach (var data in _cueData)
            {
                _player.Update(data.Value.Playback);
            }
        }

        protected async void PlaybackDestroyWaitForPlayEnd(int index)
        {
            // ループしていたら抜ける
            if (_cueData[index].IsLoop)
            {
                return;
            }

            await Task.Delay((int)_cueData[index].CueInfo.length, _tokenSource.Token);

            while (true)
            {
                if (_cueData.TryRemove(index, out CriPlayerData outData))
                {
                    _removedCueDataIndex.Add(index);
                    outData.Source?.Dispose();
                    return;
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.05D), _tokenSource.Token);
                }
            }
        }
    }

    /// <summary>音楽を管理するための機能を持ったInterface</summary>
    public interface ICustomChannel
    {
        /// <summary>ボリューム</summary>
        public IVolume Volume { get; }

        /// <summary>音楽を流す関数</summary>
        /// <param name="cueSheetName">流したいキューシートの名前</param>
        /// <param name="cueName">流したいキューの名前</param>
        /// <param name="volume">ボリューム</param>
        /// <returns>操作する際に必要なIndex</returns>
        public int Play(string cueSheetName, string cueName, float volume = 1.0F);

        /// <summary>音楽を流す関数(3D)</summary>
        /// <param name="playSoundWorldPos">流すPositionのWorldSpace</param>
        /// <param name="cueSheetName">流したいキューシートの名前</param>
        /// <param name="cueName">流したいキューの名前</param>
        /// <param name="volume">ボリューム</param>
        /// <returns>操作する際に必要なIndex</returns>
        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume = 1.0F);

        /// <summary>3Dの流すPositionを更新する</summary>
        /// <param name="playSoundWorldPos">更新するPosition</param>
        /// <param name="index">変更する音声のPlay時の戻り値(Index)</param>
        public void Update3DPos(Vector3 playSoundWorldPos, int index);

        /// <summary>音声をPauseさせる</summary>
        /// <param name="index">Pauseさせたい音声のPlay時の戻り値(Index)</param>
        public void Pause(int index);

        /// <summary>Pauseさせた音声をResumeさせる</summary>
        /// <param name="index">Resumeさせたい音声のPlay時の戻り値(Index)</param>
        public void Resume(int index);

        /// <summary>音声をStopさせる</summary>
        /// <param name="index">Stopさせたい音声のPlay時の戻り値(Index)</param>
        public void Stop(int index);

        /// <summary>すべての音声をStopさせる</summary>
        public void StopAll();

        /// <summary>ループしている音声すべてをStopさせる</summary>
        public void StopLoopCue();

        /// <summary>すべてのリスナーを設定する</summary>
        /// <param name="listener">リスナー</param>
        public void SetListenerAll(CriAtomListener listener);

        /// <summary>リスナーを設定する</summary>
        /// <param name="listener">リスナー</param>
        /// <param name="index">リスナーを変更したい音声のPlay時の戻り値</param>
        public void SetListener(CriAtomListener listener, int index);
    }

    /// <summary>BGMなどに使用する一つの音のみを出力するチャンネル</summary>
    private class CriSingleChannel : AbstractCriChannel, ICustomChannel
    {
        /// <summary>現在再生中のAcb</summary>
        private CriAtomExAcb _currentAcb = null;

        /// <summary>現在再生中のCueSheetName</summary>
        private string _currentCueName = "";

        /// <summary>コンストラクタ－</summary>
        /// <param name="masterVolume">マスターボリューム</param>
        public CriSingleChannel(Volume masterVolume) : base(masterVolume)
        {
            // TODO - Addに失敗したい際の処理を追加する
            _cueData.TryAdd(0, new CriPlayerData());
        }

        public IVolume Volume => _volume;

        public int Play(string cueSheetName, string cueName, float volume = 1.0F)
        {
            // CueSheetから情報を取得
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            if (_currentAcb == tempAcb && _currentCueName == cueName
                                       && _player.GetStatus() == CriAtomExPlayer.Status.Playing)
            {
                return _cueData.Count - 1;
            }

            Stop(_cueData.Count - 1);

            // 情報をセットして再生
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.Set3dSource(null);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();

            _cueData[_cueData.Count - 1] = tempPlayerData;

            return _cueData.Count - 1;
        }

        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume = 1.0F)
        {
            // CueSheetから情報を取得
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            if (_currentAcb == tempAcb && _currentCueName == cueName
                                       && _player.GetStatus() == CriAtomExPlayer.Status.Playing)
            {
                return _cueData.Count - 1;
            }

            Stop(_cueData.Count - 1);

            // 座標情報をセットして再生
            var temp3dData = new CriAtomEx3dSource();

            temp3dData.SetPosition(playSoundWorldPos.x, playSoundWorldPos.y, playSoundWorldPos.z);
            // リスナーとソースを設定
            _player.Set3dListener(_listener);
            _player.Set3dSource(temp3dData);
            tempPlayerData.Source = temp3dData;
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();

            _cueData[_cueData.Count - 1] = tempPlayerData;

            return _cueData.Count - 1;
        }

        public void Update3DPos(Vector3 playSoundWorldPos, int index)
        {
            if (index <= -1 || _cueData[index].Source == null) return;

            _cueData[index].UpdateCurrentVector(playSoundWorldPos);
        }

        public void Pause(int index)
        {
            if (index <= -1) return;

            _player.Pause();
        }

        public void Resume(int index)
        {
            if (index <= -1) return;

            _player.Resume(CriAtomEx.ResumeMode.PausedPlayback);
        }

        public void Stop(int index)
        {
            if (index <= -1) return;

            _player.Stop(false);
        }

        public void StopAll()
        {
            _player.Stop(false);
        }

        public void StopLoopCue()
        {
            _player.Stop(false);
        }

        public void SetListenerAll(CriAtomListener listener)
        {
            _player.Set3dListener(listener.nativeListener);
            _player.UpdateAll();
        }

        public void SetListener(CriAtomListener listener, int index)
        {
            if (_cueData[index].Playback.GetStatus() == CriAtomExPlayback.Status.Removed || index <= -1) return;

            _player.Set3dListener(listener.nativeListener);
            _player.Update(_cueData[index].Playback);
        }
    }

    private class CriMultiChannel : AbstractCriChannel, ICustomChannel
    {
        public CriMultiChannel(in Volume masterVolume) : base(in masterVolume)
        {
        }

        public IVolume Volume => _volume;

        public int Play(string cueSheetName, string cueName, float volume)
        {
            if (cueName == "") return -1;

            CriAtomEx.CueInfo cueInfo;
            CriPlayerData newAtomPlayer = new CriPlayerData();

            var tempAcb = CriAtom.GetAcb(cueSheetName);
            tempAcb.GetCueInfo(cueName, out cueInfo);

            newAtomPlayer.CueInfo = cueInfo;

            _player.SetCue(tempAcb, cueName);
            _player.Set3dSource(null);
            _player.SetVolume(volume * _volume * _masterVolume);
            newAtomPlayer.Playback = _player.Start();

            if (_removedCueDataIndex.Count > 0)
            {
                int tempIndex;
                if (_removedCueDataIndex.TryTake(out tempIndex))
                {
                    _cueData.TryAdd(tempIndex, newAtomPlayer);
                }

                PlaybackDestroyWaitForPlayEnd(tempIndex);
            }
            else
            {
                _currentMaxCount++;
                _cueData.TryAdd(_currentMaxCount, newAtomPlayer);

                PlaybackDestroyWaitForPlayEnd(_currentMaxCount);
            }

            return _cueData.Count - 1;
        }

        public int Play3D(Vector3 playSoundWorldPos, string cueSheetName, string cueName, float volume)
        {
            // CueSheetから情報を取得
            var tempAcb = CriAtom.GetAcb(cueSheetName);
            var tempPlayerData = new CriPlayerData();
            tempAcb.GetCueInfo(cueName, out CriAtomEx.CueInfo tempInfo);
            tempPlayerData.CueInfo = tempInfo;

            // 座標情報をセットして再生
            var temp3dData = new CriAtomEx3dSource();

            temp3dData.SetPosition(playSoundWorldPos.x, playSoundWorldPos.y, playSoundWorldPos.z);
            // リスナーとソースを設定
            _player.Set3dListener(_listener);
            _player.Set3dSource(temp3dData);
            tempPlayerData.Source = temp3dData;
            _player.SetCue(tempAcb, cueName);
            _player.SetVolume(_volume * _masterVolume * volume);
            _player.SetStartTime(0L);
            tempPlayerData.Playback = _player.Start();

            _cueData[_cueData.Count - 1] = tempPlayerData;

            if (_removedCueDataIndex.Count > 0)
            {
                int tempIndex;
                if (_removedCueDataIndex.TryTake(out tempIndex))
                {
                    _cueData.TryAdd(tempIndex, tempPlayerData);
                }

                PlaybackDestroyWaitForPlayEnd(tempIndex);
            }
            else
            {
                _currentMaxCount++;
                _cueData.TryAdd(_currentMaxCount, tempPlayerData);

                PlaybackDestroyWaitForPlayEnd(_currentMaxCount);
            }

            return _cueData.Count - 1;
        }

        public void Update3DPos(Vector3 playSoundWorldPos, int index)
        {
            if (index <= -1 || _cueData[index].Source == null) return;

            _cueData[index].UpdateCurrentVector(playSoundWorldPos);
        }

        public void Pause(int index)
        {
            if (index <= -1) return;

            _cueData[index].Playback.Pause();
        }

        public void Resume(int index)
        {
            if (index <= -1) return;

            _cueData[index].Playback.Resume(CriAtomEx.ResumeMode.AllPlayback);
        }

        public void Stop(int index)
        {
            if (index <= -1) return;

            _cueData[index].Playback.Stop(false);
        }

        public void StopAll()
        {
            _player.Stop(false);
        }

        public void StopLoopCue()
        {
            foreach (var n in _cueData)
            {
                if (n.Value.IsLoop)
                {
                    n.Value.Playback.Stop(false);
                }
            }
        }

        public void SetListenerAll(CriAtomListener listener)
        {
            _listener = listener.nativeListener;
            _player.Set3dListener(_listener);
            _player.UpdateAll();
        }

        public void SetListener(CriAtomListener listener, int index)
        {
            _listener = listener.nativeListener;
            _player.Set3dListener(_listener);
            _player.Update(_cueData[index].Playback);
        }
    }

    public interface IVolume
    {
        public event Action<float> OnVolumeChanged;

        public float Value { get; set; }

        public static IVolume operator +(IVolume volume, IVolume volume2) => volume;
    }

    /// <summary>ボリューム</summary>
    private class Volume : IVolume
    {
        /// <summary>ボリューム</summary>
        private float _value = 1.0F;

        /// <summary>音量が変更された際の処理</summary>
        private event Action<float> _onVolumeChanged = default;

        public event Action<float> OnVolumeChanged
        {
            add => _onVolumeChanged += value;
            remove => _onVolumeChanged -= value;
        }

        /// <summary>イベントが呼ばれる際の基準の差</summary>
        private const float DIFF = 0.01F;

        /// <summary>ボリューム</summary> 
        public float Value
        {
            get => _value;
            set
            {
                value = Mathf.Clamp01(value);

                if (_value + DIFF < value || _value - DIFF > value)
                {
                    _onVolumeChanged?.Invoke(value);
                    _value = value;
                }
            }
        }

        public static implicit operator float(Volume volume) => volume.Value;
    }
}
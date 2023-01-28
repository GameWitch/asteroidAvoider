using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // members

    public static AudioController instance;
     
    public bool debug;
   
    [NonReorderable]
    public AudioTrack[] tracks;

    private Hashtable m_AudioTable; // relationship between audiotypes (key) and audio tracks (value)
    private Hashtable m_JobTable; // relationship between audiotypes (key) and jobs (value) (Coroutine, IENumerator)

    private int soundTrackIndex = 0;
    private AudioType currentSong;
    
    
    
    // helper classes 

    [System.Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public class AudioTrack 
    {
        public AudioSource source;
        [NonReorderable]
        public AudioObject[] audio;
        public VolumeSliderAudioSource volumeSource;
    }

    private class AudioJob
    {
        public AudioAction action;
        public AudioType type;
        public bool fade;
        public float delay;

        public AudioJob(AudioAction _audioAction, AudioType _audioType, bool _fade = false, float _delay = 0.0f)
        {
            action = _audioAction;
            type = _audioType;
            fade = _fade;
            delay = _delay;
        }
    }
    private enum AudioAction
    {
        START,
        STOP,
        RESTART,
    }


#region UNITY FUNCTIONS
    private void Awake()
    {
        if (!instance)
        {
            Configure();
            PlayNextSong();
        }
    }

    private void OnDisable()
    {
        Dispose();
    }
#endregion



#region PUBLIC FUNCTIONS
    public void PlayAudio(AudioType _type, bool _fade=false, float delay=0.0f)
    {
        AddJob(new AudioJob(AudioAction.START, _type,  _fade,  delay));
    }
    public void StopAudio(AudioType _type, bool _fade = false, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioAction.STOP, _type,  _fade,  delay));

    }
    public void RestartAudio(AudioType _type, bool _fade = false, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioAction.RESTART, _type,  _fade,  delay));

    }

    public void ChangeVolume(float value, VolumeSliderAudioSource audioSource)
    {
        AdjustAudioSourceVolume(value, audioSource);
    }

    public List<AudioType> GetMusicPlaylist()
    {
        List<AudioType> audioTypes = new List<AudioType>();
        foreach (AudioObject _obj in tracks[0].audio)
        {
            audioTypes.Add(_obj.type);
        }
        return audioTypes;
        
    }

    #endregion



    #region PRIVATE FUNCTIONS

    private void FadeOutAtEnd()
    {
        StopAudio(currentSong, true);
        Invoke("PlayNextSong", 1.0f);
    }
    private void PlayNextSong()
    {
        currentSong = tracks[0].audio[soundTrackIndex].type;
        PlayAudio(currentSong, true);
        soundTrackIndex++;
        soundTrackIndex = soundTrackIndex < tracks[0].audio.Length ? soundTrackIndex : 0;
        float clipLength = tracks[0].audio[soundTrackIndex].clip.length - 1f;
        Invoke("FadeOutAtEnd", clipLength);
    }

    private void VolumePrefSettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("MAIN");
        tracks[0].source.volume = PlayerPrefs.GetFloat("MUSIC");
        tracks[tracks.Length - 1].source.volume = PlayerPrefs.GetFloat("UISFX");

        for (int i = 1; i < tracks.Length - 1; i++)
        {
            tracks[i].source.volume = PlayerPrefs.GetFloat("GAMESFX");
        }
    }

    private void AdjustAudioSourceVolume(float value, VolumeSliderAudioSource audioSource)
    {
        switch (audioSource)
        {
            case (VolumeSliderAudioSource.MAIN):
                AudioListener.volume = value;
                PlayerPrefs.SetFloat("MAIN", value);
                break;

            case (VolumeSliderAudioSource.MUSIC):

                tracks[0].source.volume = value;
                PlayerPrefs.SetFloat("MUSIC", value);
                break;

            case (VolumeSliderAudioSource.GAMESFX):

                PlayerPrefs.SetFloat("GAMESFX", value);
                for (int i = 1; i < tracks.Length - 1; i++)
                {
                    tracks[i].source.volume = value;
                }
                break;

            case (VolumeSliderAudioSource.UISFX):

                PlayerPrefs.SetFloat("UISFX", value);
                tracks[tracks.Length - 1].source.volume = value;
                break;
        }
    }

    private IEnumerator RunAudioJob(AudioJob _job)
    {
        yield return new WaitForSeconds(_job.delay);

        AudioTrack _track = (AudioTrack)m_AudioTable[_job.type];
        _track.source.clip = GetAudioClipFromAudioTrack(_job.type, _track);

        switch(_job.action)
        {
            case AudioAction.START:
                _track.source.Play();
                break;
            case AudioAction.STOP:
                if (!_job.fade)
                {
                    _track.source.Stop();
                }
                break;
            case AudioAction.RESTART:
                _track.source.Stop();
                _track.source.Play();
                break;
        }
        
        if (_job.fade)
        {
            float savedVolume = PlayerPrefs.GetFloat(_track.volumeSource.ToString());
            float _initial = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? 0.0f : savedVolume;
            float _target = _job.action == AudioAction.START || _job.action == AudioAction.RESTART ? savedVolume : 0.0f;
            float _duration = 1.0f;
            float _timer = 0.0f;

            while(_timer < _duration)
            {
                _track.source.volume = Mathf.Lerp(_initial, _target, _timer / _duration);
                _timer += Time.deltaTime;
                yield return null;
            }
            if (_job.action == AudioAction.STOP)
            {
                _track.source.Stop();
            }
        }


        m_JobTable.Remove(_job.type);
        Log("Job Count: " + m_JobTable.Count);

        yield return null;

    }
    private void AddJob(AudioJob _job)
    {
        // remove conflicting jobs
        RemoveConflictingJobs(_job.type);

        // start job
        IEnumerator _jobRunner = RunAudioJob(_job);
        m_JobTable.Add(_job.type, _jobRunner);
        StartCoroutine(_jobRunner);
        Log("starting job on [" + _job.type + "] with operation [" + _job.action + "]");
    }
    private void RemoveJob(AudioType _type)
    {
        if (!m_JobTable.ContainsKey(_type))
        {
            LogWarning("You are attempting to stop a job["+_type+"] that is not running");
            return;
        }

        IEnumerator _runningJob = (IEnumerator)m_JobTable[_type];
        StopCoroutine(_runningJob);
        m_JobTable.Remove(_type);
    }
    private void RemoveConflictingJobs(AudioType _type)
    {
        if (m_JobTable.ContainsKey(_type))
        {
            RemoveJob(_type);
        }

        AudioType _conflictAudio = AudioType.NONE;
        foreach(DictionaryEntry _entry in m_JobTable)
        {
            AudioType _audioType = (AudioType)_entry.Key;
            AudioTrack _audioTrackInUse = (AudioTrack)m_AudioTable[_audioType];
            AudioTrack _audioTrackNeeded = (AudioTrack)m_AudioTable[_type];
            if (_audioTrackNeeded.source == _audioTrackInUse.source)
            {
                _conflictAudio = _audioType;
            }
        }
        if (_conflictAudio != AudioType.NONE)
        {
            RemoveJob(_conflictAudio);
        }
    }
    private AudioClip GetAudioClipFromAudioTrack(AudioType _type, AudioTrack _track)
    {
        foreach (AudioObject _obj in _track.audio)
        {
            if (_obj.type == _type)
            {
                return _obj.clip;
            }
        }
        return null;
    }
    private void Configure()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        VolumePrefSettings();

        m_AudioTable = new Hashtable();
        m_JobTable = new Hashtable();

        GenerateAudioTable();

    }

    private void GenerateAudioTable()
    {
        foreach(AudioTrack _track in tracks)
        {
            foreach(AudioObject _obj in _track.audio)
            {
                // do not duplicate keys
                if (m_AudioTable.ContainsKey(_obj.type))
                {
                    LogWarning("You are trying to register audio [" + _obj.type + "] that has already been registered");
                }
                else
                {
                    m_AudioTable.Add(_obj.type, _track);
                    Log("registering audio [" +_obj.type + "]");
                }
            }
        }
    }
    private void Dispose()
    {
        if (m_JobTable != null)
        {
            foreach (DictionaryEntry _entry in m_JobTable)
            {
                IEnumerator _job = (IEnumerator)_entry.Value;
                StopCoroutine(_job);
            }
        }

    }
    private void Log(string _msg)
        {
            if (!debug) return;
            Debug.Log("[Audio Controller]: " + _msg);
        }

        private void LogWarning(string _msg)
        {
            if (!debug) return;
            Debug.Log("[Audio Controller Warning]: " + _msg);
        }
#endregion
}

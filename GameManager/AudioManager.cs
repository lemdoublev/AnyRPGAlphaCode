using AnyRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AnyRPG {
    public class AudioManager : MonoBehaviour {

        #region Singleton
        private static AudioManager instance;

        public static AudioManager MyInstance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<AudioManager>();
                }
                return instance;
            }
        }

        #endregion

        [SerializeField]
        private AudioMixer audioMixer = null;

        private float defaultMasterVolume = 1.0f;
        private float defaultMusicVolume = 1.0f;
        private float defaultAmbientVolume = 1.0f;
        private float defaultEffectsVolume = 1.0f;
        private float defaultUIVolume = 1.0f;
        private float defaultVoiceVolume = 1.0f;

        private string masterVolume = "MasterVolume";
        private string musicVolume = "MusicVolume";
        private string effectsVolume = "EffectsVolume";
        private string uiVolume = "UIVolume";
        private string voiceVolume = "VoiceVolume";
        private string ambientVolume = "AmbientVolume";

        [SerializeField]
        private AudioSource musicAudioSource = null;

        [SerializeField]
        private AudioSource effectsAudioSource = null;

        [SerializeField]
        private AudioSource ambientAudioSource = null;

        [SerializeField]
        private AudioSource uiAudioSource = null;

        [SerializeField]
        private AudioSource voiceAudioSource = null;

        [SerializeField]
        private AudioClip uiClickSound = null;

        private bool audioInitialized = false;

        private bool musicPaused = false;

        public string MyMasterVolume { get => masterVolume; }
        public string MyMusicVolume { get => musicVolume; }
        public string MyEffectsVolume { get => effectsVolume; }
        public string MyAmbientVolume { get => ambientVolume; }
        public AudioClip MyUIClickSound { get => uiClickSound; set => uiClickSound = value; }
        public AudioSource MyMusicAudioSource { get => musicAudioSource; set => musicAudioSource = value; }
        public AudioSource MyEffectsAudioSource { get => effectsAudioSource; set => effectsAudioSource = value; }
        public AudioSource MyAmbientAudioSource { get => ambientAudioSource; set => ambientAudioSource = value; }
        public AudioSource MyUiAudioSource { get => uiAudioSource; set => uiAudioSource = value; }
        public AudioClip UiClickSound { get => uiClickSound; set => uiClickSound = value; }
        public AudioSource MyVoiceAudioSource { get => voiceAudioSource; set => voiceAudioSource = value; }
        public string MyUiVolume { get => uiVolume; set => uiVolume = value; }
        public string MyVoiceVolume { get => voiceVolume; set => voiceVolume = value; }

        private void Awake() {
            //Debug.Log("AudioManager.Awake()");
        }

        private void Start() {
            //Debug.Log("AudioManager.Start()");
            InitializeVolume();
        }

        private void InitializeVolume() {
            //Debug.Log("AudioManager.InitializeVolume()");
            if (audioInitialized) {
                return;
            }
            SetDefaultVolume();
            audioInitialized = true;
        }

        private void SetDefaultVolume() {
            //Debug.Log("AudioManager.SetDefaultVolume()");
            if (PlayerPrefs.HasKey(MyMasterVolume) == true) {
                SetVolume(MyMasterVolume, PlayerPrefs.GetFloat(MyMasterVolume));
            } else {
                SetMasterVolume(defaultMasterVolume);
            }
            if (PlayerPrefs.HasKey(MyMusicVolume) == true) {
                SetVolume(MyMusicVolume, PlayerPrefs.GetFloat(MyMusicVolume));
            } else {
                SetMusicVolume(defaultMusicVolume);
            }
            if (PlayerPrefs.HasKey(MyAmbientVolume) == true) {
                SetVolume(MyAmbientVolume, PlayerPrefs.GetFloat(MyAmbientVolume));
            } else {
                SetAmbientVolume(defaultAmbientVolume);
            }
            if (PlayerPrefs.HasKey(MyEffectsVolume) == true) {
                SetVolume(MyEffectsVolume, PlayerPrefs.GetFloat(MyEffectsVolume));
            } else {
                SetMusicVolume(defaultEffectsVolume);
            }
            if (PlayerPrefs.HasKey(MyUiVolume) == true) {
                SetVolume(MyUiVolume, PlayerPrefs.GetFloat(MyUiVolume));
            } else {
                SetMusicVolume(defaultUIVolume);
            }
            if (PlayerPrefs.HasKey(MyVoiceVolume) == true) {
                SetVolume(MyVoiceVolume, PlayerPrefs.GetFloat(MyVoiceVolume));
            } else {
                SetMusicVolume(defaultVoiceVolume);
            }
        }

        private void SetVolume(string volumeType, float volume) {
            //Debug.Log("AudioManager.SetVolume(" + volumeType + ", " + volume + "): setting audioMixer float");
            audioMixer.SetFloat(volumeType, GetLogVolume(volume));
        }

        private float GetLogVolume(float volume) {
            //Debug.Log("AudioManager.GetLogVolume(" + volume + ")");
            float logVolume = -80.0f;
            float alternateLogVolume = -80f;
            if (volume > 0) {
                logVolume = Mathf.Log10(volume) * 20f;
                alternateLogVolume = (1 - Mathf.Sqrt(volume)) * -80f;
            }
            //Debug.Log("AudioManager.GetLogVolume(" + volume + "): returning: " + logVolume + "; alternateVolume: " + alternateLogVolume);
            return logVolume;
        }

        public void SetMasterVolume(float volume) {
            //Debug.Log("AudioManager.SetMasterVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyMasterVolume, volume);
            audioMixer.SetFloat(MyMasterVolume, GetLogVolume(volume));
        }

        public void SetMusicVolume(float volume) {
            //Debug.Log("AudioManager.SetMusicVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyMusicVolume, volume);
            audioMixer.SetFloat(MyMusicVolume, GetLogVolume(volume));
        }

        public void SetAmbientVolume(float volume) {
            //Debug.Log("AudioManager.SetAmbientVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyAmbientVolume, volume);
            audioMixer.SetFloat(MyAmbientVolume, GetLogVolume(volume));
        }

        public void SetEffectsVolume(float volume) {
            //Debug.Log("AudioManager.SetEffectsVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyEffectsVolume, volume);
            audioMixer.SetFloat(MyEffectsVolume, GetLogVolume(volume));
        }

        public void SetUIVolume(float volume) {
            //Debug.Log("AudioManager.SetEffectsVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyUiVolume, volume);
            audioMixer.SetFloat(MyUiVolume, GetLogVolume(volume));
        }

        public void SetVoiceVolume(float volume) {
            //Debug.Log("AudioManager.SetEffectsVolume(" + volume + ")");
            PlayerPrefs.SetFloat(MyVoiceVolume, volume);
            audioMixer.SetFloat(MyVoiceVolume, GetLogVolume(volume));
        }

        public float GetVolume(string volumeType) {
            //Debug.Log("AudioManager.GetVolume()");
            float returnValue = 0f;
            if (!audioMixer.GetFloat(volumeType, out returnValue)) {
                //Debug.Log("could not get float!");
            }
            //Debug.Log("AudioManager.GetVolume(): returnValue: " + returnValue);
            return returnValue;
        }

        public void PlayAmbientSound(AudioClip audioClip) {
            InitializeVolume();
            ambientAudioSource.clip = audioClip;
            ambientAudioSource.loop = true;
            ambientAudioSource.Play();
        }

        public void PlayMusic(AudioClip audioClip) {
            InitializeVolume();
            if (musicAudioSource.clip == audioClip && musicPaused == true) {
                UnPauseMusic();
                return;
            }
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

        public void PlayEffect(AudioClip audioClip) {
            if (audioClip == null) {
                return;
            }
            InitializeVolume();
            effectsAudioSource.PlayOneShot(audioClip);
        }

        public void PlayUI(AudioClip audioClip) {
            if (audioClip == null) {
                return;
            }
            InitializeVolume();
            uiAudioSource.PlayOneShot(audioClip);
        }

        public void PlayVoice(AudioClip audioClip) {
            if (audioClip == null) {
                return;
            }
            InitializeVolume();
            voiceAudioSource.PlayOneShot(audioClip);
        }

        public void PlayUIHoverSound() {
            if (uiClickSound != null) {
                PlayUI(uiClickSound);
            }
        }

        public void PlayUIClickSound() {
            //Debug.Log("AudioManager.PlayUIClickSound()");
            if (uiClickSound != null) {
                //Debug.Log("AudioManager.PlayUIClickSound(): click sound is not null");
                PlayUI(uiClickSound);
            } else {
                //Debug.Log("AudioManager.PlayUIClickSound(): click sound is null");
            }
        }

        public void StopAmbientSound() {
            ambientAudioSource.Stop();
        }

        public void StopMusic() {
            musicAudioSource.Stop();
            musicPaused = false;
        }

        public void PauseMusic() {
            if (musicPaused == false) {
                musicAudioSource.Pause();
                musicPaused = true;
            } else {
                UnPauseMusic();
            }
        }

        
        private void UnPauseMusic() {
            musicAudioSource.UnPause();
            musicPaused = false;
        }

        public void StopEffects() {
            effectsAudioSource.Stop();
        }
        public void StopUI() {
            uiAudioSource.Stop();
        }
        public void StopVoice() {
            voiceAudioSource.Stop();
        }

    }

}
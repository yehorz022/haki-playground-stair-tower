using UnityEngine;

namespace Assets.Scripts.RunMode
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static float soundVolume = 1;
        [SerializeField] AudioSource sound;
        [SerializeField] SoundClip[] sounds;

        public static AudioManager instance;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PlaySound(SoundID id)
        {
            sound.clip = sounds[(int)id].clip;
            sound.volume = sounds[(int)id].volume * soundVolume;
            sound.Play(); // play sound
        }

        public void PlaySoundSync(SoundID id)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/" + id.ToString()), Camera.main.transform.position, sounds[(int)id].volume * soundVolume);
        }
    }

    [System.Serializable]
    public class SoundClip
    {
        public SoundID id;
        public AudioClip clip;
        public float volume;
    }

    public enum SoundID
    {
        Click = 0,
        Drop = 1,
        Trash = 2,
        Join = 3,
        Tick = 4,
        Woosh = 5,
        Twitch = 6,
        Locked = 7,
        Favorite = 8,
        Unfavorite = 9,
        Notification = 10,
    };
}



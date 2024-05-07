using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;



public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public SoundObject[] audioSources;
    public static SoundManager Instance //인스턴스에 접근하기 위한 프로퍼티
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 인스턴스가 없습니다");
                }

            }
            return _instance;
        }
    }

    private AudioSource audioSource; // 사운드를 재생할 AudioSource 컴포넌트
    private AudioClip audioClip;
    [SerializeField] public Dictionary<string, Stack<GameObject>> soundPool = new Dictionary<string, Stack<GameObject>>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다. (GameManager 인스턴스는 언제나 하나여야 한다 )
        else if (_instance != this)
        {
            Debug.Log("씬에 두개이상의 인스턴스가 있습니다");
            Destroy(gameObject);
        }
        //이렇게 하면 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        //DontDestroyOnLoad(gameObject);

        audioSources =  Resources.LoadAll<SoundObject>("Sounds");
        
        foreach (var source in audioSources) 
        {
            string Key =  source.ID.ToString();
            if (soundPool.ContainsKey(Key))
            {
                continue;
            }
            soundPool[Key] = new Stack<GameObject>();
            for(int i = 0; i<5; i++)
            {
                GameObject soundObj = Instantiate(source.gameObject);
                soundPool[Key].Push(soundObj);
                soundObj.SetActive(false);
            }
        }

    }

    //오디오 중첩시 볼륨 배분 코드
    public List<AudioSource> audioList;
    private float originalVolume;

    void Start()
    {
        /*
        if (audioList.Count > 0)
        {
            originalVolume = audioList[0].volume;
        }
        */
    }

    void Update()
    {
        int activeSources = 0;
        foreach (var source in audioList)
        {
            if (source.isPlaying)
            {
                activeSources++;
            }
        }

        float newVolume = activeSources > 0 ? originalVolume / activeSources : originalVolume;
        foreach (var source in audioList)
        {
            source.volume = newVolume;
        }
    }



    // 사운드를 재생하는 함수
    public GameObject PlaySound(GameObject clip , Vector3 tf, int key)
    {
        string Key = key.ToString();
        //audioClip = clip.soundClip;

        if (soundPool.ContainsKey(Key) && soundPool[Key].Count > 0)
        {
            // 풀에 해당하는 키가 있고, 풀에 오브젝트가 있는 경우
            clip = soundPool[Key].Pop();


            //사운드가 동시에 여럿 재생될때 각 볼륨크기를 배분하여 재생한다.
            AudioSource a = clip.GetComponent<AudioSource>();
            if (!audioList.Contains(a))
            {
                audioList.Add(clip.GetComponent<AudioSource>());
                if (originalVolume == 0)
                {
                    originalVolume = audioList[0].volume;
                }
            }



            clip.transform.position = tf;
            clip.gameObject.SetActive(true);
        }
        else
        {
            // 풀에 해당하는 키가 없거나 풀에 오브젝트가 없는 경우
            GameObject soundObj = Instantiate(clip, tf, Quaternion.identity);
            soundObj.gameObject.SetActive(true);
            if (!soundPool.ContainsKey(Key))
            {
                soundPool[Key] = new Stack<GameObject>();
            }
            soundPool[Key].Push(soundObj);
        }
        return null;
    }
    public void ReleaseObject(GameObject obj, int key) // 오브젝트를 풀로 되돌린다.
    {
        obj.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
        string Key = key.ToString();
        if (!soundPool.ContainsKey(Key))
        {
            soundPool[Key] = new Stack<GameObject>();
        }
        soundPool[Key].Push(obj);
    }

}
/*public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public void PlaySoundTrack(int num)
    {
        audioSources[0].Play();
    }

    public void PlayBGM()
    {
        //audioSource.PlayOneShot(audioClips[0]);
        audioSource.Play();
    }
    void Start()
    {
        //PlaySoundTrack(0);
    }
}
*/

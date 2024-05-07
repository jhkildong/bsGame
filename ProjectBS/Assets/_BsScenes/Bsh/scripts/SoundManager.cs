using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;



public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public SoundObject[] audioSources;
    public static SoundManager Instance //�ν��Ͻ��� �����ϱ� ���� ������Ƽ
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                {
                    Debug.Log("�̱��� �ν��Ͻ��� �����ϴ�");
                }

            }
            return _instance;
        }
    }

    private AudioSource audioSource; // ���带 ����� AudioSource ������Ʈ
    private AudioClip audioClip;
    [SerializeField] public Dictionary<string, Stack<GameObject>> soundPool = new Dictionary<string, Stack<GameObject>>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�. (GameManager �ν��Ͻ��� ������ �ϳ����� �Ѵ� )
        else if (_instance != this)
        {
            Debug.Log("���� �ΰ��̻��� �ν��Ͻ��� �ֽ��ϴ�");
            Destroy(gameObject);
        }
        //�̷��� �ϸ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
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

    //����� ��ø�� ���� ��� �ڵ�
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



    // ���带 ����ϴ� �Լ�
    public GameObject PlaySound(GameObject clip , Vector3 tf, int key)
    {
        string Key = key.ToString();
        //audioClip = clip.soundClip;

        if (soundPool.ContainsKey(Key) && soundPool[Key].Count > 0)
        {
            // Ǯ�� �ش��ϴ� Ű�� �ְ�, Ǯ�� ������Ʈ�� �ִ� ���
            clip = soundPool[Key].Pop();


            //���尡 ���ÿ� ���� ����ɶ� �� ����ũ�⸦ ����Ͽ� ����Ѵ�.
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
            // Ǯ�� �ش��ϴ� Ű�� ���ų� Ǯ�� ������Ʈ�� ���� ���
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
    public void ReleaseObject(GameObject obj, int key) // ������Ʈ�� Ǯ�� �ǵ�����.
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

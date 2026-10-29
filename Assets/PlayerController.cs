using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 15f; 
    public float turnSpeed = 100f;   

    [Header("音效設定")]
    public AudioSource flightAudio;   // 飛行持續音效
    public AudioClip crashClip;       // 撞到紅色障礙物的音效
    public AudioClip passClip;        // 順利通過綠色方塊的音效
    private AudioSource sfxAudio;     

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 自動取得或新增第二個 AudioSource 播放音效
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length > 1)
        {
            sfxAudio = sources[1];
        }
        else
        {
            sfxAudio = gameObject.AddComponent<AudioSource>();
        }

        // 播放飛行持續音效
        if (flightAudio != null)
        {
            flightAudio.loop = true;
            flightAudio.Play();
        }
    }

    void Update()
    {
        // 1. 持續向前移動
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 2. 左右方向控制 (A / D 鍵)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
    }

    // 穿過綠色方塊 (Trigger 觸發)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Green") || other.name.Contains("Green"))
        {
            if (sfxAudio != null && passClip != null)
            {
                sfxAudio.PlayOneShot(passClip);
                Debug.Log("順利通過綠色方塊！");
            }
        }
    }

    // 撞到紅色方塊 (實體碰撞)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Red") || collision.gameObject.name.Contains("Red"))
        {
            if (sfxAudio != null && crashClip != null)
            {
                sfxAudio.PlayOneShot(crashClip);
                Debug.Log("撞到紅色障礙物！");
            }
        }
    }
}
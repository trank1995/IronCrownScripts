using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {
    public static VideoManager videoManager;
    public Canvas vidCanvas;
    public RawImage image;
    public VideoPlayer videoPlayer;
    public VideoSource videoSource;
    public AudioSource audioSource;
    public List<VideoClip> chap1;
    public VideoClip curClip;
    public static bool playing;
    float playingBuffer = 0;
	// Use this for initialization
	void Start () {
        videoManager = this;
        gameObject.SetActive(false);
        //playVideo(1, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //long playerFrameCount = System.Convert.ToInt64(VP.GetComponent<VideoPlayer>().frameCount);
        if (playingBuffer <= .1f)
        {
            if (videoPlayer.frame == (long)videoPlayer.frameCount)
            {
                WorldInteraction.playerControllable = true;
                playing = false;
                gameObject.SetActive(false);
            }
        } else
        {
            playingBuffer -= Time.deltaTime;
        }
        
		if (Input.GetKeyUp(KeyCode.Escape))
        {
            WorldInteraction.playerControllable = true;
            gameObject.SetActive(false);
        }
	}

    public void playVideo(int chap, int num)
    {
        gameObject.SetActive(true);
        WorldInteraction.playerControllable = false;
        switch (chap)
        {
            case 1:
                curClip = chap1[num];
                break;
        }
        Application.runInBackground = true;
        StartCoroutine(playVideo());
        playingBuffer = 3.0f;
        playing = true;
    }

    IEnumerator playVideo()
    {

        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //We want to play from video clip not from url

        videoPlayer.source = VideoSource.VideoClip;
        

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = curClip;
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;
        image.rectTransform.sizeDelta = new Vector2(videoPlayer.texture.width, videoPlayer.texture.height);
        image.rectTransform.localPosition = new Vector3(0, 0, 0);

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();
        
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        
    }
}

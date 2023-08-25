using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;


    public void preLoadVideo(string videoUrl)
    {
        videoPlayer.url = videoUrl;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.Prepare();
        videoPlayer.Pause();
    }


    public void stopVideo()
    {
        videoPlayer.Stop();
    }

    public void startVideo()
    {
        videoPlayer.Play();
    }

    public void toggleVideo()
    {
        if (videoPlayer.isPlaying)
        {
            stopVideo();
        }
        else
        {
            startVideo();
        }
    }
}

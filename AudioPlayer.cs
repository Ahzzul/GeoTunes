using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

public class AudioPlayer
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    private bool isPlaying = false;
    private string lastPlayed = "";

    // Asynchron MP3 abspielen, blockiert solange die Audio läuft
    public async Task PlayMp3Async(string[] files, int index, bool checkForAnotherSong)
    {
        if (isPlaying)
        {
            // Bereits eine Wiedergabe aktiv → warten bis fertig
            return;
        }
        if (lastPlayed == files[index] && checkForAnotherSong)
        {
            if (lastPlayed == files[0])
            {    
                index++;
            }
            else
            {
                index--;
            }
        }

        isPlaying = true;
        lastPlayed = files[index];
        var tcs = new TaskCompletionSource<bool>();

        audioFile = new AudioFileReader(files[index]);
        outputDevice = new WaveOutEvent();

        outputDevice.Init(audioFile);

        outputDevice.PlaybackStopped += (s, e) =>
        {
            outputDevice.Dispose();
            audioFile.Dispose();
            outputDevice = null;
            audioFile = null;
            isPlaying = false;
            tcs.SetResult(true);
        };

        outputDevice.Play();

        await tcs.Task; // call will be blocked until mp3 finished playing
    }

  
    public bool IsPlaying()
    {
        return isPlaying;
    }
}
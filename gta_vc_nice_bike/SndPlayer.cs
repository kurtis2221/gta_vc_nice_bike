using NAudio.Wave;

namespace gta_vc_nice_bike
{
    internal class SndPlayer
    {
        AudioFileReader snd_src;
        WaveOut snd_player;

        public SndPlayer(string fname)
        {
            snd_src = new AudioFileReader(fname);
            snd_player = new WaveOut();
            snd_player.Init(snd_src);
        }

        public void Play()
        {
            snd_player.Stop();
            snd_src.Position = 0;
            snd_player.Play();
        }

        public void SetVolume(float volume)
        {
            snd_src.Volume = volume;
        }
    }
}

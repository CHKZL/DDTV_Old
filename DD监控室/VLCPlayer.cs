using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace DD监控室
{
    public class VLCPlayer : IDisposable
    {
        // Properties

        private IntPtr libvlc_instance_;
        private IntPtr libvlc_media_player_;

        private bool HasInstance => (libvlc_instance_ != IntPtr.Zero);
        private bool HasPlayer => HasInstance && (libvlc_media_player_ != IntPtr.Zero);

        public string Version => VLCAPI.libvlc_get_version();
        public bool IsInitlized => HasPlayer;

        public double PlayTime
        {
            get => VLCAPI.libvlc_media_player_get_time(libvlc_media_player_) / 1000.0;
            set => VLCAPI.libvlc_media_player_set_time(libvlc_media_player_, (Int64)(value * 1000));
        }
        public int Volume
        {
            get => VLCAPI.libvlc_audio_get_volume(libvlc_media_player_);
            set => VLCAPI.libvlc_audio_set_volume(libvlc_media_player_, value);
        }
        public bool Fullscreen
        {
            get => VLCAPI.libvlc_get_fullscreen(libvlc_media_player_) == 1;
            set => VLCAPI.libvlc_set_fullscreen(libvlc_media_player_, value ? 1 : 0);
        }

        // Methods

        public VLCPlayer()
        {

            //string plugin_arg = "--plugin-path=" + System.Environment.CurrentDirectory + "\\plugins\\";
            //"--network-caching=300"减少网络延迟300秒
            // "--no-snapshot-preview"
            //"-I", "dummy", "--ignore-config", "--extraintf=logger", 
            string[] args = { "--verbose=2", "--network-caching=300", "--no-snapshot-preview" };

            byte[][] argvbytes = new byte[args.Length][];

            for (int i = 0; i < args.Length; i++)
            {
                argvbytes[i] = Encoding.UTF8.GetBytes(args[i]);
            }

            libvlc_instance_ = VLCAPI.libvlc_new(args.Length, VLCUtil.ReturnIntPtr(argvbytes, args.Length));

            if (HasInstance)
                libvlc_media_player_ = VLCAPI.libvlc_media_player_new(libvlc_instance_);

            if (!HasPlayer)
                Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (HasPlayer)
            {
                Stop(); // Stop (no effect if there is no media)

                VLCAPI.libvlc_media_player_release(libvlc_media_player_);
                libvlc_media_player_ = IntPtr.Zero;
            }

            if (HasInstance)
            {
                VLCAPI.libvlc_release(libvlc_instance_);
                libvlc_instance_ = IntPtr.Zero;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public int GetPlayerState()
        {
            try
            {
                return VLCAPI.libvlc_media_player_get_state(libvlc_media_player_);
            }
            catch (Exception)
            {
                return -10;
            }

        }

        private void SetRenderWindow(int wndHandle)
        {
            if (wndHandle != 0)
            {
                VLCAPI.libvlc_media_player_set_hwnd(libvlc_media_player_, wndHandle);
            }
        }

        #region Media Resource

        private void LoadMediaFromUrl(string path)
        {
            IntPtr pathPtr = VLCUtil.StringToPtr(path);
            IntPtr md = VLCAPI.libvlc_media_new_location(libvlc_instance_, pathPtr);
            Marshal.FreeHGlobal(pathPtr);

            SetMedia(md);
        }

        private void LoadMediaFromFile(string path)
        {
            IntPtr pathPtr = VLCUtil.StringToPtr(path);
            IntPtr md = VLCAPI.libvlc_media_new_path(libvlc_instance_, pathPtr);
            Marshal.FreeHGlobal(pathPtr);

            SetMedia(md);
        }

        private void SetMedia(IntPtr media)
        {
            if (media != IntPtr.Zero)
            {
                VLCAPI.libvlc_media_player_set_media(libvlc_media_player_, media); // If any, previous media will be released.
                VLCAPI.libvlc_media_release(media);
            }
        }

        #endregion // Media Resource

        #region Play Control

        public void Play(string filePath, IntPtr handle)
        {
            this.SetRenderWindow((int)handle);
            LoadMediaFromUrl(filePath);

            VLCAPI.libvlc_media_player_play(libvlc_media_player_);
        }

        public void PlayFile(string filePath, IntPtr handle)
        {
            this.SetRenderWindow((int)handle);
            LoadMediaFromFile(filePath);

            VLCAPI.libvlc_media_player_play(libvlc_media_player_);
        }

        public void Pause()
        {
             VLCAPI.libvlc_media_player_pause(libvlc_media_player_);
        }

        public void Stop()
        {
            VLCAPI.libvlc_media_player_stop(libvlc_media_player_);
        }

        public int Snapshot(string savePath, uint width = 0, uint height = 0)
        {
            IntPtr pathPtr = VLCUtil.StringToPtr(savePath);
            int result = VLCAPI.libvlc_video_take_snapshot(libvlc_media_player_, 0, pathPtr, width, height);
            Marshal.FreeHGlobal(pathPtr);

            return result;
        }

        #endregion // Play Control
    }
}

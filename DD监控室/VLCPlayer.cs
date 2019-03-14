using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.ExceptionServices;
using System.Security;

namespace DD监控室
{
    public class VLCPlayer : IDisposable
    {
        private IntPtr libvlc_instance_;
        private IntPtr libvlc_media_player_;

        //private double duration_;

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


            if (libvlc_instance_ != IntPtr.Zero)
                libvlc_media_player_ = VLCAPI.libvlc_media_player_new(libvlc_instance_);


        }
        public void Dispose()
        {
            if (libvlc_instance_ != IntPtr.Zero)
                VLCAPI.libvlc_release(libvlc_instance_);
        }
        /// <summary>
        /// 设置播放窗口
        /// </summary>
        /// <param name="wndHandle"></param>
        private void SetRenderWindow(int wndHandle)
        {

            if (libvlc_instance_ != IntPtr.Zero && wndHandle != 0)
            {

                VLCAPI.libvlc_media_player_set_hwnd(libvlc_media_player_, wndHandle);

            }

        }

        public void Play(string filePath, IntPtr handle)
        {
            if (libvlc_instance_ == IntPtr.Zero || libvlc_media_player_ == IntPtr.Zero) return;
            this.SetRenderWindow((int)handle);

            IntPtr pathPtr = VLCUtil.StringToPtr(filePath);
            //从url取视频
            IntPtr libvlc_media = VLCAPI.libvlc_media_new_location(libvlc_instance_, pathPtr);
            Marshal.FreeHGlobal(pathPtr);
            if (libvlc_media != IntPtr.Zero)
            {
                //取时长前必须调用 
                //VLCAPI.libvlc_media_parse(libvlc_media);


                //duration_ = VLCAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;

                VLCAPI.libvlc_media_player_set_media(libvlc_media_player_, libvlc_media);

                VLCAPI.libvlc_media_release(libvlc_media);

                VLCAPI.libvlc_media_player_play(libvlc_media_player_);

            }

        }
        public void PlayFile(string filePath, IntPtr handle)
        {
            if (libvlc_instance_ == IntPtr.Zero || libvlc_media_player_ == IntPtr.Zero) return;
            this.SetRenderWindow((int)handle);

            IntPtr pathPtr = VLCUtil.StringToPtr(filePath);
            //从url取视频
            IntPtr libvlc_media = VLCAPI.libvlc_media_new_path(libvlc_instance_, pathPtr);
            Marshal.FreeHGlobal(pathPtr);
            if (libvlc_media != IntPtr.Zero)
            {
                //取时长前必须调用 
                //VLCAPI.libvlc_media_parse(libvlc_media);


                //duration_ = VLCAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;

                VLCAPI.libvlc_media_player_set_media(libvlc_media_player_, libvlc_media);

                VLCAPI.libvlc_media_release(libvlc_media);

                VLCAPI.libvlc_media_player_play(libvlc_media_player_);

            }

        }

        public void Pause()
        {

            if (libvlc_media_player_ != IntPtr.Zero)
            {

                VLCAPI.libvlc_media_player_pause(libvlc_media_player_);

            }

        }

       

        public void Stop()
        {

            if (libvlc_media_player_ != IntPtr.Zero)
            {

                VLCAPI.libvlc_media_player_stop(libvlc_media_player_);

            }

        }

        public double GetPlayTime()
        {
            if (libvlc_media_player_ == IntPtr.Zero) return 0;
            return VLCAPI.libvlc_media_player_get_time(libvlc_media_player_) / 1000.0;

        }

        public void SetPlayTime(double seekTime)
        {
            if (libvlc_media_player_ != IntPtr.Zero)
                VLCAPI.libvlc_media_player_set_time(libvlc_media_player_, (Int64)(seekTime * 1000));

        }

        public int GetVolume()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
                return VLCAPI.libvlc_audio_get_volume(libvlc_media_player_);
            else
                return -1;
        }

        public void SetVolume(int volume)
        {
            if (libvlc_media_player_ != IntPtr.Zero)
                VLCAPI.libvlc_audio_set_volume(libvlc_media_player_, volume);

        }

        public void SetFullScreen(bool istrue)
        {
            if (libvlc_media_player_ != IntPtr.Zero)
                VLCAPI.libvlc_set_fullscreen(libvlc_media_player_, istrue ? 1 : 0);

        }

       
        public string Version()
        {

            return VLCAPI.libvlc_get_version();

        }
        /// <summary>
        /// 获取播放状态
        /// </summary>
        /// <returns></returns>
        [HandleProcessCorruptedStateExceptions]
        public int getPlayerState()
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


        /// <summary>
        /// 抓图(支持多种格式)
        /// </summary>
        /// <param name="savePath">完整路径（bmp,png,jpg）</param>
        /// <param name="width">抓图宽度</param>
        /// <param name="height">抓图高度</param>
        /// <returns></returns>
        public int SnapShot(string savePath, uint width = 0, uint height = 0)
        {
            int result = -1;
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                IntPtr pathPtr = VLCUtil.StringToPtr(savePath);
                result = VLCAPI.libvlc_video_take_snapshot(libvlc_media_player_, 0, pathPtr, width, height);
                Marshal.FreeHGlobal(pathPtr);

            }
            return result;
        }
    }
}

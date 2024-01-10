using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace AudioRecorder
{
    public partial class Form1 : Form
    {
        private string? outPutFileNameSpeaker;
        private string? outPutFileNameMic;
        private string? outPutFileName;
        private WasapiLoopbackCapture? loopbackCapture;
        private WasapiCapture? microphoneCapture;
        private WaveFileWriter? writer_Speaker;
        private WaveFileWriter? writer_Mic;

        public Form1()
        {
            InitializeComponent();
            LoadDevices();
        }

        private void LoadDevices()
        {
            // Load output devices (speakers)
            var outputEnumerator = new MMDeviceEnumerator();
            var outputDevices = outputEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            output_devices.Items.AddRange(outputDevices.ToArray());

            // Load input devices (microphones)
            var inputEnumerator = new MMDeviceEnumerator();
            var inputDevices = inputEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            input_devices.Items.AddRange(inputDevices.ToArray());

            output_devices.SelectedIndex = 0;
            input_devices.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_stop.Enabled = false;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                outPutFileNameSpeaker = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\speaker_{Guid.NewGuid()}.wav";
                outPutFileNameMic = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\mic_{Guid.NewGuid()}.wav";
                outPutFileName = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\final_{Guid.NewGuid()}.wav";


                //outPutFileNameSpeaker = Path.GetTempFileName();
                //outPutFileNameMic = Path.GetTempFileName();

                //FileInfo fileInfo1 = new FileInfo(outPutFileNameSpeaker);
                //fileInfo1.Attributes = FileAttributes.Temporary;

                //FileInfo fileInfo2 = new FileInfo(outPutFileNameMic);
                //fileInfo2.Attributes = FileAttributes.Temporary;



                var outputDevice = (MMDevice)output_devices.SelectedItem;
                var inputDevice = (MMDevice)input_devices.SelectedItem;

                loopbackCapture = new WasapiLoopbackCapture(outputDevice);
                microphoneCapture = new WasapiCapture(inputDevice);

                writer_Speaker = new WaveFileWriter(outPutFileNameSpeaker, loopbackCapture.WaveFormat);
                writer_Mic = new WaveFileWriter(outPutFileNameMic, loopbackCapture.WaveFormat);

                //record the audio which is in the speaker channel
                loopbackCapture.DataAvailable += (s, loopbackArgs) =>
                {
                    writer_Speaker.Write(loopbackArgs.Buffer, 0, loopbackArgs.BytesRecorded);
                };

                //record the audio from the mic
                microphoneCapture.DataAvailable += (s, microphoneArgs) =>
                {
                    writer_Mic.Write(microphoneArgs.Buffer, 0, microphoneArgs.BytesRecorded);
                };

                loopbackCapture.RecordingStopped += (s, loopbackArgs) =>
                {
                    writer_Speaker.Dispose();
                    writer_Mic.Dispose();

                    loopbackCapture.Dispose();
                    microphoneCapture.Dispose();

                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;

                    var startInfoSpeaker = new ProcessStartInfo
                    {
                        FileName = Path.GetDirectoryName(outPutFileNameSpeaker),
                        UseShellExecute = true
                    };
                    var startInfoMic = new ProcessStartInfo
                    {
                        FileName = Path.GetDirectoryName(outPutFileNameMic),
                        UseShellExecute = true
                    };
                    Process.Start(startInfoSpeaker);
                    Process.Start(startInfoMic);

                    using (var reader1 = new AudioFileReader(outPutFileNameSpeaker))
                    using (var reader2 = new AudioFileReader(outPutFileNameMic))
                    {
                        reader1.Volume = 0.75f;
                        reader2.Volume = 0.75f;
                        var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                        WaveFileWriter.CreateWaveFile16(outPutFileName, mixer);
                    }
                };

                loopbackCapture.StartRecording();
                microphoneCapture.StartRecording();

                btn_start.Enabled = false;
                btn_stop.Enabled = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (loopbackCapture != null)
            {
                loopbackCapture.StopRecording();
            }
            if (microphoneCapture != null)
            {
                microphoneCapture.StopRecording();
            }
        }






        //private void btn_stop_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (loopbackCapture != null)
        //        {
        //            loopbackCapture.StopRecording();
        //            loopbackCapture.Dispose(); // Dispose of the recording instance
        //        }

        //        if (microphoneCapture != null)
        //        {
        //            microphoneCapture.StopRecording();
        //            microphoneCapture.Dispose(); // Dispose of the recording instance
        //        }

        //        // Allow some time for the processes to release the files
        //        Thread.Sleep(1000);

        //        // Delete files after stopping and disposing of recording instances
        //        File.Delete(outPutFileNameSpeaker);
        //        File.Delete(outPutFileNameMic);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        // Handle exceptions or log errors if needed
        //    }
        //    finally
        //    {
        //        btn_start.Enabled = true;
        //        btn_stop.Enabled = false;
        //    }
        //}

    }
}

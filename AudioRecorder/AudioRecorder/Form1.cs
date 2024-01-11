using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace AudioRecorder
{
    public partial class Form1 : Form
    {
        private string? outPutFileNameSpeaker = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\Recorded Audio\\speaker_{Guid.NewGuid()}.wav";
        private string? outPutFileNameMic = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\Recorded Audio\\mic_{Guid.NewGuid()}.wav";
        private string? outPutFileName = $"C:\\Users\\rugyawali\\Desktop\\AudioRecorder\\Recorded Audio\\final_{Guid.NewGuid()}.wav";

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

            //the below code selects the depault playback and loopback devices whose main purpose it for communication
            //is commented for now as not in use
            //var ouptputDevice = outputEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
            //var onputDevice = outputEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_stop.Enabled = false;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {

                btn_start.Enabled = false;
                btn_stop.Enabled = true;

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

                    using (var reader1 = new AudioFileReader(outPutFileNameSpeaker))
                    using (var reader2 = new AudioFileReader(outPutFileNameMic))
                    {
                        reader1.Volume = 0.75f;
                        reader2.Volume = 0.75f;
                        var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                        WaveFileWriter.CreateWaveFile16(outPutFileName, mixer);
                    }

                    //if(!string.IsNullOrEmpty(outPutFileNameSpeaker) || !string.IsNullOrEmpty(outPutFileNameMic))
                    //{
                    // have supressed the warning of possible null refrence
                        File.Delete(outPutFileNameSpeaker!);
                        File.Delete(outPutFileNameMic!);
                    //}
                };

                loopbackCapture.StartRecording();
                microphoneCapture.StartRecording();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace AudioRecorder
{
    public partial class Form1 : Form
    {
        private string? outPutFileName;
        private WasapiLoopbackCapture? loopbackCapture;
        private WasapiCapture? microphoneCapture;
        private WaveFileWriter? writer;

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
            var dialog = new SaveFileDialog();
            dialog.Filter = "Wave files | *.wav";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //C: \\Users\\rugyawali\\Desktop\\AudioRecorder\\test 109.wav
            outPutFileName = dialog.FileName;

            var outputDevice = (MMDevice)output_devices.SelectedItem;
            var inputDevice = (MMDevice)input_devices.SelectedItem;

            loopbackCapture = new WasapiLoopbackCapture(outputDevice);
            microphoneCapture = new WasapiCapture(inputDevice);

            writer = new WaveFileWriter(outPutFileName, loopbackCapture.WaveFormat);

            loopbackCapture.DataAvailable += (s, loopbackArgs) =>
            {
                writer.Write(loopbackArgs.Buffer, 0, loopbackArgs.BytesRecorded);
            };

            microphoneCapture.DataAvailable += (s, microphoneArgs) =>
            {
                writer.Write(microphoneArgs.Buffer, 0, microphoneArgs.BytesRecorded);
            };

            loopbackCapture.RecordingStopped += (s, loopbackArgs) =>
            {
                writer.Dispose();
                loopbackCapture.Dispose();
                microphoneCapture.Dispose();

                btn_start.Enabled = true;
                btn_stop.Enabled = false;

                var startInfo = new ProcessStartInfo
                {
                    FileName = Path.GetDirectoryName(outPutFileName),
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            };

            loopbackCapture.StartRecording();
            microphoneCapture.StartRecording();

            btn_start.Enabled = false;
            btn_stop.Enabled = true;
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
    }
}

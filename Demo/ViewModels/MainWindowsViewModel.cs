using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.DataSources;
using ScottPlot.Stylers;
using SkiaSharp;

namespace QLingScope.ViewModels;

public class MainWindowsViewModel : ViewModelBase
{
    public MainWindowsViewModel(MainWindow view)
    {
        _view = view;
        Init();

        // _timer = new DispatcherTimer();
        // _timer.Interval = new TimeSpan(200);
        // _timer.Tick += Timer_Tick;
    }

    private DispatcherTimer _timer;
    private Thread _sampleThread;
    float[] ch1Xdata = new float[50000];
    float[] ch2Xdata = new float[50000];
    float[] ch1Volts = new float[50000];
    float[] ch2Volts = new float[50000];

    private MainWindow _view;

    private void Init()
    {
        TimeDivItems = new ObservableCollection<string>();
        VoltDivItems = new ObservableCollection<string>();
        for (int i = 0; i < QL6747Param.TimeSpanArr.Length; i++)
        {
            TimeDivItems.Add(QL6747Param.TimeSpanArr[i]);
        }

        for (int i = 0; i < QL6747Param.VoltDivArr.Length; i++)
        {
            VoltDivItems.Add(QL6747Param.VoltDivArr[i]);
        }

        // change figure colors
        _view.wavechart.Plot.FigureBackground.Color = Color.FromHex("#181818");
        _view.wavechart.Plot.DataBackground.Color = Color.FromHex("#1f1f1f");

        // change axis and grid colors
        _view.wavechart.Plot.Axes.Color(Color.FromHex("#707070"));
        _view.wavechart.Plot.Grid.MajorLineColor = Color.FromHex("#404040");

        // change legend colors
        _view.wavechart.Plot.Legend.BackgroundColor = Color.FromHex("#404040");
        _view.wavechart.Plot.Legend.FontColor = Color.FromHex("#d7d7d7");
        _view.wavechart.Plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");

        timeAxis = _view.wavechart.Plot.Axes.Bottom;
        _view.wavechart.Plot.Axes.Bottom.Label.Text = "Time (uS)";
        _view.wavechart.Plot.Axes.Title.Label.Text = "Oscilloscope Wave";
        ch1Yaxis = _view.wavechart.Plot.Axes.Left as LeftAxis;
        ch2Yaxis = new LeftAxis();
        _view.wavechart.Plot.Axes.AddYAxis(ch2Yaxis);
        ch2Yaxis.Color(Color.FromHex("#707070"));
        ch1Yaxis.LabelText = "CH1 Volts(mV)";
        ch2Yaxis.LabelText = "CH2 Volts(mV)";
        ch1Yaxis.LabelFontColor = Colors.YellowGreen;
        ch2Yaxis.LabelFontColor = Colors.Orange;
        ch1Yaxis.TickLabelStyle.ForeColor = Colors.YellowGreen;
        ch2Yaxis.TickLabelStyle.ForeColor = Colors.Orange;
        // ch1Yaxis.Color(Colors.YellowGreen);


        var sig1 = _view.wavechart.Plot.Add.SignalXY(this.ch1Xdata, this.ch1Volts, Colors.YellowGreen);
        sig1.LineWidth = 2;
        sig1.Axes.YAxis = ch1Yaxis;
        var sig2 = _view.wavechart.Plot.Add.SignalXY(this.ch2Xdata, this.ch2Volts, Colors.Orange);
        sig2.LineWidth = 2;
        sig2.Axes.YAxis = ch2Yaxis;

        MeasureCollection = new ObservableCollection<Measure>();
        MeasureCollection.Add(new Measure() { ChName = "Channel1" });
        MeasureCollection.Add(new Measure() { ChName = "Channel2" });

        TimeDivIndex = 9;
        VoltDivIndex = 5;
        VoltDivIndex1 = 5;

        try
        {
            byte[] slots = new byte[32];
            int devcount = PXIe6747.LoadDevice(slots);
            if (devcount <= 0)
            {
                Console.WriteLine("未发现任何设备！");
                return;
            }

            slot = slots[0];
            Console.WriteLine($"当前槽位:{slot}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private int _lastTimeDivIndex = 9;
    private LeftAxis ch1Yaxis = new LeftAxis();
    private LeftAxis ch2Yaxis = new LeftAxis();
    private IAxis timeAxis = new BottomAxis();
    public ObservableCollection<string> TimeDivItems { set; get; }
    public ObservableCollection<string> VoltDivItems { set; get; }

    private ObservableCollection<Measure> measureCollection;

    public ObservableCollection<Measure> MeasureCollection
    {
        set { SetProperty(ref measureCollection, value); }
        get { return measureCollection; }
    }

    private int timeDivIndex = 9;

    public int TimeDivIndex
    {
        get { return timeDivIndex; }
        set
        {
            SetProperty(ref timeDivIndex, value);
            double timediv = QL6747Param.MinTimeDivArr[timeDivIndex] / 1e6;

            double[] xIntervals = new double[11];
            string[] labs = new string[11];
            for (int i = 0; i <= 10; i++)
            {
                xIntervals[i] = i * timediv - 5 * timediv;
                labs[i] = ((int)xIntervals[i]).ToString();
            }

            timeAxis.Min = -5 * timediv;
            timeAxis.Max = 5 * timediv;
            timeAxis.SetTicks(xIntervals, labs);

            PXIe6747.SetTimeDiv(slot, QL6747Param.MinTimeDivArr[timeDivIndex]);
        }
    }

    private double timeDelay = 0;

    public double TimeDelay
    {
        get { return timeDelay; }
        set { SetProperty(ref timeDelay, value); }
    }

    private int timerefer = 0;

    public int Timerefer
    {
        get { return timerefer; }
        set { SetProperty(ref timerefer, value); }
    }

    private int voltDivIndex = 5;

    public int VoltDivIndex
    {
        get { return voltDivIndex; }
        set
        {
            SetProperty(ref voltDivIndex, value);
            double voltdiv = QL6747Param.MinVoltDivArr[voltDivIndex] / 1e3;

            double[] yIntervals = new double[9];
            string[] labs = new string[9];
            for (int i = 0; i <= 8; i++)
            {
                yIntervals[i] = i * voltdiv - 4 * voltdiv;
                labs[i] = ((int)yIntervals[i]).ToString();
            }

            ch1Yaxis.Min = -4 * voltdiv;
            ch1Yaxis.Max = 4 * voltdiv;

            ch1Yaxis.SetTicks(yIntervals, labs);
        }
    }

    private double offset = 0;

    public double Offset
    {
        get { return offset; }
        set { SetProperty(ref offset, value); }
    }

    private bool highImpedChecked = true;

    public bool HighImpedChecked
    {
        get { return highImpedChecked; }
        set { SetProperty(ref highImpedChecked, value); }
    }

    private bool dcChecked = true;

    public bool DcChecked
    {
        get { return dcChecked; }
        set { SetProperty(ref dcChecked, value); }
    }

    private bool maxBdChecked = true;

    public bool MaxBdChecked
    {
        get { return maxBdChecked; }
        set { SetProperty(ref maxBdChecked, value); }
    }

    private int voltDivIndex1 = 5;

    public int VoltDivIndex1
    {
        get { return voltDivIndex1; }
        set
        {
            SetProperty(ref voltDivIndex1, value);
            double voltdiv = QL6747Param.MinVoltDivArr[voltDivIndex1] / 1e3;

            double[] yIntervals = new double[9];
            string[] labs = new string[9];
            for (int i = 0; i <= 8; i++)
            {
                yIntervals[i] = i * voltdiv - 4 * voltdiv;
                labs[i] = ((int)yIntervals[i]).ToString();
            }

            ch2Yaxis.Min = -4 * voltdiv;
            ch2Yaxis.Max = 4 * voltdiv;

            ch2Yaxis.SetTicks(yIntervals, labs);
        }
    }

    private double offset1 = 0;

    public double Offset1
    {
        get { return offset1; }
        set { SetProperty(ref offset1, value); }
    }

    private bool highImpedChecked1 = true;

    public bool HighImpedChecked1
    {
        get { return highImpedChecked1; }
        set { SetProperty(ref highImpedChecked1, value); }
    }

    private bool dcChecked1 = true;

    public bool DcChecked1
    {
        get { return dcChecked1; }
        set { SetProperty(ref dcChecked1, value); }
    }

    private bool maxBdChecked1 = true;

    public bool MaxBdChecked1
    {
        get { return maxBdChecked1; }
        set { SetProperty(ref maxBdChecked1, value); }
    }

    private int chSource;

    public int Chsource
    {
        get { return chSource; }
        set { SetProperty(ref chSource, value); }
    }

    private int triggermode;

    public int Triggermode
    {
        get { return triggermode; }
        set { SetProperty(ref triggermode, value); }
    }

    private byte triggerway;

    public byte Triggerway
    {
        get { return triggerway; }
        set { SetProperty(ref triggerway, value); }
    }

    public double triggerVolt = 0;

    public double TrggerVolt
    {
        get { return triggerVolt; }
        set { SetProperty(ref triggerVolt, value); }
    }

    private bool isRun = false;
    private byte slot;

    public async void Start()
    {
        if (isRun)
        {
            return;
        }

        if (slot <= 0)
        {
            Console.WriteLine("槽号错误！");
            return;
        }

        await Task.Run(() =>
        {
            int status = PXIe6747.InitConfig(slot);
            if (status <= 0)
            {
                Console.WriteLine($"设备初始化失败！{status}");
                return;
            }

            Console.WriteLine("初始化成功!");
        });

        isRun = true;
        //_timer.Start();
        StartSampleTask();
        ProcessMeasureTask();
    }

    public void Stop()
    {
        isRun = false;
        // _timer.Stop();
    }

    private Random rd = new Random();

    public void Timer_Tick(object? sender, EventArgs e)
    {
        SampleData();
    }

    private void SampleData()
    {
        try
        {
            uint timeout = 5000;
            double offsetTime = 0;
            int length = 50000;
            Console.WriteLine("设置参数...");
            PXIe6747.SetTimeDiv(slot, QL6747Param.MinTimeDivArr[timeDivIndex]);
            PXIe6747.SetTimeDelay(slot, timeDelay * 1e6);
            PXIe6747.SetTimeRefer(slot, timerefer);

            PXIe6747.SetVoltDiv(slot, 0, (long)QL6747Param.MinVoltDivArr[voltDivIndex]);
            PXIe6747.SetVoltOffset(slot, 0, offset * 1e3);
            PXIe6747.SetImped(slot, 0, highImpedChecked ? 0 : 1, (long)QL6747Param.MinVoltDivArr[voltDivIndex]);
            PXIe6747.SetCouplingMode(slot, 0, dcChecked ? 0 : 1);
            PXIe6747.SetBandWidth(slot, 0, maxBdChecked ? 100e6 : 20e6);

            PXIe6747.SetVoltDiv(slot, 1, (long)QL6747Param.MinVoltDivArr[voltDivIndex1]);
            PXIe6747.SetVoltOffset(slot, 1, offset1 * 1e3);
            PXIe6747.SetImped(slot, 1, highImpedChecked1 ? 0 : 1, (long)QL6747Param.MinVoltDivArr[voltDivIndex1]);
            PXIe6747.SetCouplingMode(slot, 1, dcChecked1 ? 0 : 1);
            PXIe6747.SetBandWidth(slot, 1, maxBdChecked1 ? 100e6 : 20e6);

            PXIe6747.SetTriggerSource(slot, chSource, triggermode, triggerway);
            PXIe6747.SetTriggerVolt(slot, triggerVolt * 1e3);

            Console.WriteLine("设置参数完成！");
            Console.WriteLine("采集波形数据...");
            PXIe6747.ReadData(slot, ch1Xdata, ch2Xdata, ch1Volts, ch2Volts, ref length, timeout, ref offsetTime);
            Console.WriteLine("波形采集完成");

            for (int i = 0; i < length; i++)
            {
                // ch1Xdata[i] = (float)(10.0 / (length - 1) * i - 5);
                // ch2Xdata[i] = (float)(10.0 / (length - 1) * i - 5);
                // ch1Volts[i] = rd.Next(-500, 500);
                // ch2Volts[i] = rd.Next(-500, 500);

                ch1Xdata[i] /= 1e6f;
                ch2Xdata[i] /= 1e6f;
                ch1Volts[i] /= 1e3f;
                ch2Volts[i] /= 1e3f;
            }

            _view.wavechart.Refresh();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void StartSampleTask()
    {
        Task.Run(() =>
        {
            while (isRun)
            {
                SampleData();
                Thread.Sleep(100);
            }
        });
    }

    private void ProcessMeasureTask()
    {
        Task.Run(() =>
        {
            while (isRun)
            {
                try
                {
                    string timeunit = Regex.Replace(QL6747Param.TimeSpanArr[timeDivIndex], "[0-9]", "",
                        RegexOptions.IgnoreCase);
                    double timeScale = GetTimeScale(timeunit);

                    string voltunit = Regex.Replace(
                        QL6747Param.VoltDivArr[chSource == 0 ? voltDivIndex : voltDivIndex1],
                        "[0-9]", "",
                        RegexOptions.IgnoreCase);
                    double voltScale = GetTimeScale(voltunit);

                    float[] xdata = new float[ch1Xdata.Length];
                    float[] ydata = new float[ch1Xdata.Length];

                    Array.Copy(ch1Xdata, 0, xdata, 0, xdata.Length);
                    Array.Copy(chSource == 0 ? ch1Volts : ch2Volts, 0, ydata, 0, ydata.Length);

                    for (int i = 0; i < ch1Xdata.Length; i++)
                    {
                        xdata[i] *= 1e6f;
                        ydata[i] *= 1e3f;
                        xdata[i] /= (float)timeScale;
                        ydata[i] /= (float)voltScale;
                    }

                    float amp = 0,
                        positivePulseWidth = 0,
                        negativePulseWidth = 0,
                        positiveDutyCycle = 0,
                        negativDutyCycle = 0,
                        risingEdgeTime = 0,
                        fallingEdgeTime = 0,
                        osVolt = 0,
                        pulseTopUnev = 0,
                        pulseSequeceUnev = 0;
                    PXIe6747.CalPulse(xdata, ydata, (uint)ydata.Length, ref amp,
                        ref positivePulseWidth, ref negativePulseWidth,
                        ref positiveDutyCycle, ref negativDutyCycle,
                        ref risingEdgeTime, ref fallingEdgeTime,
                        ref osVolt, ref pulseTopUnev, ref pulseSequeceUnev);

                    double max = ydata.Max() * voltScale;
                    double min = ydata.Min() * voltScale;
                    double ffVal = Math.Abs(max - min);
                    double ac = PXIe6747.CalAcVaildValue(ydata, (uint)ydata.Length);
                    double dc = PXIe6747.CalDcVaildValue(ydata, (uint)ydata.Length);

                    MeasureCollection[chSource].Max = FormatYvalue(max);
                    MeasureCollection[chSource].Min = FormatYvalue(min);
                    MeasureCollection[chSource].FFVal = FormatYvalue(ffVal);
                    MeasureCollection[chSource].Ac = FormatYvalue(ac * voltScale);
                    MeasureCollection[chSource].Dc = FormatYvalue(dc * voltScale);
                    MeasureCollection[chSource].PositiveCircle = positiveDutyCycle.ToString("F2") + "%";
                    MeasureCollection[chSource].NegativeCircle = negativDutyCycle.ToString("F2") + "%";
                    MeasureCollection[chSource].PositivePulseWidth = FormatXvalue(positivePulseWidth * timeScale);
                    MeasureCollection[chSource].NegativePulseWidth = FormatXvalue(negativePulseWidth * timeScale);
                    MeasureCollection[chSource].RisingEdgeTime = FormatYvalue(risingEdgeTime * voltScale);
                    MeasureCollection[chSource].FallingEdgeTime = FormatYvalue(fallingEdgeTime * voltScale);
                    MeasureCollection[chSource].OsVolt = FormatYvalue(osVolt * voltScale);
                    MeasureCollection[chSource].PulseTopUnev = FormatYvalue(pulseTopUnev * voltScale);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(500);
            }
        });
    }

    public static double GetTimeScale(string timeUnit)
    {
        double scale = 1;
        switch (timeUnit.ToLower())
        {
            case "ps":
                scale = 1;
                break;
            case "ns":
                scale = 1e3;
                break;
            case "us":
                scale = 1e6;
                break;
            case "ms":
                scale = 1e9;
                break;
            case "s":
                scale = 1e12;
                break;
        }

        return scale;
    }

    /// <summary>
    /// y数据格式化
    /// </summary>
    /// <param name="uv"></param>
    /// <returns></returns>
    public static string FormatYvalue(double uv)
    {
        string formatY = uv.ToString("F3") + "uV";
        if (Math.Abs(uv) / 1e6 >= 1)
        {
            formatY = (uv / 1e6).ToString("F3") + "V";
        }
        else if (Math.Abs(uv) / 1e3 >= 1)
        {
            formatY = (uv / 1e3).ToString("F3") + "mV";
        }

        return formatY;
    }

    /// <summary>
    /// x数据格式化
    /// </summary>
    /// <param name="ps"></param>
    /// <returns></returns>
    public static string FormatXvalue(double ps, string format = "F3")
    {
        string formatX = ps.ToString(format) + "pS";
        if (Math.Abs(ps) / 1e12 >= 1)
        {
            formatX = (ps / 1e12).ToString(format) + "S";
        }
        else if (Math.Abs(ps) / 1e9 >= 1)
        {
            formatX = (ps / 1e9).ToString(format) + "mS";
        }
        else if (Math.Abs(ps) / 1e6 >= 1)
        {
            formatX = (ps / 1e6).ToString(format) + "uS";
        }
        else if (Math.Abs(ps) / 1e3 >= 1)
        {
            formatX = (ps / 1e3).ToString(format) + "nS";
        }

        return formatX;
    }
}
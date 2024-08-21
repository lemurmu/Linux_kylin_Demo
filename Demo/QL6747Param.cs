namespace QLingScope;

public class QL6747Param
{
    //时基
    public static readonly string[] TimeSpanArr =
    {
        "1nS", "2nS", "5nS",
        "10nS", "20nS", "50nS",
        "100nS", "200nS", "500nS",
        "1uS", "2uS", "5uS",
        "10uS", "20uS", "50uS",
        "100uS", "200uS", "500uS",
        "1mS", "2mS", "5mS",
        "10mS", "20mS", "50mS",
        "100mS", "200mS", "500mS",
        "1S", "2S", "5S",
        "10S"
    };

    //换成ps的时基
    public static readonly double[] MinTimeDivArr =
    {
        1e3, 2e3, 5e3,
        10e3, 20e3, 50e3,
        100e3, 200e3, 500e3,
        1e6, 2e6, 5e6,
        10e6, 20e6, 50e6,
        100e6, 200e6, 500e6,
        1e9, 2e9, 5e9,
        10e9, 20e9, 50e9,
        100e9, 200e9, 500e9,
        1e12, 2e12, 5e12,
        10e12
    };

    //高阻1M欧姆的档位
    public static readonly string[] VoltDivArr =
    {
        "10mV", "20mV", "50mV",
        "100mV", "200mV", "500mV",
        "1V", "2V", "5V",
        "10V"
    };
    
    //高阻阻最小单位档位uV
    public static readonly double[] MinVoltDivArr =
    {
        10e3, 20e3, 50e3,
        100e3, 200e3, 500e3,
        1e6, 2e6, 5e6,
        10e6
    };

    //1G:1nS,5uS/div; 500M:2nS,10uS/div ;250M:4ns,20uS/div
    public static long SampleRate = 250000000;//Hz采样率 500M采样率 小于10uS的都为500采样率 大于10uS用 1/(10*timediv/10k)计算
    public static long FpgaFs = 250000000;
    public static int RealTimeDivIndex = 15;//实际数据刚好的档位索引 1G:1nS,5uS/div; 500M:2nS,10uS/div ;250M:4ns,20uS 
    public static int _PerPointTime = 2000;//2ns=2000ps 每个点实际间隔时间间隔
    public static double MaxFullBandWidth = 100e6;//最大全带宽
}
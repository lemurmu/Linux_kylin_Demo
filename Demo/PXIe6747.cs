using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QLingScope;

/// <summary>
/// 操作硬件Dll导入类
/// </summary>
public static class PXIe6747
{
    private const string dllName = "libPXIe6747.so";

    /// <summary>
    /// 初始化,查找所有槽号
    /// </summary>
    /// <param name="slots"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LoadDevice(byte[] slots);

    /// <summary>
    /// 关闭模块 释放内存
    /// </summary>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CloseDevice(byte slot);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void pcie_deinit();

    /// <summary>
    /// 获取模块是否已经运行的标志
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool GetModeIsRuning(byte slot);

    /// <summary>
    /// 设置模块已经运行的标志
    /// </summary>
    /// <param name="slot"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetModeIsRuning(byte slot, int state);

    /// <summary>
    /// 初始化配置寄存器
    /// </summary>
    /// <returns>1：配置成功 -1：ADC1偏移寄存器配置出错 -2：ADC2偏移寄存器配置出错</returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int InitConfig(byte slot);

    /// <summary>
    /// 读取序列号
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="sn"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReadDeviceSn(byte slot, ref int sn);

    /// <summary>
    /// 配置ADC1
    /// </summary>
    /// <param name="slot"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetADC1(byte slot, int value);

    /// <summary>
    /// 配置ADC2
    /// </summary>
    /// <param name="slot"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetADC2(byte slot, int value);

    /// <summary>
    /// 设置增益校准系数k
    /// </summary>
    /// <param name="coefficient"></param>
    /// <param name="length"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetGainCalibrateK(double[] coefficient, uint length); //设置增益校准系数k

    /// <summary>
    /// 读取增益校准系数k
    /// </summary>
    /// <param name="coefficient"></param>
    /// <param name="length"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetGainCalibrateK(double[] coefficient, uint length);

    /// <summary>
    /// /设置偏移校准系数k
    /// </summary>
    /// <param name="coefficient"></param>
    /// <param name="length"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetOffsetCalibrateK(double[] coefficient, uint length); //设置偏移校准系数k

    /// <summary>
    /// 设置偏移校准系数b
    /// </summary>
    /// <param name="coefficient"></param>
    /// <param name="length"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetOffsetCalibrateB(double[] coefficient, uint length); //设置偏移校准系数b

    /// <summary>
    /// 烧写最大采样率
    /// </summary>
    /// <param name="slot"></param>
    /// <param name=""></param>
    /// <param name="fs"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int WriteSampleRate(byte slot, long fs);

    /// <summary>
    /// 烧写最大全带宽
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="bandwidth"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int WriteMaxFullBandWidth(byte slot, double bandwidth);

    /// <summary>
    /// 设置时基 pS/div
    /// </summary>
    /// <param name="timeDiv"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetTimeDiv(byte slot, double timeDiv);

    /// <summary>
    /// 设置时钟参考
    /// </summary>
    /// <param name="model"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetTimeRefer(byte slot, int model);

    /// <summary>
    /// 读取时基
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetTimeDiv(byte slot);

    /// <summary>
    /// 设置延迟
    /// </summary>
    /// <param name="timeDelay">延迟pS</param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTimeDelay(byte slot, double timeDelay);

    /// <summary>
    /// 读取延迟
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetTimeDelay(byte slot);

    /// <summary>
    /// 设置水平偏移点数
    /// </summary>
    /// <param name="points"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTimeOffset(byte slot, uint points);

    /// <summary>
    /// 设置挡位
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="voltDiv"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetVoltDiv(byte slot, int channel, long voltDiv);

    /// <summary>
    /// 读取档位
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern long GetVoltDiv(byte slot, int channel);

    /// <summary>
    /// 设置触发电平
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="volt"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTriggerVolt(byte slot, double volt);

    /// <summary>
    /// 设置阻抗
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="ImpedType"></param>
    /// <param name="voltDiv"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetImped(byte slot, int channel, int ImpedType, long voltDiv);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetImped(byte slot, int channel);

    /// <summary>
    /// 设置偏移
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="voltOffset"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetVoltOffset(byte slot, int channel, double voltOffset);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetVoltOffset(byte slot, int channel);

    /// <summary>
    /// 设置耦合方式
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="coup"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetCouplingMode(byte slot, int channel, int coup);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetCouplingMode(byte slot, int channel);

    /// <summary>
    /// 设置带宽
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="bandwidth">带宽Hz</param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetBandWidth(byte slot, int channel, double bandwidth);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetBandWidth(byte slot, int channel);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Read_User(byte slot, int addr, ref int val);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Write_User(byte slot, int addr, int val);

    /// <summary>
    /// XDMA读取
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="fpga_ddr_addr"></param>
    /// <param name="buffer"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Read_XDMA(byte slot, int fpga_ddr_addr, byte[] buffer, uint len);

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="ch1Xdata"></param>
    /// <param name="ch2Xdata"></param>
    /// <param name="ch1Volts"></param>
    /// <param name="ch2Volts"></param>
    /// <param name="length"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReadData(byte slot, float[] ch1Xdata, float[] ch2Xdata, float[] ch1Volts,
        float[] ch2Volts, ref int length, uint timeOut, ref double offsetTime);

    /// <summary>
    /// 单次读取
    /// </summary>
    /// <param name="slot">槽号</param>
    /// <param name="ch1Xdata">通道1时间轴数据pS 初始化大小为50000</param>
    /// <param name="ch2Xdata">通道2时间轴数据pS 初始化大小为50000</param>
    /// <param name="ch1Volts">通道1电平值uV 初始化大小为50000</param>
    /// <param name="ch2Volts">通道2电平值uV 初始化大小为50000</param>
    /// <param name="length">有效数据长度 初始化为50000</param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReadSingle(byte slot, float[] ch1Xdata, float[] ch2Xdata, float[] ch1Volts,
        float[] ch2Volts, ref int length, ref double offsetTime, uint timeOut);

    /// <summary>
    /// XDMA读取
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="fpga_ddr_addr"></param>
    /// <param name="buffer"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Read_XDMA_Ex(byte slot, long fpga_ddr_addr, byte[] buffer, uint len);

    /// <summary>
    /// XDMA写入
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="fpga_ddr_addr"></param>
    /// <param name="buffer"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Write_XDMA(byte slot, int fpga_ddr_addr, byte[] buffer, uint len);


    /// <summary>
    /// 获取硬件通道数
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetChannelNum();

    /// <summary>
    /// 硬件单通道最大采样率
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern long GetOneChannelMaxSampleRate(byte slot);

    /// <summary>
    /// 读取采样率
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern long GetSampleRate(byte slot, double timeDiv);

    /// <summary>
    /// 获取最小时间刻度 ps/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetMinTimeDiv(byte slot);

    /// <summary>
    /// 获取最大时间刻度 ps/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetMaxTimeDiv(byte slot);

    /// <summary>
    /// 获取高阻最大档位 uV/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetHighImpedMaxVoltDiv(byte slot);

    /// <summary>
    /// 获取高阻最小档位 uV/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetHighImpedMinVoltDiv(byte slot);

    /// <summary>
    /// 获取低阻最大档位 uV/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetLowImpedMaxVoltDiv(byte slot);

    /// <summary>
    /// 获取低阻最小档位 uV/div
    /// </summary>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetLowImpedMinVoltDiv(byte slot);

    /// <summary>
    /// 获取高阻最大垂直偏移
    /// </summary>
    /// <param name="voltIndex">档位索引，第几档</param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetHighImpedMaxVoltOffset(byte slot, uint voltIndex);

    /// <summary>
    /// 获取低阻最大垂直偏移
    /// </summary>
    /// <param name="voltIndex">档位索引，第几档</param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetLowImpedMaxVoltOffset(byte slot, uint voltIndex);

    /// <summary>
    /// 设置单通道最大采样率
    /// </summary>
    /// <param name="fs"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetOneChannelMaxSampleRate(double fs);

    /// <summary>
    /// 设置单通道最大存储深度
    /// </summary>
    /// <param name="depth"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetOneChannelMaxSampleDepth(double depth);

    /// <summary>
    /// 设置触发源 
    /// <param name="channel">选择模拟触发通道号 0：通道1;2：通道2</param>
    /// <param name="mode">触发方式 mode=0:模拟触发 mode=1：外部触发 mode=2:软件触发</param>
    /// </summary>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetTriggerSource(byte slot, int channel, int trigway, byte mode);

    /// <summary>
    /// 设置触发模式（a = 0：上升沿；a = 1：下降沿；a = 2：边沿）
    /// </summary>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetTriggerMode(byte mode);

    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetTriggerVolt(byte slot);

    //-------------------------------------------参数测量--------------------------------------------------------

    /// <summary>
    /// 计算最大值
    /// </summary>
    /// <param name="volts"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalcuMax(float[] volts, uint length);

    /// <summary>
    /// 计算最小值
    /// </summary>
    /// <param name="volts"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalcuMin(float[] volts, uint length);

    /// <summary>
    /// 计算峰峰值
    /// </summary>
    /// <param name="volts"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalcFFValue(float[] volts, uint length);

    /// <summary>
    /// 计算平均值
    /// </summary>
    /// <param name="volts"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalcAvg(float[] volts, uint length);

    /// <summary>
    /// 计算标准差
    /// </summary>
    /// <param name="volts"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalAcVaildValue(float[] volts, uint length);

    /// <summary>
    /// 计算中位数
    /// </summary>
    /// <param name="volts"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalcMedian(float[] volts, uint length);

    /// <summary>
    /// 计算均方根
    /// </summary>
    /// <param name="volts"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float CalDcVaildValue(float[] volts, uint length);

    /// <summary>
    /// 计算脉冲宽度和过冲电压差
    /// </summary>
    /// <param name="times">时间</param>
    /// <param name="volts">电平</param>
    /// <param name="length">数据长度</param>
    /// <param name="pulseWidth">脉宽</param>
    /// <param name="osVolt">过冲电压差</param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CalPulse(float[] times, float[] volts, uint length, ref float amp,
        ref float positivePulseWidth, ref float negativePulseWidth,
        ref float positiveDutyCycle, ref float negativDutyCycle, ref float risingEdgeTime, ref float fallingEdgeTime,
        ref float osVolt, ref float pulseTopUnev, ref float pulseSequeceUnev);


    /// <summary>
    /// 计算脉冲频率和周期
    /// </summary>
    /// <param name="real">实部</param>
    /// <param name="imag">虚部</param>
    /// <param name="length">数据长度</param>
    /// <param name="fs">采样率Hz</param>
    /// <param name="fc">频率</param>
    /// <param name="t">周期</param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CalFrequecyAndPeriod(float[] volts, uint length, double fs, ref double fc, ref double t);

    /// <summary>
    /// 计算相位差和延迟
    /// </summary>
    /// <param name="ch1Volts"></param>
    /// <param name="ch1Voltslength"></param>
    /// <param name="ch2Volts"></param>
    /// <param name="ch2Voltslength"></param>
    /// <param name="fs"></param>
    /// <param name="phase"></param>
    /// <param name="delay"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CalPhaseDiff(float[] ch1Volts, uint ch1Voltslength, float[] ch2Volts, uint ch2Voltslength,
        double fs, ref double ch2diffch1, ref double ch1diffch2);

    /// <summary>
    /// 计算幅度差
    /// </summary>
    /// <param name="ch1Volts"></param>
    /// <param name="ch1Voltslength"></param>
    /// <param name="ch2Volts"></param>
    /// <param name="ch2Voltslength"></param>
    /// <param name="ch2diffch1"></param>
    /// <param name="ch1diffch2"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CalAmpDiff(float[] ch1Volts, uint ch1Voltslength, float[] ch2Volts, uint ch2Voltslength,
        ref double ch2diffch1, ref double ch1diffch2);

    /// <summary>
    /// 计算峰值和次峰值
    /// </summary>
    /// <param name="volts"></param>
    /// <param name="length"></param>
    /// <param name="peak"></param>
    /// <param name="secondPeak"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CalPeak(float[] volts, uint length, ref double peak, ref double secondPeak);

    /// <summary>
    /// 将校准系数写入FPGA
    /// </summary>
    /// <param name="slot">槽号</param>
    /// <param name="coeffcientBytes">校准字节流(用float）</param>
    /// <param name="length">字节长度</param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void WriteCalibrateCoeffToFlash(byte slot, byte[] coeffcientBytes, uint length);

    /// <summary>
    /// 从FPGA读取校准数据
    /// </summary>
    /// <param name="slot"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ReadCalibrateCoeffFromFlash(byte slot);

    /// <summary>
    /// 读取校准状态
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte ReadCalibrateStatus(byte slot);

    /// <summary>
    /// 写入校准状态 0:未校准 1：校准
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="state"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void WriteCalibrateStatus(byte slot, byte state);

    /// <summary>
    /// 读取开机次数
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ReadStartUpTimes(byte slot);

    /// <summary>
    /// 获取最大全带宽
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern double GetMaxFullBandWidth(byte slot);


    //-------------------派格测控API-------------------
    /// <summary>
    /// 开始采集
    /// </summary>
    /// <param name="slot"></param>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Start(byte slot);

    /// <summary>
    /// 只读取波形数据
    /// </summary>
    /// <param name="slot">槽号</param>
    /// <param name="ch1Xdata">通道1时间轴数据pS 初始化大小为50000</param>
    /// <param name="ch2Xdata">通道2时间轴数据pS 初始化大小为50000</param>
    /// <param name="ch1Volts">通道1电平值uV 初始化大小为50000</param>
    /// <param name="ch2Volts">通道2电平值uV 初始化大小为50000</param>
    /// <param name="length">有效数据长度 初始化为50000</param>
    ///  <param name="timeout"></param>
    /// <returns>-100：超时</returns>
    [DllImport(dllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReadWaveform(byte slot, float[] ch1Xdata, float[] ch2Xdata, float[] ch1Volts,
        float[] ch2Volts, ref int length, uint timeout);
    //--------------------------------------------------
}
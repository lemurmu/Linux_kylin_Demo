using QLingScope.ViewModels;

namespace QLingScope;

public class Measure : ViewModelBase
{
    private string chName;

    public string ChName
    {
        get { return chName; }
        set { SetProperty(ref chName, value); }
    }

    private string max;

    public string Max
    {
        get { return max; }
        set { SetProperty(ref max, value); }
    }

    private string min;

    public string Min
    {
        get { return min; }
        set { SetProperty(ref min, value); }
    }

    private string ffvalue;

    public string FFVal
    {
        get { return ffvalue; }
        set { SetProperty(ref ffvalue, value); }
    }

    private string ac;

    public string Ac
    {
        get { return ac; }
        set { SetProperty(ref ac, value); }
    }

    private string dc;

    public string Dc
    {
        get { return dc; }
        set { SetProperty(ref dc, value); }
    }

    private string negativeCircle;

    public string NegativeCircle
    {
        get { return negativeCircle; }
        set { SetProperty(ref negativeCircle, value); }
    }

    private string positiveCircle;

    public string PositiveCircle
    {
        get { return positiveCircle; }
        set { SetProperty(ref positiveCircle, value); }
    }

    private string negativePulseWidth;

    public string NegativePulseWidth
    {
        get { return negativePulseWidth; }
        set { SetProperty(ref negativePulseWidth, value); }
    }

    private string positivePulseWidth;

    public string PositivePulseWidth
    {
        get { return positivePulseWidth; }
        set { SetProperty(ref positivePulseWidth, value); }
    }
    
    private string risingEdgeTime;

    public string RisingEdgeTime
    {
        get { return risingEdgeTime; }
        set { SetProperty(ref risingEdgeTime, value); }
    }
    
    private string fallingEdgeTime;

    public string FallingEdgeTime
    {
        get { return fallingEdgeTime; }
        set { SetProperty(ref fallingEdgeTime, value); }
    }
    
    private string osVolt;

    public string OsVolt
    {
        get { return osVolt; }
        set { SetProperty(ref osVolt, value); }
    }
    
    private string pulseTopUnev;

    public string PulseTopUnev
    {
        get { return pulseTopUnev; }
        set { SetProperty(ref pulseTopUnev, value); }
    }
}
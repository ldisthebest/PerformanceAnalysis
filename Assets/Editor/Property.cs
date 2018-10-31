
public class Property{

    private string name;
    private float minValue;
    private float maxValue;
    private float averageValue;
    protected string result;

    private double sum;
    protected int frameCount;


    public string Name
    {
        get { return name; }
    }

    public float MinValue
    {
        get { return minValue; }
    }

    public float MaxValue
    {
        get { return maxValue; }
    }

    public float AverageValue
    {
        get { return averageValue; }
    }

    public string Result
    {
        get { return result; }
    }

    public int FrameCount
    {
        get { return frameCount; }
    }


    public Property(string nameType)
    {
        name = nameType;
        frameCount = 0;
        sum = 0;
    }

    public virtual void UpdateValueEveryFrame(int frame,float frameValue)
    {
        if (frame == Analysis.firstFrame)//对于第一帧的情况
        {
            minValue = frameValue;
            maxValue = frameValue;
        }
        else
        {
            minValue = frameValue < minValue ? frameValue : minValue;
            maxValue = frameValue > maxValue ? frameValue : maxValue;
        }
        frameCount++;
        sum += frameValue;
       
    }

    protected void CaculateAverageValue()
    {
        averageValue = (float)(sum / frameCount);
    }

    public virtual void MakeResult(PropertyData standardData)
    {
        CaculateAverageValue();

        if (standardData.LimitValue == -1)
        {
            result = "不确定";
        }
        else if((maxValue <= standardData.LimitValue && standardData.Compare == CompareType.LessThan)||
            (averageValue >= standardData.LimitValue && standardData.Compare == CompareType.GreaterThan))
        {
            result = "合格";
        }
        else
        {
            result = "不合格";
        }

    }

}

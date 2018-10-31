
public class RateProperty : Property {

    private PropertyData standardValue;
    private float qulifiedFrame;

	public RateProperty(string nameType,PropertyData limitValue) : base(nameType)
    {
        standardValue = limitValue;
        qulifiedFrame = 0;
    }
    public override void UpdateValueEveryFrame(int frame, float frameValue)
    {
        base.UpdateValueEveryFrame(frame, frameValue);

        if ((standardValue.Compare == CompareType.GreaterThan && frameValue >= standardValue.LimitValue)
        || (standardValue.Compare == CompareType.LessThan && frameValue <= standardValue.LimitValue))
        {
            qulifiedFrame++;
        }
    }

    public override void MakeResult(PropertyData standardData)
    {
        //base.MakeResult(standardData);
        CaculateAverageValue();
        float rate = qulifiedFrame / frameCount;
        if(standardData.LimitValue == -1)
        {
            result = "unknow";
        }
        else if((standardData.Compare == CompareType.GreaterThan && rate >= standardData.LimitValue)||
            (standardData.Compare == CompareType.LessThan && rate <= standardData.LimitValue))
        {
            result = "合格";
        }
        else
        {
            result = "不合格";
        }
    }
}


public enum CompareType
{
    LessThan = 1,
    GreaterThan = 2
}

public class PropertyData
{
    public float LimitValue;
    public CompareType Compare;

    public PropertyData(float value, CompareType compare)
    {
        LimitValue = value;
        Compare = compare;
    }
}

public class BaseStandardData 
{
    public PropertyData FPS;
    public PropertyData FPSRate;
    public PropertyData CPU;
    public PropertyData CPURate;
   
    public PropertyData DrawCalls;
    public PropertyData Tris;

    public PropertyData MonoMemory;
    public PropertyData TextureMemory;
    public PropertyData MeshMemory;
    public PropertyData AnimationMemory;
    public PropertyData AudioMemory;
    public PropertyData GameObjectCount;

    public PropertyData CpuRenderingTime;
    public PropertyData CpuScriptsTime;
    public PropertyData CpuAnimationTime;
    public PropertyData CpuUiTime;

    public BaseStandardData()
    {
        //初始化默认值,都是-1意味着不确定，就是测试者不知道标准到底是多少
        FPS = new PropertyData(-1, CompareType.GreaterThan);
        FPSRate = new PropertyData(-1, CompareType.GreaterThan);
        CPU = new PropertyData(-1, CompareType.GreaterThan);
        CPURate = new PropertyData(-1, CompareType.GreaterThan);

        
        DrawCalls = new PropertyData(-1, CompareType.GreaterThan);
        Tris = new PropertyData(-1, CompareType.GreaterThan);

        MonoMemory = new PropertyData(-1, CompareType.GreaterThan);
        TextureMemory = new PropertyData(-1, CompareType.GreaterThan);
        MeshMemory = new PropertyData(-1, CompareType.GreaterThan);
        AnimationMemory = new PropertyData(-1, CompareType.GreaterThan);
        AudioMemory = new PropertyData(-1, CompareType.GreaterThan);
        GameObjectCount = new PropertyData(-1, CompareType.GreaterThan);

        CpuRenderingTime = new PropertyData(-1, CompareType.GreaterThan);
        CpuScriptsTime = new PropertyData(-1, CompareType.GreaterThan);
        CpuAnimationTime = new PropertyData(-1, CompareType.GreaterThan);
        CpuUiTime = new PropertyData(-1, CompareType.GreaterThan);

    }

}

public class PCStandardData : BaseStandardData
{
    public PCStandardData()
    {
        FPS = new PropertyData(25, CompareType.GreaterThan);
        FPSRate = new PropertyData(0.9f, CompareType.GreaterThan);
        CPU = new PropertyData(0.6f, CompareType.LessThan);
        CPURate = new PropertyData(0.9f, CompareType.GreaterThan);
        MonoMemory = new PropertyData(50, CompareType.LessThan);
        DrawCalls = new PropertyData(250, CompareType.LessThan);
        Tris = new PropertyData(100000, CompareType.LessThan);
        //TextureMemory = new PropertyData(-1, CompareType.LessThan);
        MeshMemory = new PropertyData(20, CompareType.LessThan);
        AnimationMemory = new PropertyData(15, CompareType.LessThan);
        AudioMemory = new PropertyData(15, CompareType.LessThan);
        GameObjectCount = new PropertyData(10000, CompareType.LessThan);
    }  
}

public class AndriodStandardData : BaseStandardData
{
    public AndriodStandardData()
    {
        FPS = new PropertyData(25, CompareType.GreaterThan);
        FPSRate = new PropertyData(0.9f, CompareType.GreaterThan);
        CPU = new PropertyData(0.6f, CompareType.LessThan);
        CPURate = new PropertyData(0.9f, CompareType.GreaterThan);
        MonoMemory = new PropertyData(50, CompareType.LessThan);
        DrawCalls = new PropertyData(250, CompareType.LessThan);
        Tris = new PropertyData(100000, CompareType.LessThan);
        TextureMemory = new PropertyData(50, CompareType.LessThan);
        MeshMemory = new PropertyData(20, CompareType.LessThan);
        AnimationMemory = new PropertyData(15, CompareType.LessThan);
        AudioMemory = new PropertyData(15, CompareType.LessThan);
        GameObjectCount = new PropertyData(10000, CompareType.LessThan);
    }
}


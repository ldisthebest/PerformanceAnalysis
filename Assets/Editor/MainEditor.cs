using UnityEngine;
using UnityEditor;
using UnityEditorInternal;



public class MainEditor : EditorWindow
{
    public const string ANDRIOD_PLATFORM = "Andriod Statistics";
    public const string PC_PLATFORM = "PC Statistics";

    bool analysis = false;
    bool over = false;
    public static string labelTitle = "";
    public static int firstFrame;
    int continuousFrame;

    [MenuItem("性能分析/Andriod")]
    static void AndriodAnalysis()
    {

        Debug.Log(labelTitle + firstFrame);
        ProfilerDriver.enabled = true;
        labelTitle = ANDRIOD_PLATFORM;
 
        GetWindow<MainEditor>();
        
    }

    [MenuItem("性能分析/PC")]
    static void PCAnalysis()
    {
        ProfilerDriver.enabled = true;
        labelTitle = PC_PLATFORM;
        GetWindow<MainEditor>();
    }

    void OnGUI()
    {
        GUILayout.Label(labelTitle);

        //if (GUILayout.Button("Test"))
        //{
        //    Debug.Log(continuousFrame);

        //}
        if (over)
        {
            GUILayout.Label("Done!");
            return;
        } 
        if(!analysis)
        {
            if (GUILayout.Button("begin"))
            {
                analysis = true;
                continuousFrame = ProfilerDriver.lastFrameIndex;               
                firstFrame = ProfilerDriver.firstFrameIndex;//先获得第一帧再进行统计
                DataStatistics.Instance.UpdateProperties();
            }
        }
        else
        {
            GUILayout.Label("正在检测统计性能数据...");
            if (GUILayout.Button("end"))
            {
                over = true;
                analysis = false;
                DataStatistics.Instance.UpdateProperties();
                DataStatistics.Instance.ExportTxtResult();
                CleanUp();
            }
        }
               
    }

    void Update()
    {
        if (!analysis) return;
        if (ProfilerDriver.firstFrameIndex >= continuousFrame - 5)//间隔大约5帧的范围，为的是保证统计的区间段能连起来
        {
            DataStatistics.Instance.UpdateProperties();
            continuousFrame = ProfilerDriver.lastFrameIndex;
        }
    }

    void CleanUp()//每次测试完数据需要清空静态数据
    {
        labelTitle = "";
        firstFrame = 0;
    }


}



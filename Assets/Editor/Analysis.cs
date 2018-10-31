using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;
using System.IO;



public class Analysis : EditorWindow {

    bool analysis = false;
    static string labelTitle = "";
    public static int firstFrame;
    int continuousFrame;

    Property monoProperty = new Property("Mono");
    Property textureProperty = new Property("Texture");
    Property animationProperty = new Property("Animation");
    Property audioProperty = new Property("Audio");
    Property meshProperty = new Property("Mesh");
    Property gameobjectProperty = new Property("GameObject");
    Property drawCallProperty = new Property("DrawCalls");
    Property trisProperty = new Property("Tris");
    Property fpsProperty = new Property("FPS");

   // ProfilerProperty profilerProperty;//用于获取CPU、FPS等数据

    [MenuItem("Analysis/Andriod1")]
    static void AndriodAnalysis()
    {
        ProfilerDriver.enabled = true;
        labelTitle = "Andriod";
        GetWindow<Analysis>();
    }
    [MenuItem("Analysis/PC")]
    static void PCAnalysis()
    {
        ProfilerDriver.enabled = true;
        labelTitle = "PC";
        GetWindow<Analysis>();
    }

    [MenuItem("Analysis/IOS")]
    static void IOSAnalysis()
    {
        ProfilerDriver.enabled = true;
        labelTitle = "IOS";
        GetWindow<Analysis>();
    }



    void OnGUI()
    {
        GUILayout.BeginVertical();
        //GUILayout.Label(labelTitle);
        if(GUILayout.Button("begin"))
        {
            analysis = true;
            //profilerProperty = new ProfilerProperty();
            continuousFrame = ProfilerDriver.lastFrameIndex;
            GetInfo(); //开始时统计一个区间
            firstFrame = ProfilerDriver.firstFrameIndex;
        }
        if(GUILayout.Button("end"))
        {
            analysis = false;
            GetInfo();//结束时统计一个区间
            //测试2
            //Debug.Log("max:" + monoProperty.MaxValue);
            //Debug.Log("min:" + monoProperty.MinValue);
            //Debug.Log("average:" + monoProperty.AverageValue);

        }
    }

    void Update()
    {
        if (!analysis) return;
        if(ProfilerDriver.firstFrameIndex >= continuousFrame-5)//间隔大约5帧的范围，为的是保证统计的区间段能连起来
        {
            GetInfo();
            continuousFrame = ProfilerDriver.lastFrameIndex;
        }
    }

    void GetInfo()
    {
       int firstFrameIndex = ProfilerDriver.firstFrameIndex;
       if (firstFrameIndex == -1) return;
       int lastFrameIndex = ProfilerDriver.lastFrameIndex;

      
       

       for(int frame = firstFrameIndex; frame <= lastFrameIndex; frame++)//每一帧
       {
            string MemoryAllInfo = ProfilerDriver.GetOverviewText(ProfilerArea.Memory, frame);

            float frameMonoValue = StringFormat.GetMonoMemory(MemoryAllInfo);
            monoProperty.UpdateValueEveryFrame(frame, frameMonoValue);

            float frameTextureValue = StringFormat.GetTextureMemory(MemoryAllInfo);
            textureProperty.UpdateValueEveryFrame(frame, frameTextureValue);

            float frameAnimationValue = StringFormat.GetAnimationMemory(MemoryAllInfo);
            animationProperty.UpdateValueEveryFrame(frame, frameAnimationValue);

            float frameAudioValue = StringFormat.GetAudioMemory(MemoryAllInfo);
            audioProperty.UpdateValueEveryFrame(frame, frameAudioValue);

            float frameMeshValue = StringFormat.GetMeshMemory(MemoryAllInfo);
            meshProperty.UpdateValueEveryFrame(frame, frameMeshValue);

            float frameGameObjectValue = StringFormat.GameObjectCount(MemoryAllInfo);
            gameobjectProperty.UpdateValueEveryFrame(frame, frameGameObjectValue);

            string RendeAllInfo = ProfilerDriver.GetOverviewText(ProfilerArea.Rendering, frame);

            float frameDrawCallValue = StringFormat.GetDrawCalls(RendeAllInfo);
            drawCallProperty.UpdateValueEveryFrame(frame, frameDrawCallValue);

            float frameTrisValue = StringFormat.GetTrisCount(RendeAllInfo);
            trisProperty.UpdateValueEveryFrame(frame, frameTrisValue);


            //profilerProperty.SetRoot(frame,ProfilerColumn.TotalTime,ProfilerViewType.Hierarchy);
            //string frameFpsValue = profilerProperty.frameFPS;
            //fpsProperty.UpdateValueEveryFrame(frame, float.Parse(frameFpsValue));
           // profilerProperty.Cleanup();



        }
        Debug.Log("first:" + firstFrameIndex + "," + "last:" + lastFrameIndex + ",Diff:" + (lastFrameIndex - firstFrameIndex));
    }

    void FinalCaculate()
    {
        if(labelTitle == "Andriod")
        {
            //monoProperty.MakeResult(AndriodStandardData.MonoMemory);
        }
        else if(labelTitle == "PC")
        {
           // monoProperty.MakeResult(PCStandardData.MonoMemory);

        }
        else if(labelTitle == "IOS")
        {

        }
    }


    //void GetInfo()
    //{
    //    int firstFrameIndex = ProfilerDriver.firstFrameIndex;
    //    int lastFrameIndex = ProfilerDriver.lastFrameIndex;
    //    ProfilerColumn totalTime = ProfilerColumn.TotalTime;
    //    ProfilerViewType type = ProfilerViewType.Hierarchy;

   
    //    for(int frameIndex = firstFrameIndex; frameIndex <= firstFrameIndex; frameIndex++)
    //    {
    //        ProfilerProperty profilerProperty = new ProfilerProperty();
    //        profilerProperty.SetRoot(frameIndex, totalTime, type);
            
    //        profilerProperty.onlyShowGPUSamples = false;
    //        //bool enterChildren = true;

    //        //while (profilerProperty.Next(true))
    //        //{
    //        //    //string name = profilerProperty.GetColumn(ProfilerColumn.FunctionName);
    //        //    //string time = profilerProperty.GetColumn(ProfilerColumn.TotalTime);
    //        //    //Debug.Log("name:" + name + "," + "time:" + time);
    //        //    Debug.Log("propertyName:" + profilerProperty.propertyName); 
    //        //}

            
    //        //Debug.Log("FPS:" + profilerProperty.frameFPS);

    //        Debug.Log("memory info:" + ProfilerDriver.GetOverviewText(ProfilerArea.Memory, frameIndex));
    //        Debug.Log("Rendering info:" + ProfilerDriver.GetOverviewText(ProfilerArea.Rendering, frameIndex));
    //        Debug.Log("CPU info:" + ProfilerDriver.GetOverviewText(ProfilerArea.CPU, frameIndex));
    //        Debug.Log("GPU info:" + ProfilerDriver.GetOverviewText(ProfilerArea.CPU, frameIndex));
    //        Debug.Log("Audio info:" + ProfilerDriver.GetOverviewText(ProfilerArea.Audio, frameIndex));
    //        Debug.Log("AreaCount info:" + ProfilerDriver.GetOverviewText(ProfilerArea.AreaCount, frameIndex));

    //        profilerProperty.Cleanup();
            
    //    }


    //    //Debug.Log("first index:" + firstFrameIndex);
    //    //Debug.Log("last index:" + lastFrameIndex);

      

    //}


    


}

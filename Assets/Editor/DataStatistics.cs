
using UnityEngine;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;
using System.IO;
using System.Text;


public class DataStatistics
{
    private static DataStatistics instance;

    public static DataStatistics Instance
    {
        get
        {
            if(instance == null)
            {
                return new DataStatistics();
            }
            return instance;
        }
    }

    Property monoProperty;
    Property textureProperty;
    Property animationProperty;
    Property audioProperty;
    Property meshProperty;
    Property gameobjectProperty;
    Property drawCallProperty;
    Property trisProperty;
    Property renderingTimeProperty;
    Property animationTimeProperty;
    Property uiTimeProperty;
    Property scriptsTimeProperty;
    RateProperty fpsProperty;

    ProfilerProperty profilerProperty;//用于获取CPU、FPS等数据

    BaseStandardData standardPlatformData;

    private DataStatistics() //如果在测完数据不手动设置instance == null,则会出现数据堆积。因为这是个单例模式
    {
        instance = this;

        if (MainEditor.labelTitle.Equals(MainEditor.ANDRIOD_PLATFORM))
        {
            standardPlatformData = new AndriodStandardData();
        }
        else if (MainEditor.labelTitle.Equals(MainEditor.PC_PLATFORM))
        {
            standardPlatformData = new PCStandardData();
        }

        profilerProperty = new ProfilerProperty();

        monoProperty = new Property("MonoMemory");
        textureProperty = new Property("TextureMemory");
        animationProperty = new Property("AnimationMemory");
        audioProperty = new Property("AudioMemory");
        meshProperty = new Property("MeshMemory");
        gameobjectProperty = new Property("GameObjects");
        drawCallProperty = new Property("DrawCalls");
        trisProperty = new Property("Tris");
        fpsProperty = new RateProperty("FPS", standardPlatformData.FPS);
        renderingTimeProperty = new Property("CpuRenderingTime");
        animationTimeProperty = new Property("CpuAnimationTime");
        uiTimeProperty = new Property("CpuUiTime");
        scriptsTimeProperty = new Property("CpuScriptsTime");


    }



    public void UpdateProperties()
    {
        int firstFrameIndex = ProfilerDriver.firstFrameIndex;
        if (firstFrameIndex == -1) return;
        int lastFrameIndex = ProfilerDriver.lastFrameIndex;

        for (int frame = firstFrameIndex; frame <= lastFrameIndex; frame++)//每一帧
        {
            //获得各种内存的信息
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

            //获得渲染相关的信息
            string RenderAllInfo = ProfilerDriver.GetOverviewText(ProfilerArea.Rendering, frame);

            float frameDrawCallValue = StringFormat.GetDrawCalls(RenderAllInfo);
            drawCallProperty.UpdateValueEveryFrame(frame, frameDrawCallValue);

            float frameTrisValue = StringFormat.GetTrisCount(RenderAllInfo);
            trisProperty.UpdateValueEveryFrame(frame, frameTrisValue);

            //获得FPS
            profilerProperty.SetRoot(frame, ProfilerColumn.TotalTime, ProfilerViewType.Hierarchy);
            string frameFpsValue = profilerProperty.frameFPS;
            fpsProperty.UpdateValueEveryFrame(frame, float.Parse(frameFpsValue));
            profilerProperty.Cleanup();

            //获得CPU耗时相关的信息
            string CpuAllInfo = ProfilerDriver.GetOverviewText(ProfilerArea.CPU, frame);

            float renderingTime = StringFormat.GetCpuRenderingTime(CpuAllInfo);
            renderingTimeProperty.UpdateValueEveryFrame(frame, renderingTime);

            float animationTime = StringFormat.GetCpuAnimationTime(CpuAllInfo);
            animationTimeProperty.UpdateValueEveryFrame(frame, animationTime);

            float scriptsTime = StringFormat.GetCpuScriptsTime(CpuAllInfo);
            scriptsTimeProperty.UpdateValueEveryFrame(frame, scriptsTime);

            float uiTime = StringFormat.GetCpuUiTime(CpuAllInfo);
            uiTimeProperty.UpdateValueEveryFrame(frame, uiTime);

        }
        Debug.Log("first:" + firstFrameIndex + "," + "last:" + lastFrameIndex + ",Diff:" + (lastFrameIndex - firstFrameIndex));
    }

    private void FinalCaculate()
    {
        monoProperty.MakeResult(standardPlatformData.MonoMemory);
        textureProperty.MakeResult(standardPlatformData.TextureMemory);
        animationProperty.MakeResult(standardPlatformData.AnimationMemory);
        audioProperty.MakeResult(standardPlatformData.AudioMemory);
        meshProperty.MakeResult(standardPlatformData.MeshMemory);
        gameobjectProperty.MakeResult(standardPlatformData.GameObjectCount);

        drawCallProperty.MakeResult(standardPlatformData.DrawCalls);
        trisProperty.MakeResult(standardPlatformData.Tris);
        fpsProperty.MakeResult(standardPlatformData.FPSRate);

        renderingTimeProperty.MakeResult(standardPlatformData.CpuRenderingTime);
        animationTimeProperty.MakeResult(standardPlatformData.CpuAnimationTime);
        scriptsTimeProperty.MakeResult(standardPlatformData.CpuScriptsTime);
        uiTimeProperty.MakeResult(standardPlatformData.CpuUiTime);

    }

    private void MakeTxt()
    {
        StringBuilder txtInfo = new StringBuilder();
        txtInfo.Append(StringFormat.GetExportMemoryInfo(monoProperty));
        txtInfo.Append(StringFormat.GetExportMemoryInfo(textureProperty));
        txtInfo.Append(StringFormat.GetExportMemoryInfo(meshProperty));
        txtInfo.Append(StringFormat.GetExportMemoryInfo(animationProperty));
        txtInfo.Append(StringFormat.GetExportMemoryInfo(audioProperty));
        txtInfo.Append(StringFormat.GetExportCountInfo(gameobjectProperty));

        txtInfo.Append(StringFormat.GetExportCountInfo(drawCallProperty));
        txtInfo.Append(StringFormat.GetExportCountInfo(fpsProperty));
        txtInfo.Append(StringFormat.GetExportGeometryInfo(trisProperty));

        txtInfo.Append(StringFormat.GetExportTimeConsumingInfo(renderingTimeProperty));
        txtInfo.Append(StringFormat.GetExportTimeConsumingInfo(animationTimeProperty));
        txtInfo.Append(StringFormat.GetExportTimeConsumingInfo(scriptsTimeProperty));
        txtInfo.Append(StringFormat.GetExportTimeConsumingInfo(uiTimeProperty));

        Debug.Log(txtInfo.ToString());
        File.WriteAllText(MainEditor.labelTitle + ".txt", txtInfo.ToString(),Encoding.UTF8);
    }

    public void ExportTxtResult()
    {
        FinalCaculate();
        MakeTxt();
        instance = null;//舍弃掉此实例，因为里面的数据不会自动清理。相当于清空数据
    }

    
}

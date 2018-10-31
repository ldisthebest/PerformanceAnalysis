
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
    RateProperty fpsProperty;

    ProfilerProperty profilerProperty;//用于获取CPU、FPS等数据

    BaseStandardData standardPlatformData;

    private DataStatistics()
    {
        instance = this;

        if (MainEditor.labelTitle.Equals(MainEditor.ANDRIOD_PLATFORM))
        {
            standardPlatformData = new PCStandardData();
        }
        else if (MainEditor.labelTitle.Equals(MainEditor.PC_PLATFORM))
        {
            standardPlatformData = new AndriodStandardData();
        }

        profilerProperty = new ProfilerProperty();

        monoProperty = new Property("Mono");
        textureProperty = new Property("Texture");
        animationProperty = new Property("Animation");
        audioProperty = new Property("Audio");
        meshProperty = new Property("Mesh");
        gameobjectProperty = new Property("GameObject");
        drawCallProperty = new Property("DrawCalls");
        trisProperty = new Property("Tris");
        fpsProperty = new RateProperty("FPS", standardPlatformData.FPS);


    }



    public void UpdateProperties()
    {
        int firstFrameIndex = ProfilerDriver.firstFrameIndex;
        if (firstFrameIndex == -1) return;
        int lastFrameIndex = ProfilerDriver.lastFrameIndex;

        for (int frame = firstFrameIndex; frame <= lastFrameIndex; frame++)//每一帧
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

            profilerProperty.SetRoot(frame, ProfilerColumn.TotalTime, ProfilerViewType.Hierarchy);
            string frameFpsValue = profilerProperty.frameFPS;
            fpsProperty.UpdateValueEveryFrame(frame, float.Parse(frameFpsValue));
            profilerProperty.Cleanup();

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

        Debug.Log(txtInfo.ToString());
        File.WriteAllText(MainEditor.labelTitle + "Analysis.txt", txtInfo.ToString(),Encoding.UTF8);
    }

    public void ExportTxtResult()
    {
        FinalCaculate();
        MakeTxt();
    }

    
}

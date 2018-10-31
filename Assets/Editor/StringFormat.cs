using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StringFormat{

    private static float GetMemoryValue(string info)//返回单位是 MB,解析内存数据形参格式如:xxx MB
    {
        float value = -1;
        string unit = info.Substring(info.IndexOf(' ') + 1);
        float beforeConvertValue = float.Parse(info.Remove(info.IndexOf(' '),unit.Length+1));
        if(unit.Equals("MB"))
        {
            value = beforeConvertValue;
        }
        else if(unit.Equals("KB"))
        {
            value = beforeConvertValue / 1024;
        }
        else if(unit.Equals("B"))
        {
            value = beforeConvertValue / 1024 / 1024;
        }
        return value;
    }

    private static string GetResourceInfo(string info,string Resource)
    {
        int textureIndex = info.IndexOf(Resource);
        while (info[textureIndex] != '/')
        {
            textureIndex++;
        }
        textureIndex += 2;
        int startIndex = textureIndex;
        while (info[textureIndex] != 'B')
        {
            textureIndex++;
        }

        return info.Substring(startIndex, textureIndex + 1 - startIndex);
    }

    public static float GetMonoMemory(string info)
    {
        int monoIndex = info.LastIndexOf("Mono");
        while (info[monoIndex] != ' ')
        {
            monoIndex++;
        }
        int startIndex = ++monoIndex;
        while (info[monoIndex] != 'B')
        {
            monoIndex++;
        }
        string valueInfo = info.Substring(startIndex, monoIndex + 1 - startIndex);

        return GetMemoryValue(valueInfo);
    }

    public static float GetTextureMemory(string info)
    {
        return GetMemoryValue(GetResourceInfo(info,"Textures"));
    }

    public static float GetMeshMemory(string info)
    {
        return GetMemoryValue(GetResourceInfo(info, "Meshes"));
    }

    public static float GetAnimationMemory(string info)
    {
        return GetMemoryValue(GetResourceInfo(info, "AnimationClips"));
    }

    public static float GetAudioMemory(string info)
    {
        return GetMemoryValue(GetResourceInfo(info, "AudioClips"));
    }

    public static float GameObjectCount(string info)
    {
        int textureIndex = info.IndexOf("GameObjects");
        textureIndex += 22;//"GameObjects in Scene: "的字符数量
        int startIndex = textureIndex; 
        while(info[textureIndex] != '\n')
        {
            textureIndex++;
        }
        string value = info.Substring(startIndex, textureIndex - startIndex);
        return float.Parse(value);
    }

    public static float GetDrawCalls(string info)
    {
        int index = info.IndexOf("Draw");
        index += 12; //"Draw Calls: "的字符数量
        int startIndex = index;
        while(info[index] != ' ')
        {
            index++;
        }
        return float.Parse(info.Substring(startIndex, index - startIndex));
    }

    public static float GetTrisCount(string info) //单位：k
    {
        int index = info.IndexOf("Tris");
        index += 6;//"Tris: "的字符数量
        int startIndex = index;
        while(info[index] != ' ')
        {
            index++;
        }
        if(info[index-1] == 'k')
        {
            return float.Parse(info.Substring(startIndex, index-1 - startIndex));
        }
        else
        {
            return float.Parse(info.Substring(startIndex, index - startIndex));
        }
        
    }

    private static float GetCpuTimeInfo(string info,string name)
    {
        int index = info.IndexOf(name);
        index += (name.Length+2);
        int startIndex = index;
        while (info[index] != 'm')
        {
            index++;
        }
        return float.Parse(info.Substring(startIndex, index - startIndex));
    }

    public static float GetCpuRenderingTime(string info) //单位毫秒
    {
        return GetCpuTimeInfo(info, "Rendering");
    }

    public static float GetCpuScriptsTime(string info)
    {
        return GetCpuTimeInfo(info, "Scripts");
    }

    public static float GetCpuUiTime(string info)
    {
        return GetCpuTimeInfo(info, "UI");
    }

    public static float GetCpuAnimationTime(string info)
    {
        return GetCpuTimeInfo(info, "Animation");
    }

    private static void AddUnitString(StringBuilder info, string unitFormat)
    {
        info.Append(" ");
        info.Append(unitFormat);
        info.Append("    ");
    }
    private static string GetExportInfo(Property property,string unitFormat)
    {
        StringBuilder info = new StringBuilder();
        info.Append(property.Name);
        info.Append("--\n");
        info.Append("最小: ");
        info.Append(property.MinValue);
        AddUnitString(info, unitFormat);
        info.Append("最大: ");
        info.Append(property.MaxValue);
        AddUnitString(info, unitFormat);
        info.Append("平均: ");
        info.Append(property.AverageValue);
        AddUnitString(info, unitFormat);
        info.Append("结果: ");
        info.Append(property.Result);
        info.Append("\n\n");

        return info.ToString();
    }
    public static string GetExportMemoryInfo(Property property)
    {
        return GetExportInfo(property,"MB");
    }
    public static string GetExportCountInfo(Property property)
    {
        return GetExportInfo(property, "");
    }
    public static string GetExportGeometryInfo(Property property)
    {
        return GetExportInfo(property, "k");
    }
    public static string GetExportTimeConsumingInfo(Property property)
    {
        return GetExportInfo(property, "ms");
    }

}

﻿using GTA5Core.Native;
using GTA5Core.Offsets;
using GTA5Core.GTA.Vehicles;

namespace GTA5Core.Features;

public static class Vehicle2
{
    /// <summary>
    /// 刷出线上载具
    /// </summary>
    /// <param name="model">载具名称</param>
    /// <param name="z255">高度</param>
    /// <param name="dist">距离</param>
    /// <param name="mods">Mod</param>
    public static async Task SpawnVehicle(string model, float z255, int dist, int[] mods)
    {
        await Task.Run(() =>
        {
            if (string.IsNullOrEmpty(model))
                return;

            long pCPed = Globals.GetCPed();
            Vector3 vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

            long pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation.__Offset__);

            float sin = Memory.Read<float>(pCNavigation + CPed.CNavigation.RightX);
            float cos = Memory.Read<float>(pCNavigation + CPed.CNavigation.ForwardX);

            vector3.X += cos * dist;
            vector3.Y += sin * dist;

            if (z255 == -255.0f)
                vector3.Z = z255;
            else
                vector3.Z += z255;

            Hacks.WriteGA(Base.oVMCreate + 7 + 0, vector3.X);        // 载具坐标x
            Hacks.WriteGA(Base.oVMCreate + 7 + 1, vector3.Y);        // 载具坐标y
            Hacks.WriteGA(Base.oVMCreate + 7 + 2, vector3.Z);        // 载具坐标z

            Hacks.WriteGA(Base.oVMCreate + 27 + 66, Hacks.Joaat(model));     // 载具哈希值

            Hacks.WriteGA(Base.oVMCreate + 3, 0);            // pegasus
            Hacks.WriteGA(Base.oVMCreate + 5, 1);            // can spawn flag must be odd
            Hacks.WriteGA(Base.oVMCreate + 2, 1);            // spawn toggle gets reset to 0 on car spawn

            Hacks.WriteGAString(Base.oVMCreate + 27 + 1, Guid.NewGuid().ToString()[..8]);    // License plate  车牌

            // 这两辆载具使用mod会导致崩溃
            if (model != "issi8" && model != "entity3")
            {
                // 最大化升级载具
                for (int i = 0; i < 48; i++)
                {
                    if (i < 17)
                    {
                        Hacks.WriteGA(Base.oVMCreate + 27 + 10 + i, mods[i]);
                    }
                    else if (i > 22)
                    {
                        Hacks.WriteGA(Base.oVMCreate + 27 + 10 + 6 + i, mods[i]);
                    }
                }
            }

            // 随机涂装
            if (mods[48] > 0)
                Hacks.WriteGA(Base.oVMCreate + 27 + 10 + 48, new Random().Next(0, mods[48] + 1));

            Hacks.WriteGA(Base.oVMCreate + 27 + 5, -1);      // primary -1 auto 159  主色调
            Hacks.WriteGA(Base.oVMCreate + 27 + 6, -1);      // secondary -1 auto 159  副色调
            Hacks.WriteGA(Base.oVMCreate + 27 + 7, -1);      // pearlescent  珠光色

            Hacks.WriteGA(Base.oVMCreate + 27 + 8, -1);      // wheel color  车轮颜色
            Hacks.WriteGA(Base.oVMCreate + 27 + 69, -1);     // Wheel type  车轮类型
            Hacks.WriteGA(Base.oVMCreate + 27 + 33, -1);     // wheel selection  车轮选择

            Hacks.WriteGA(Base.oVMCreate + 27 + 24, new Random().Next(0, mods[14] + 1));      // 喇叭
            Hacks.WriteGA(Base.oVMCreate + 27 + 27, 1);      // Turbo (0-1)  涡轮增压
            Hacks.WriteGA(Base.oVMCreate + 27 + 28, 1);      // weaponised ownerflag

            Hacks.WriteGA(Base.oVMCreate + 27 + 30, 1);      // 烧胎烟雾
            Hacks.WriteGA(Base.oVMCreate + 27 + 32, 1);      // 氙气车灯 (0-14)

            Hacks.WriteGA(Base.oVMCreate + 27 + 60, 1);      // landinggear/vehstate  起落架/载具状态
            Hacks.WriteGA(Base.oVMCreate + 27 + 62, new Random().Next(0, 256));      // Tire smoke color R  烧胎烟雾颜色
            Hacks.WriteGA(Base.oVMCreate + 27 + 63, new Random().Next(0, 256));      // G
            Hacks.WriteGA(Base.oVMCreate + 27 + 64, new Random().Next(0, 256));      // B
            Hacks.WriteGA(Base.oVMCreate + 27 + 65, new Random().Next(0, 7));        // Window tint 0-6  窗户

            Hacks.WriteGA(Base.oVMCreate + 27 + 74, new Random().Next(0, 256));      // Red Neon Amount 1-255 100%-0%  红霓虹灯数量
            Hacks.WriteGA(Base.oVMCreate + 27 + 75, new Random().Next(0, 256));      // G
            Hacks.WriteGA(Base.oVMCreate + 27 + 76, new Random().Next(0, 256));      // B

            Hacks.WriteGA(Base.oVMCreate + 27 + 77, 0xF0400200);   // vehstate 4030726305  载具状态 没有这个载具起落架是收起状态

            Hacks.WriteGA(Base.oVMCreate + 27 + 95, 14);     // Ownerflag  拥有者标志
            Hacks.WriteGA(Base.oVMCreate + 27 + 94, 2);      // Personal car ownerflag  个人载具拥有者标志
        });
    }

    /// <summary>
    /// 通过Hash值查找载具信息
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public static VehicleInfo FindVehicleNameByHash(long hash)
    {
        var result = VehicleHash.AllVehicles.Find(v => v.Hash == hash);
        if (result != null)
            return result;

        return null;
    }
}

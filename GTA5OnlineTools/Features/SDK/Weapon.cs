﻿using GTA5OnlineTools.Features.Core;

namespace GTA5OnlineTools.Features.SDK;

public static class Weapon
{
    /// <summary>
    /// 补满当前武器弹药
    /// </summary>
    public static void FillCurrentAmmo()
    {
        long pWeapon_AmmoInfo = GTA5Mem.Read<long>(General.WorldPTR, Offsets.Weapon.AmmoInfo);

        int getMaxAmmo = GTA5Mem.Read<int>(pWeapon_AmmoInfo + 0x28);

        long my_offset_0 = pWeapon_AmmoInfo;
        long my_offset_1;
        byte ammo_type;

        do
        {
            my_offset_0 = GTA5Mem.Read<long>(my_offset_0 + 0x08);
            my_offset_1 = GTA5Mem.Read<long>(my_offset_0 + 0x00);

            if (my_offset_0 == 0 || my_offset_1 == 0)
                return;

            ammo_type = GTA5Mem.Read<byte>(my_offset_1 + 0x0C);

        } while (ammo_type == 0x00);

        GTA5Mem.Write<int>(my_offset_1 + 0x18, getMaxAmmo);
    }

    /// <summary>
    /// 补满全部武器弹药
    /// </summary>
    public static void FillAllAmmo()
    {
        long pWeapon = GTA5Mem.Read<long>(General.WorldPTR, new int[] { 0x08, 0x10D0, 0x48 });

        int count = 0;
        while (GTA5Mem.Read<int>(pWeapon + count * 0x08) != 0 && GTA5Mem.Read<int>(pWeapon + count * 0x08, new int[] { 0x08 }) != 0)
        {
            int ammo_1 = GTA5Mem.Read<int>(pWeapon + count * 0x08, new int[] { 0x08, 0x28 });
            int ammo_2 = GTA5Mem.Read<int>(pWeapon + count * 0x08, new int[] { 0x08, 0x34 });
            int max_ammo = Math.Max(ammo_1, ammo_2);
            GTA5Mem.Write<int>(pWeapon + count * 0x08, new int[] { 0x20 }, max_ammo);
            count++;
        }
    }

    /// <summary>
    /// 无限弹药
    /// </summary>
    public static void InfiniteAmmo(bool isEnable)
    {
        if (isEnable)
        {
            long addrAmmo = GTA5Mem.FindPattern("41 2B D1 E8");
            if (addrAmmo == 0)
            {
                addrAmmo = GTA5Mem.FindPattern("90 90 90 E8");
            }
            GTA5Mem.WriteBytes(addrAmmo, new byte[] { 0x90, 0x90, 0x90 });
        }
        else
        {
            long addrAmmo = GTA5Mem.FindPattern("41 2B D1 E8");
            if (addrAmmo == 0)
            {
                addrAmmo = GTA5Mem.FindPattern("90 90 90 E8");
            }
            GTA5Mem.WriteBytes(addrAmmo, new byte[] { 0x41, 0x2B, 0xD1 });
        }
    }

    /// <summary>
    /// 无需换弹
    /// </summary>
    public static void NoReload(bool isEnable)
    {
        if (isEnable)
        {
            long addrAmmo = GTA5Mem.FindPattern("41 2B C9 3B C8 0F");
            if (addrAmmo == 0)
            {
                addrAmmo = GTA5Mem.FindPattern("90 90 90 3B C8 0F");
            }
            GTA5Mem.WriteBytes(addrAmmo, new byte[] { 0x90, 0x90, 0x90 });
        }
        else
        {
            long addrAmmo = GTA5Mem.FindPattern("41 2B C9 3B C8 0F");
            if (addrAmmo == 0)
            {
                addrAmmo = GTA5Mem.FindPattern("90 90 90 3B C8 0F");
            }
            GTA5Mem.WriteBytes(addrAmmo, new byte[] { 0x41, 0x2B, 0xC9 });
        }
    }

    /// <summary>
    /// 弹药编辑，默认0，无限弹药1，无限弹夹2，无限弹药和弹夹3
    /// </summary>
    public static void AmmoModifier(byte flag)
    {
        GTA5Mem.Write<byte>(General.WorldPTR, Offsets.Weapon.AmmoModifier, flag);
    }

    /// <summary>
    /// 无后坐力
    /// </summary>
    public static void NoRecoil()
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.NoRecoil, 0.0f);
    }

    /// <summary>
    /// 无子弹扩散
    /// </summary>
    public static void NoSpread()
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.NoSpread, 0.0f);
    }

    /// <summary>
    /// 启用子弹类型，2:Fists，3:Bullet，5:Explosion
    /// </summary>
    public static void ImpactType(byte type)
    {
        GTA5Mem.Write<byte>(General.WorldPTR, Offsets.Weapon.ImpactType, type);
    }

    /// <summary>
    /// 设置子弹类型
    /// </summary>
    public static void ImpactExplosion(int id)
    {
        GTA5Mem.Write<int>(General.WorldPTR, Offsets.Weapon.ImpactExplosion, id);
    }

    /// <summary>
    /// 武器射程
    /// </summary>
    public static void Range()
    {
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.Range, 2000.0f);
        GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.LockRange, 1000.0f);
    }

    /// <summary>
    /// 武器快速换弹
    /// </summary>
    public static void ReloadMult(bool isEnable)
    {
        if (isEnable)
            GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.ReloadMult, 4.0f);
        else
            GTA5Mem.Write<float>(General.WorldPTR, Offsets.Weapon.ReloadMult, 1.0f);
    }
}

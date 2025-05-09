﻿global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using Terraria;
global using Terraria.Localization;
global using Terraria.ModLoader;
global using StarOwner.Core.SwingHelpers;
global using System.IO;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.Audio;
global using Terraria.Graphics.CameraModifiers;
global using Terraria.ID;

namespace StarOwner
{
    public static class TheUtility
    {
        public static bool InBegin()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            FieldInfo field = spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null && field.GetValue(spriteBatch) is bool canDraw)
            {
                return canDraw;
            }
            return false;
        }
        public static void VillagesItemOnHit(Item item, Player player, Rectangle itemRectangle, int damage, float knockBack, int npcIndex, int dmgRandomized, int dmgDone)
        {
            Type type = player.GetType();
            type.GetMethod("ApplyNPCOnHitEffects", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(player, new object[] { item, itemRectangle, damage, knockBack, npcIndex, dmgRandomized, dmgDone });
        }
        public static void ItemCheck_MeleeHitNPCs(Item sItem, Player player, Rectangle itemRectangle, int originalDamage, float knockBack)
        {
            Type type = player.GetType();
            type.GetMethod("ItemCheck_MeleeHitNPCs", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(player, [sItem, itemRectangle, originalDamage, knockBack]);
        }
        public static void Player_ItemCheck_Shoot(Player player, Item sItem, int weaponDamage)
        {
            Type type = player.GetType();
            type.GetMethod("ItemCheck_Shoot", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(player, new object[] { player.whoAmI, sItem, weaponDamage });
        }
        public static void Player_ItemCheck_EmitUseVisuals(Player player, Item item, Rectangle hitBox)
        {
            Type type = player.GetType();
            type.GetMethod("ItemCheck_EmitUseVisuals", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(player, new object[] { item, hitBox });
        }
        public static void ResetProjHit(Projectile proj)
        {
            for (int i = 0; i < proj.localNPCImmunity.Length; i++)
            {
                proj.localNPCImmunity[i] = 0;
            }
        }
        public static List<CustomVertexInfo> GenerateTriangle(List<CustomVertexInfo> customs)
        {
            List<CustomVertexInfo> triangleList = new();
            for (int i = 0; i < customs.Count - 2; i += 2)
            {
                if (i + 3 >= customs.Count - 2)
                    break;
                triangleList.Add(customs[i]);
                triangleList.Add(customs[i + 2]);
                triangleList.Add(customs[i + 1]);

                triangleList.Add(customs[i + 1]);
                triangleList.Add(customs[i + 2]);
                triangleList.Add(customs[i + 3]);
            }
            return triangleList;
        }
        public static void GetWeaponDrawColor(Texture2D DrawColorTex, Asset<Texture2D> texture, float factor = 0.9f, float colorScale = 1.95f)
        {
            Main.QueueMainThreadAction(() =>
            {
                Color[] TexColor = new Color[texture.Width() * texture.Height()];
                int width = texture.Width();
                texture.Value.GetData(TexColor);
                Color[] colors = new Color[texture.Height()];
                for (int i = colors.Length - 1; i > 0; i--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Color value1 = TexColor[(colors.Length - i) * width + j];
                        if (value1 == default) continue;
                        colors[i] = Color.Lerp(value1, colors[i], factor);
                    }
                    colors[i] *= colorScale;
                }
                DrawColorTex.SetData(colors);
            });
        }
        public static int GetItemFrameCount(Item item)
        {
            if (Main.itemAnimations[item.type] != null)
            {
                return Main.itemAnimations[item.type].FrameCount;
            }
            return 1;
        }
        public static void SetProjFrameWithItem(Projectile proj, Item item)
        {
            if (Main.itemAnimations[item.type] != null) proj.frame = Main.itemAnimations[item.type].Frame;
        }
        public static void SetPlayerImmune(Player player, int immuneTime = 12)
        {
            if (player.immuneTime < immuneTime)
                player.SetImmuneTimeForAllTypes(immuneTime);
        }
        /// <summary>
        /// 用这个直接获取当前实例的命名空间带一个"/"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetInstancePart(this object obj)
        {
            string nameSpace = obj.GetType().Namespace;
            nameSpace = nameSpace.Replace('.', '/');
            nameSpace += "/";
            return nameSpace;
        }
        /// <summary>
        /// 用这个直接获取当前实例的命名空间附带名字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetInstancePartWithName(this object obj)
        {
            string nameSpace = obj.GetInstancePart();
            string name = obj.GetType().Name;
            return nameSpace + name;
        }
        public static bool TryAddArray<T>(this HashSet<T> hash, T[] Array)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                if (!hash.Add(Array[i]))
                    return false;
            }
            return true;
        }
        public static string RegisterText(string key)
        {
            return Language.GetOrRegister(key, () => "null").Value;
        }
    }
}

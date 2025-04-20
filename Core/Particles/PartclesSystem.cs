namespace StarOwner.Core.Particles
{
    public class PartclesSystem : ModSystem
    {
        public static Dictionary<BasicPartcle.DrawLayer, List<BasicPartcle>> partcle = new();
        private static Dictionary<BasicPartcle.DrawLayer, List<int>> partclesRemoveCeche = new();
        public override void Load()
        {
            On_Main.DrawDust += PostDrawDusts;
            On_Main.DrawProjectiles += PostDrawProjectiles;
            On_Main.DrawTiles += OnMain_PostDrawTiles;
            On_Main.DrawPlayers_BehindNPCs += OnMain_PostDrawPlayers_BehindNPCs;
        }

        private static void OnMain_PostDrawPlayers_BehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
        {
            orig.Invoke(self);
            if (partcle.TryGetValue(BasicPartcle.DrawLayer.AfterPlayer, out List<BasicPartcle> value))
            {
                for (int i = 0; i < value.Count; i++) // 遍历每一个粒子
                {
                    BasicPartcle partcle = value[i];
                    partcle.Draw();
                }
            }
        }

        private static void OnMain_PostDrawTiles(On_Main.orig_DrawTiles orig, Main self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig.Invoke(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
            if (partcle.TryGetValue(BasicPartcle.DrawLayer.AfterTile, out List<BasicPartcle> value))
            {
                for (int i = 0; i < value.Count; i++) // 遍历每一个粒子
                {
                    BasicPartcle partcle = value[i];
                    partcle.Draw();
                }
            }
        }

        private static void PostDrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            if (partcle.TryGetValue(BasicPartcle.DrawLayer.AfterProj, out List<BasicPartcle> value))
            {
                for (int i = 0; i < value.Count; i++) // 遍历每一个粒子
                {
                    BasicPartcle partcle = value[i];
                    partcle.Draw();
                }
            }
        }

        private static void PostDrawDusts(On_Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            if (partcle.TryGetValue(BasicPartcle.DrawLayer.AfterDust, out List<BasicPartcle> value))
            {
                for (int i = 0; i < value.Count; i++) // 遍历每一个粒子
                {
                    BasicPartcle partcle = value[i];
                    partcle.Draw();
                }
            }
        }

        public override void PostUpdateDusts()
        {
            partclesRemoveCeche.Clear();
            foreach (var layer in partcle.Keys) // 遍历每一层
            {
                for (int i = 0; i < partcle[layer].Count; i++) // 遍历每一个粒子
                {
                    BasicPartcle partcle = PartclesSystem.partcle[layer][i];
                    int extraUpdate = partcle.extraUpdate;
                    while (extraUpdate >= 0)
                    {
                        partcle.Update();
                        extraUpdate--;
                    }
                    if (partcle.ShouldRemove)
                    {
                        if (partclesRemoveCeche.ContainsKey(layer))
                        {
                            partclesRemoveCeche[layer].Add(i);
                        }
                        else
                        {
                            partclesRemoveCeche.Add(layer, new(){ i });
                        }
                    }
                }
            }
            foreach(var layer in partclesRemoveCeche.Keys)
            {
                foreach(var partcle in partclesRemoveCeche[layer])
                {
                    PartclesSystem.partcle[layer].RemoveAt(partcle);
                }
            }
        }
        public static void AddPartcle(BasicPartcle.DrawLayer drawLayer, BasicPartcle partcle)
        {
            if (PartclesSystem.partcle.TryGetValue(drawLayer, out List<BasicPartcle> value))
                value.Add(partcle);
            else
                PartclesSystem.partcle.Add(drawLayer, new() { partcle });
        }
    }
}

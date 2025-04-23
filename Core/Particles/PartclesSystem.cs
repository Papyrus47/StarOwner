using Terraria.Graphics.Renderers;

namespace StarOwner.Core.Particles
{
    public class ParticlesSystem : ModSystem
    {

        //原写法大量增删粒子时会有明显性能问题，故修改

        public static HashSet<BasicParticle> particle = new();

        public override void Load()
        {
            On_Main.DrawDust += PostDrawDusts;
            On_Main.DrawProjectiles += PostDrawProjectiles;
            On_Main.DrawTiles += OnMain_PostDrawTiles;
            On_Main.DrawPlayers_BehindNPCs += OnMain_PostDrawPlayers_BehindNPCs;
        }

        private static void DrawParticles(BasicParticle.DrawLayer layer, bool endSBFirst = false)
        {
            if (endSBFirst)
                Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (BasicParticle part in particle)
            {
                if (part.drawLayer == layer)
                    part.Draw();
            }
            Main.spriteBatch.End();
            if (endSBFirst)
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        }

        private static void OnMain_PostDrawPlayers_BehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterPlayer);
        }

        private static void OnMain_PostDrawTiles(On_Main.orig_DrawTiles orig, Main self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig.Invoke(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
            //DrawParticles(BasicParticle.DrawLayer.AfterTile);
        }

        private static void PostDrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterProj);
        }

        private static void PostDrawDusts(On_Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterDust);
        }

        public override void PostUpdateDusts()
        {
            HashSet<BasicParticle> toRemove = new();
            foreach (BasicParticle part in particle)
            {
                int extraUpdate = part.extraUpdate;
                while (extraUpdate >= 0)
                {
                    part.Update();
                    extraUpdate--;
                }
                if (part.ShouldRemove)
                {
                    toRemove.Add(part);
                }
            }
            foreach (BasicParticle part in toRemove)
            {
                particle.Remove(part);
            }
        }

        public static void AddParticle(BasicParticle.DrawLayer drawLayer, BasicParticle part)
        {
            part.drawLayer = drawLayer;
            particle.Add(part);
        }

    }
}

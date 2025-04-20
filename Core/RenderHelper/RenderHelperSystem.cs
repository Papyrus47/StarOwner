namespace StarOwner.Core.RenderHelper
{
    public class RenderHelperSystem
    {
        public Queue<DrawRender> drawRenders = new();
        public void Draw(RenderTarget2D renderTarget1, RenderTarget2D renderTarget2)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;//把这一长串用gd代替，方便写
            SpriteBatch sb = Main.spriteBatch;//这个同理
            drawRenders ??= new();
            while (drawRenders.Count > 0)
            {
                var render = drawRenders.Dequeue();
                render?.Draw(gd, sb, renderTarget1, renderTarget2);
            }
        }
    }
}

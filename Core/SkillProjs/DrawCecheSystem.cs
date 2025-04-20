namespace StarOwner.Core.SkillProjs
{
    /// <summary>
    /// 实现这个系统 添加绘制
    /// </summary>
    public class DrawCecheSystem
    {
        /// <summary>
        /// 这是缓存
        /// </summary>
        public class Ceche
        {
            public bool Remove;
            public virtual void DrawCeche() { }
            public virtual void UpdateCeche() { }
        }
        public List<Ceche> CecheList = new();
        public void Update()
        {
            for (int i = 0; i < CecheList.Count; i++)
            {
                if (CecheList[i].Remove)
                {
                    CecheList.RemoveAt(i);
                    i--;
                }
                else
                {
                    CecheList[i].UpdateCeche();
                }
            }
        }
        public void Draw()
        {
            for (int i = 0; i < CecheList.Count; i++)
            {
                if (CecheList[i].Remove)
                {
                    CecheList.RemoveAt(i);
                    i--;
                }
                else
                {
                    CecheList[i].DrawCeche();
                }
            }
        }
    }
}

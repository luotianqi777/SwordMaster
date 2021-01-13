namespace AI.Role
{
    class 李逍遥 : RoleAI
    {
        protected override void AddSwords()
        {
            AddSword<Sword.万剑诀>();
            AddSword<Sword.千方残光剑>();
            AddSword<Sword.逍遥神剑>();
        }
    }
}

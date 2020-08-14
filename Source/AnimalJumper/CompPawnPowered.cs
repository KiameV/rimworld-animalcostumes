using RimWorld;
using System.Reflection;

namespace AnimalCostumes
{
	public class CompPawnPowered : CompReloadable
	{
		private static readonly FieldInfo rcFI = typeof(CompReloadable).GetField("remainingCharges", BindingFlags.Instance | BindingFlags.NonPublic);

		public new CompProperties_PawnPowered Props => props as CompProperties_PawnPowered;

		public override void PostPostMake()
		{
			base.PostPostMake();
			this.SetRemainingCharges();
		}
		public override void PostExposeData()
		{
			base.PostExposeData();
		}
		public void SetRemainingCharges()
        {
			float v = (this.Wearer.needs.rest.CurLevel - 0.14f) * this.Props.energyUseMultiplier;
			rcFI.SetValue(this, (int)v);
		}

		public new void UsedOnce()
		{
			this.Wearer.needs.rest.CurLevelPercentage -= Props.ammoCountPerCharge * 0.01f;
			SetRemainingCharges();
		}
	}
}
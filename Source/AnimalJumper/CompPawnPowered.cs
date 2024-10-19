using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace AnimalCostumes
{
	public class CompPawnPowered : CompApparelReloadable
	{
        private static readonly FieldInfo rcFI = typeof(CompApparelReloadable).GetField("remainingCharges", BindingFlags.Instance | BindingFlags.NonPublic);

		public new CompProperties_PawnPowered Props => props as CompProperties_PawnPowered;

		public override void PostExposeData()
		{
			base.PostExposeData();
		}

		public void SetRemainingCharges()
		{
			float v = 0;
			Need rest = this.Wearer?.needs?.rest;
			if (rest != null && this.Props != null)
            {
				v = (rest.CurLevel - 0.14f) * this.Props.energyUseMultiplier;
			}
			
			rcFI.SetValue(this, Math.Max(0, (int)v));
		}

		public void UsedOnceNew()
		{
			Log.Warning("UsedOnce");
			Log.Warning(this.Wearer.needs.rest.CurLevelPercentage + "");
			this.Wearer.needs.rest.CurLevelPercentage -= Props.ammoCountPerCharge * 0.01f;
			SetRemainingCharges();
		}
	}
}
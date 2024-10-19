using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AnimalCostumes
{
	public class CompProperties_PawnPowered : CompProperties_ApparelReloadable
	{
		public float energyUseMultiplier = 1f;

		public CompProperties_PawnPowered()
		{
			base.compClass = typeof(CompPawnPowered);
		}

        public override void PostLoadSpecial(ThingDef parent)
        {
            base.PostLoadSpecial(parent);
			energyUseMultiplier = 100f / ammoCountPerCharge;
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			base.maxCharges = (int)(86.0f / base.ammoCountPerCharge);
			base.destroyOnEmpty = false;
			base.ammoCountToRefill = 0;
			return new List<string>(0);
		}
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			return new List<StatDrawEntry>(0);
		}
	}
}

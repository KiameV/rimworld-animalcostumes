using RimWorld;
using Verse;

namespace AnimalCostumes
{
	class Verb_Jump : RimWorld.Verb_Jump
	{		
		protected override bool TryCastShot()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Items with jump capability are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 550187797);
				return false;
			}

            if (base.CasterPawn == null || !(base.DirectOwner is CompPawnPowered ccpp))
            {
                return false;
            }

            IntVec3 cell = currentTarget.Cell;
			Map map = base.CasterPawn.Map;
			ccpp.UsedOnce();
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ACThingDefOf.ACPawnJumper, base.CasterPawn, cell);
			if (pawnFlyer != null)
			{
				GenSpawn.Spawn(pawnFlyer, cell, map);
				return true;
			}
			return false;
		}
	}
}
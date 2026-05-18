using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SEABOURNE.SEABOURNECode.Extensions;

namespace SEABOURNE.SEABOURNECode.Relics;

public sealed class CannonDamageRelic : SeabourneRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override void ModifyCannonDamage(ref decimal multiplier)
    {
        multiplier += 0.25m;
    }
}

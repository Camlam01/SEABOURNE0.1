using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using SEABOURNE.SEABOURNECode.Extensions;

namespace SEABOURNE.SEABOURNECode.Relics;

public sealed class CastBlockRelic : SeabourneRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override async Task OnCast(PlayerChoiceContext choiceContext, int amount)
    {
        if (Owner is not null)
            await SeabourneState.GainBlock(this, Owner.Creature, 2);
    }
}

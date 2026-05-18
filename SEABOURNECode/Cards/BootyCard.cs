using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class BootyCard : SeabourneCard
{
    public BootyCard() : base(3, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(15m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Block(choiceContext, play, PrimaryBlock);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) SeabourneState.Gems(owner).SlotCount += 1;
        if (!Acquire(play, SeabourneGemType.Amber)) Acquire(play, SeabourneGemType.Opal);
    }

    protected override void OnUpgrade()
    {
        UpgradeBlock(5m);
    }
}

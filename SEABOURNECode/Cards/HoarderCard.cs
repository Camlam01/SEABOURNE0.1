using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class HoarderCard : SeabourneCard
{
    public HoarderCard() : base(1, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(7m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) SeabourneState.Gems(owner).SlotCount += 1;
        await Block(choiceContext, play, PrimaryBlock);
    }

    protected override void OnUpgrade()
    {
        UpgradeBlock(1m);
    }
}

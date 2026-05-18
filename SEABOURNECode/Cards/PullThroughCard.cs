using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class PullThroughCard : SeabourneCard
{
    public PullThroughCard() : base(2, CardType.Skill, CardRarity.Common, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(11m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Reel(choiceContext, play);
        await Block(choiceContext, play, PrimaryBlock);
    }

    protected override void OnUpgrade()
    {
        UpgradeBlock(3m);
    }
}

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class DefendCard : SeabourneCard
{
    public DefendCard() : base(1, CardType.Skill, CardRarity.Basic, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(5m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Block(choiceContext, play, PrimaryBlock);
    }

    protected override void OnUpgrade()
    {
        UpgradeBlock(3m);
    }
}

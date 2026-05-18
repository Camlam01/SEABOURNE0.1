using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class SeaShieldCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ BlockVar(12m) ];
    public SeaShieldCard() : base(1, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
        AddWet(1);
    }

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

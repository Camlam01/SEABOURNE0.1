using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class FullyLoadedPowerCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public FullyLoadedPowerCard() : base(2, CardType.Power, CardRarity.Rare, SelfTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplySelf<FullyLoadedPower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {

    }
}

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class FlawlessStoneCard : SeabourneCard
{
    public FlawlessStoneCard() : base(2, CardType.Power, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        Acquire(play, SeabourneGemType.Diamond);
        await ApplySelf<FlawlessStonePower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}

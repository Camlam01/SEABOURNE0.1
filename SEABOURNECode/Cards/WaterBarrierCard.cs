using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class WaterBarrierCard : SeabourneCard
{
    public WaterBarrierCard() : base(2, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        var cast = SeabourneReflection.FindPower<CastPower>(owner)?.Amount ?? 0;
        await ApplySelf<WaterwallPower>(choiceContext, play, cast);
        await Reel(choiceContext, play);
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}

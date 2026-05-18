using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class RockySeasCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public RockySeasCard() : base(1, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        var waterwall = SeabourneReflection.FindPower<WaterwallPower>(owner)?.Amount ?? 0;
        await Block(choiceContext, play, waterwall);
        await Cast(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {

    }
}

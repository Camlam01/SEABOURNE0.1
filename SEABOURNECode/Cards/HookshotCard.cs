using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class HookshotCard : SeabourneCard
{
    public HookshotCard() : base(2, CardType.Attack, CardRarity.Rare, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await FireCannon(choiceContext, play);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) { var cannon = SeabourneState.Cannon(owner); cannon.LoadedCards.AddRange(cannon.FiredCards); cannon.FiredCards.Clear(); }
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}

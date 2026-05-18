using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class HarpoonCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(15m) ];
    public HarpoonCard() : base(2, CardType.Attack, CardRarity.Rare, AnyEnemyTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Attack(choiceContext, play, PrimaryDamage);
        await Cast(choiceContext, play, 6);
        await Reel(choiceContext, play);
    }

    protected override void OnUpgrade()
    {

    }
}

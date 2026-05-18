using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class SplashingStrikeCard : SeabourneCard
{
    public SplashingStrikeCard() : base(1, CardType.Attack, CardRarity.Common, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m) ];
    private int _wetStacks = 1;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Cast(choiceContext, play, 1);
        await AttackAll(choiceContext, play, PrimaryDamage);
        AddWet(_wetStacks);
    }

    protected override void OnUpgrade()
    {
        _wetStacks = 2;
    }
}

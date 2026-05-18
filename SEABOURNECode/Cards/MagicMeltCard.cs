using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class MagicMeltCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m), BlockVar(5m) ];
    public MagicMeltCard() : base(1, CardType.Attack, CardRarity.Rare, AllEnemiesTarget)
    {
        AddImbued(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await AttackAll(choiceContext, play, PrimaryDamage);
        await Block(choiceContext, play, PrimaryBlock);
    }

    protected override void OnUpgrade()
    {
        AddImbued(1);
    }
}

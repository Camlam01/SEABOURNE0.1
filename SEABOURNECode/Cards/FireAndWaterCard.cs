using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class FireAndWaterCard : SeabourneCard
{
    public FireAndWaterCard() : base(1, CardType.Attack, CardRarity.Uncommon, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(5m), BlockVar(5m) ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => KeywordList(CardKeyword.Innate);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Attack(choiceContext, play, PrimaryDamage);
        await Block(choiceContext, play, PrimaryBlock);
        if (!Acquire(play, SeabourneGemType.Ruby)) Acquire(play, SeabourneGemType.Sapphire);
    }

    protected override void OnUpgrade()
    {

    }
}

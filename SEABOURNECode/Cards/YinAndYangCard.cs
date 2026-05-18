using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class YinAndYangCard : SeabourneCard
{
    public YinAndYangCard() : base(2, CardType.Attack, CardRarity.Uncommon, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(8m) ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => KeywordList(CardKeyword.Innate);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await AttackAll(choiceContext, play, PrimaryDamage);
        await ApplyAll<WeakPower>(choiceContext, play, 1);
        if (!Acquire(play, SeabourneGemType.Diamond)) Acquire(play, SeabourneGemType.Opal);
    }

    protected override void OnUpgrade()
    {

    }
}

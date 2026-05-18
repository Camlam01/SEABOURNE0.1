using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class EmpowerCard : SeabourneCard
{
    public EmpowerCard() : base(2, CardType.Skill, CardRarity.Uncommon, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _trance = 5;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplyTarget<TrancePower>(choiceContext, play, _trance);
        await ApplyTarget<StrengthPower>(choiceContext, play, 3);
    }

    protected override void OnUpgrade()
    {
        _trance = 6;
    }
}

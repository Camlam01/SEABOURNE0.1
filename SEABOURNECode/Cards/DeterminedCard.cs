using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class DeterminedCard : SeabourneCard
{
    public DeterminedCard() : base(1, CardType.Skill, CardRarity.Uncommon, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplyAll<StrengthPower>(choiceContext, play, 3);
        await ApplySelf<SlipperyPower>(choiceContext, play, 1);
    }

    protected override void OnUpgrade()
    {

    }
}

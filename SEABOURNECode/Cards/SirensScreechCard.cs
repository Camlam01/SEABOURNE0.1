using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class SirensScreechCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _trance = 3;

    public SirensScreechCard() : base(3, CardType.Skill, CardRarity.Uncommon, AllEnemiesTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await ApplyAll<TrancePower>(choiceContext, play, _trance);
    }

    protected override void OnUpgrade()
    {
        _trance = 4;
    }
}

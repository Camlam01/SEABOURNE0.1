using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class EntrancingMelodyCard : SeabourneCard
{
    public EntrancingMelodyCard() : base(2, CardType.Skill, CardRarity.Rare, AllEnemiesTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _times = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        for (var i = 0; i < _times; i++) await ApplyAll<TrancePower>(choiceContext, play, 3);
    }

    protected override void OnUpgrade()
    {
        _times = 3;
    }
}

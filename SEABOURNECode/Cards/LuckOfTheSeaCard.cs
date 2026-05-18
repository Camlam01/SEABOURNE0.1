using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class LuckOfTheSeaCard : SeabourneCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _count = 2;

    public LuckOfTheSeaCard() : base(1, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
        AddWet(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var order = new[] { SeabourneGemType.Ruby, SeabourneGemType.Sapphire, SeabourneGemType.Emerald, SeabourneGemType.Amber, SeabourneGemType.Opal, SeabourneGemType.Diamond };
        var granted = 0;
        foreach (var gem in order) { if (Acquire(play, gem)) { granted++; if (granted >= _count) break; } }
    }

    protected override void OnUpgrade()
    {
        _count = 3;
    }
}

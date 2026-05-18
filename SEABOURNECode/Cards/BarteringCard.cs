using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class BarteringCard : SeabourneCard
{
    public BarteringCard() : base(1, CardType.Power, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _goldPerGem = 15;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) { var removed = SeabourneCardRuntime.RemoveAllGems(owner); SeabourneReflection.Invoke(owner, "GainGold", removed * _goldPerGem); }
    }

    protected override void OnUpgrade()
    {
        _goldPerGem = 25;
    }
}

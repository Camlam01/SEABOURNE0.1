using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class VaultCard : SeabourneCard
{
    public VaultCard() : base(1, CardType.Power, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _amount = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) SeabourneState.Gems(owner).SlotCount += _amount;
    }

    protected override void OnUpgrade()
    {
        _amount = 3;
    }
}

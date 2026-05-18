using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class PerfectCatchCard : SeabourneCard
{
    public PerfectCatchCard() : base(2, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _count = 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Reel(choiceContext, play);
        var owner = SeabourneReflection.GetOwner(play);
        var hand = owner is null ? null : SeabourneReflection.GetHand(owner);
        if (hand is not null) { foreach (var cardObj in hand.Cast<object>().OfType<CardModel>().Take(_count)) SeabourneCardRuntime.AddImbued(cardObj, 1); }
    }

    protected override void OnUpgrade()
    {
        _count = 3;
    }
}

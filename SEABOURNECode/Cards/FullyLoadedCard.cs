using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class FullyLoadedCard : SeabourneCard
{
    public FullyLoadedCard() : base(2, CardType.Skill, CardRarity.Uncommon, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Reel(choiceContext, play);
        var owner = SeabourneReflection.GetOwner(play);
        var hand = owner is null ? null : SeabourneReflection.GetHand(owner);
        if (hand is not null) { var cannonballs = hand.Cast<object>().OfType<CardModel>().Where(c => c is CannonballCard or SpinyCannonballCard or GrapeshotCard or RoundshotCard or VimshotCard).ToList(); foreach (var cannonball in cannonballs) await SeabourneCardRuntime.LoadCannon(choiceContext, this, play, cannonball); }
    }

    protected override void OnUpgrade()
    {
        UpgradeCost(1);
    }
}

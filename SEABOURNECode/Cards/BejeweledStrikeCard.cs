using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class BejeweledStrikeCard : SeabourneCard
{
    public BejeweledStrikeCard() : base(1, CardType.Attack, CardRarity.Rare, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(10m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Attack(choiceContext, play, PrimaryDamage);
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null) SeabourneState.Gems(owner).SlotCount += 1;
        if (!Acquire(play, SeabourneGemType.Ruby) && !Acquire(play, SeabourneGemType.Sapphire)) Acquire(play, SeabourneGemType.Emerald);
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(5m);
    }
}

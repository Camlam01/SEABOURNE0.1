using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class CastStrikeCard : SeabourneCard
{
    public CastStrikeCard() : base(1, CardType.Attack, CardRarity.Uncommon, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [ DamageVar(6m) ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        await Cast(choiceContext, play, 3);
        var owner = SeabourneReflection.GetOwner(play);
        var discard = owner is null ? null : SeabourneReflection.GetDiscard(owner);
        var cast = SeabourneReflection.FindPower<CastPower>(owner)?.Amount ?? 1;
        var hookedIndex = discard is null ? -1 : Math.Max(0, discard.Count - cast);
        var hookedCard = discard is not null && discard.Count > 0 && hookedIndex < discard.Count ? discard[hookedIndex] as CardModel : null;
        var hookedEnergy = hookedCard is null ? 0 : SeabourneReflection.GetInt(hookedCard, "CurrentStarCost", "CanonicalStarCost", "BaseStarCost");
        await Attack(choiceContext, play, PrimaryDamage * Math.Max(0, hookedEnergy));
    }

    protected override void OnUpgrade()
    {
        UpgradeDamage(2m);
    }
}

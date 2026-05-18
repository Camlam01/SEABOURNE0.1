using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class GemCannonCard : SeabourneCard
{
    public GemCannonCard() : base(2, CardType.Attack, CardRarity.Rare, AnyEnemyTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _damagePerGem = 20;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var owner = SeabourneReflection.GetOwner(play);
        var gemCount = owner is null ? 0 : SeabourneState.Gems(owner).Owned.Count;
        await Attack(choiceContext, play, _damagePerGem * gemCount);
    }

    protected override void OnUpgrade()
    {
        _damagePerGem = 30;
    }
}

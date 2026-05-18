using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class ArtilleryCard : SeabourneCard
{
    public ArtilleryCard() : base(1, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardTag> Tags => CardTags("Exhaust");

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var pool = new Func<int, Type>(i => (i % 3) switch { 0 => typeof(CannonballCard), 1 => typeof(SpinyCannonballCard), _ => typeof(GrapeshotCard) });
        for (var i = 0; i < 5; i++) { var t = pool(i); if (t == typeof(CannonballCard)) await AddCardCopiesToHand<CannonballCard>(choiceContext, play, 1); else if (t == typeof(SpinyCannonballCard)) await AddCardCopiesToHand<SpinyCannonballCard>(choiceContext, play, 1); else await AddCardCopiesToHand<GrapeshotCard>(choiceContext, play, 1); }
    }

    protected override void OnUpgrade()
    {

    }
}

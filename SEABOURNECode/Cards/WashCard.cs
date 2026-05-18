using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Extensions;
using SEABOURNE.SEABOURNECode.Powers;

namespace SEABOURNE.SEABOURNECode.Cards;

public sealed class WashCard : SeabourneCard
{
    public WashCard() : base(0, CardType.Skill, CardRarity.Rare, SelfTarget)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    private int _bonus = 0;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SeabourneState.ApplyCostAndWetMods(this, play);
        var x = Math.Max(1, Math.Max(play.Resources.EnergySpent, play.Resources.StarsSpent));
        var owner = SeabourneReflection.GetOwner(play);
        var draw = owner is null ? null : SeabourneReflection.GetDraw(owner);
        var discard = owner is null ? null : SeabourneReflection.GetDiscard(owner);
        if (draw is not null && discard is not null) { var count = Math.Min(draw.Count, x + _bonus); for (var i = 0; i < count; i++) { var idx = draw.Count - 1; if (draw[idx] is CardModel c) { SeabourneCardRuntime.AddImbued(c, 1); discard.Add(c); } draw.RemoveAt(idx); } }
    }

    protected override void OnUpgrade()
    {
        _bonus = 1;
    }
}

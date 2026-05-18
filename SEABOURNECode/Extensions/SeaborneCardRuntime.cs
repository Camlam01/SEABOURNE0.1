using System.Collections;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SEABOURNE.SEABOURNECode.Cards;
using SEABOURNE.SEABOURNECode.Powers;
using SEABOURNE.SEABOURNECode.Relics;

namespace SEABOURNE.SEABOURNECode.Extensions;

public static class SeabourneCardRuntime
{
    public static void SetCost(CardModel card, int newCost)
    {
        try
        {
            SeabourneReflection.Invoke(card, "TryModifyStarCost", newCost);
        }
        catch
        {
            try
            {
                SeabourneReflection.Invoke(card, "AddTemporaryStarCost", newCost);
            }
            catch
            {
                SeabourneState.Card(card).TemporaryCostDelta = newCost;
            }
        }
    }

    public static async Task Attack(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, decimal damage, int hitCount = 1)
    {
        var target = SeabourneReflection.GetTarget(play);
        if (target is null)
            return;

        var finalDamage = SeabourneState.ApplyDamageMods(card, play, damage);
        dynamic command = CommonActions.CardAttack(card, target, finalDamage, hitCount);
        await command.Execute();
        NotifyModifierPlay(card, play);
    }

    public static async Task AttackAll(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, decimal damage, int hitCount = 1)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var finalDamage = SeabourneState.ApplyDamageMods(card, play, damage);
        foreach (var enemy in SeabourneReflection.GetEnemies(owner))
        {
            dynamic command = CommonActions.CardAttack(card, enemy, finalDamage, hitCount);
            await command.Execute();
        }

        NotifyModifierPlay(card, play);
    }

    public static async Task GainBlock(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, decimal amount)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var finalAmount = SeabourneState.ApplyBlockMods(card, play, amount);
        await SeabourneState.GainBlock(card, owner, finalAmount);
        NotifyModifierPlay(card, play);
    }

    public static async Task ApplySelf<TPower>(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new()
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var stacks = SeabourneState.ApplyStackMods(card, play, amount);
        await CommonActions.ApplySelf<TPower>(choiceContext, card, stacks, false);
        NotifyModifierPlay(card, play);
    }

    public static async Task ApplyTarget<TPower>(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new()
    {
        var target = SeabourneReflection.GetTarget(play);
        if (target is null)
            return;

        var stacks = SeabourneState.ApplyStackMods(card, play, amount);
        await CommonActions.Apply<TPower>(choiceContext, target, card, stacks, false);
        NotifyModifierPlay(card, play);
    }

    public static async Task ApplyAll<TPower>(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, int amount)
        where TPower : SEABOURNEPower, new()
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var stacks = SeabourneState.ApplyStackMods(card, play, amount);
        foreach (var enemy in SeabourneReflection.GetEnemies(owner))
            await CommonActions.Apply<TPower>(choiceContext, enemy, card, stacks, false);

        NotifyModifierPlay(card, play);
    }

    public static async Task Cast(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play, int amount)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var adjusted = SeabourneState.ApplyCastMods(card, play, amount);
        if (adjusted <= 0)
            return;

        await CommonActions.ApplySelf<CastPower>(choiceContext, card, adjusted, false);

        foreach (var relic in SeabourneState.GetSeabourneRelics(owner))
            await relic.OnCast(choiceContext, adjusted);
    }

    public static async Task Reel(PlayerChoiceContext choiceContext, SeabourneCard card, CardPlay play)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var discard = SeabourneReflection.GetDiscard(owner);
        var hand = SeabourneReflection.GetHand(owner);
        if (discard is null || hand is null || discard.Count == 0)
            return;

        var cast = SeabourneReflection.FindPower<CastPower>(owner);
        var hookDepth = Math.Max(1, cast?.Amount ?? 1);
        var topIndex = discard.Count - 1;
        var hookedIndex = Math.Max(0, discard.Count - hookDepth);
        var reeled = new List<CardModel>();

        for (var index = hookedIndex; index <= topIndex; index++)
        {
            if (discard[index] is CardModel reeledCard)
                reeled.Add(reeledCard);
        }

        for (var i = reeled.Count - 1; i >= 0; i--)
        {
            var reeledCard = reeled[i];
            SeabourneReflection.RemoveFromList(discard, reeledCard);
            var modifiers = SeabourneState.Card(reeledCard);
            var wetStacks = Math.Max(0, modifiers.WetStacks);

            if (wetStacks > 0 && reeledCard is SeabourneCard wetCard)
            {
                for (var repeat = 0; repeat < wetStacks; repeat++)
                    await wetCard.OnReeled(choiceContext, play, wetCard);
            }
            else
            {
                SeabourneReflection.AddToList(hand, reeledCard);
            }

            SeabourneState.Turn(owner).ReeledCardsThisTurn++;

            if (SeabourneReflection.FindPower<FishermansFortitudePower>(owner) is { } fortitude)
                await SeabourneState.GainBlock(card, owner, fortitude.Amount);

            if (SeabourneReflection.FindPower<BarbedHookPower>(owner) is { } hook)
                await CommonActions.ApplySelf<TemporaryStrengthPower>(choiceContext, card, hook.Amount, false);

            if (SeabourneState.Turn(owner).EnchantedRodRemaining > 0)
            {
                SeabourneState.Card(reeledCard).ImbuedStacks += 1;
                SeabourneState.Turn(owner).EnchantedRodRemaining--;
            }
        }

        SeabourneState.Turn(owner).ReelsThisTurn++;

        if (cast is not null)
            SeabourneReflection.Invoke(cast, "SetAmount", 0);
    }

    public static bool AcquireGem(CardPlay play, SeabourneGemType gem)
    {
        var owner = SeabourneReflection.GetOwner(play);
        return owner is not null && SeabourneState.Gems(owner).Acquire(gem);
    }

    public static async Task FireCannon(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var cannon = SeabourneState.Cannon(owner);
        if (cannon.LoadedCards.Count == 0)
        {
            foreach (var enemy in SeabourneReflection.GetEnemies(owner))
            {
                dynamic cmd = CommonActions.CardAttack(source, enemy, 6m, 1);
                await cmd.Execute();
            }

            return;
        }

        var target = SeabourneReflection.GetTarget(play);
        var loaded = cannon.LoadedCards.ToList();
        cannon.LoadedCards.Clear();

        var multiplier = 1m;
        if (SeabourneReflection.FindPower<ExplosiveGunpowderPower>(owner) is not null)
            multiplier *= 2m;

        foreach (var relic in SeabourneState.GetSeabourneRelics(owner))
            relic.ModifyCannonDamage(ref multiplier);

        foreach (var loadedCard in loaded)
        {
            await ResolveCannonball(choiceContext, source, play, loadedCard, target, multiplier);
            cannon.FiredCards.Add(loadedCard);
        }
    }

    public static Task LoadCannon(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play, CardModel loadedCard)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return Task.CompletedTask;

        var hand = SeabourneReflection.GetHand(owner);
        var discard = SeabourneReflection.GetDiscard(owner);
        SeabourneReflection.RemoveFromList(hand, loadedCard);
        SeabourneReflection.RemoveFromList(discard, loadedCard);
        SeabourneState.Cannon(owner).LoadedCards.Add(loadedCard);
        NotifyModifierPlay(source, play);
        return Task.CompletedTask;
    }

    public static async Task AddCardCopiesToHand<TCard>(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play, int count)
        where TCard : CardModel, new()
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var hand = SeabourneReflection.GetHand(owner);
        if (hand is null)
            return;

        for (var i = 0; i < count; i++)
            hand.Add(ModelDb.Card<TCard>().ToMutable());
    }

    public static async Task AddCardCopyToDiscard<TCard>(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play, int count = 1)
        where TCard : CardModel, new()
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var discard = SeabourneReflection.GetDiscard(owner);
        if (discard is null)
            return;

        for (var i = 0; i < count; i++)
            discard.Add(ModelDb.Card<TCard>().ToMutable());
    }

    public static async Task GainEnergy(SeabourneCard source, CardPlay play, int amount)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is not null)
            SeabourneState.GainEnergy(owner, amount);
    }


    public static Task LoadSpecificCard(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play, CardModel loadedCard) =>
        LoadCannon(choiceContext, source, play, loadedCard);

    public static int RemoveAllGems(object owner) => SeabourneState.Gems(owner).RemoveAll();

    public static void AddWet(CardModel card, int amount) => SeabourneState.Card(card).WetStacks += Math.Max(0, amount);
    public static void AddImbued(CardModel card, int amount) => SeabourneState.Card(card).ImbuedStacks += Math.Max(0, amount);
    public static void SetUnimbued(CardModel card, bool value = true) => SeabourneState.Card(card).Unimbued = value;
    public static void RechargeGems(object owner) => SeabourneState.Gems(owner).Recharge();

    private static async Task ResolveCannonball(PlayerChoiceContext choiceContext, SeabourneCard source, CardPlay play, CardModel card, Creature? target, decimal multiplier)
    {
        if (target is null)
            return;

        switch (card)
        {
            case CannonballCard cannonball:
                await Attack(choiceContext, cannonball, play, cannonball.Damage * multiplier);
                break;
            case SpinyCannonballCard spiny:
                await Attack(choiceContext, spiny, play, spiny.Damage * multiplier);
                await CommonActions.Apply<VulnerablePower>(choiceContext, target, spiny, 1, false);
                break;
            case GrapeshotCard grapeshot:
                await Attack(choiceContext, grapeshot, play, grapeshot.Damage * multiplier);
                await CommonActions.Apply<VulnerablePower>(choiceContext, target, grapeshot, 1, false);
                break;
            case RoundshotCard roundshot:
                await Attack(choiceContext, roundshot, play, roundshot.Damage * multiplier);
                break;
            case VimshotCard vimshot:
                await Attack(choiceContext, vimshot, play, vimshot.Damage * multiplier);
                await GainEnergy(vimshot, play, vimshot.EnergyGain);
                break;
            default:
                if (card is SeabourneCard seabourneCard)
                    await Attack(choiceContext, seabourneCard, play, seabourneCard.PrimaryDamage * multiplier);
                break;
        }
    }

    private static void NotifyModifierPlay(SeabourneCard card, CardPlay play)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        if (!SeabourneState.HasAnyModifier(card))
            return;

        if (SeabourneReflection.FindPower<IdolatryPower>(owner) is { } idolatry)
        {
            foreach (var enemy in SeabourneReflection.GetEnemies(owner))
            {
                dynamic cmd = CommonActions.CardAttack(card, enemy, idolatry.Amount, 1);
                _ = cmd.Execute();
            }
        }
    }
}

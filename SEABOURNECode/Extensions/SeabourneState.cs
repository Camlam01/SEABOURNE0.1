using System.Collections;
using System.Runtime.CompilerServices;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using SEABOURNE.SEABOURNECode.Cards;
using SEABOURNE.SEABOURNECode.Powers;
using SEABOURNE.SEABOURNECode.Relics;

namespace SEABOURNE.SEABOURNECode.Extensions;

public enum SeabourneGemType
{
    Ruby,
    Sapphire,
    Emerald,
    Amber,
    Opal,
    Diamond
}

public sealed class SeabourneCardModifiers
{
    public int WetStacks { get; set; }
    public int ImbuedStacks { get; set; }
    public bool Unimbued { get; set; }
    public int TemporaryCostDelta { get; set; }
    public bool ExhaustFromAmber { get; set; }
}

public sealed class SeabourneGemState
{
    public int SlotCount { get; set; } = 2;
    public HashSet<SeabourneGemType> Owned { get; } = [];
    public HashSet<SeabourneGemType> Charged { get; } = [];

    public void Recharge()
    {
        Charged.Clear();
        foreach (var gem in Owned)
            Charged.Add(gem);
    }

    public bool CanAcquire(SeabourneGemType gem) => Owned.Contains(gem) || Owned.Count < SlotCount;

    public bool Acquire(SeabourneGemType gem)
    {
        if (Owned.Contains(gem))
        {
            Charged.Add(gem);
            return true;
        }

        if (Owned.Count >= SlotCount)
            return false;

        Owned.Add(gem);
        Charged.Add(gem);
        return true;
    }

    public int RemoveAll() { var count = Owned.Count; Owned.Clear(); Charged.Clear(); return count; }
}

public sealed class SeabourneCannonState
{
    public List<CardModel> LoadedCards { get; } = [];
    public List<CardModel> FiredCards { get; } = [];
}

public sealed class SeabourneTurnState
{
    public bool GildedTriggered { get; set; }
    public int ReelsThisTurn { get; set; }
    public int ReeledCardsThisTurn { get; set; }
    public int EnchantedRodRemaining { get; set; }
}

public static class SeabourneState
{
    private static readonly ConditionalWeakTable<CardModel, SeabourneCardModifiers> CardData = new();
    private static readonly ConditionalWeakTable<object, SeabourneGemState> GemData = new();
    private static readonly ConditionalWeakTable<object, SeabourneCannonState> CannonData = new();
    private static readonly ConditionalWeakTable<object, SeabourneTurnState> TurnData = new();

    public static SeabourneCardModifiers Card(CardModel card) => CardData.GetOrCreateValue(card);
    public static SeabourneGemState Gems(object owner) => GemData.GetOrCreateValue(owner);
    public static SeabourneCannonState Cannon(object owner) => CannonData.GetOrCreateValue(owner);
    public static SeabourneTurnState Turn(object owner) => TurnData.GetOrCreateValue(owner);

    public static int EffectiveImbuedMultiplier(CardModel card)
    {
        var data = Card(card);
        if (data.Unimbued)
            return 0;

        return 1 + Math.Max(0, data.ImbuedStacks);
    }

    public static bool HasAnyModifier(CardModel card)
    {
        var data = Card(card);
        return data.WetStacks > 0 || data.ImbuedStacks > 0 || data.Unimbued || data.TemporaryCostDelta != 0 || data.ExhaustFromAmber;
    }

    public static decimal ApplyDamageMods(SeabourneCard card, CardPlay play, decimal value)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return value;

        var result = value;
        var data = Card(card);
        var multiplier = EffectiveImbuedMultiplier(card);

        if (Gems(owner).Charged.Contains(SeabourneGemType.Ruby) && value > 0m)
        {
            result += value * 0.2m * multiplier;
            Gems(owner).Charged.Remove(SeabourneGemType.Ruby);
        }

        if (HasPower<ShardyShrapnelPower>(owner, out var shrapnel))
            result += shrapnel.Amount;

        if (result < 0m)
            result = 0m;

        return decimal.Round(result, 0, MidpointRounding.AwayFromZero);
    }

    public static decimal ApplyBlockMods(SeabourneCard card, CardPlay play, decimal value)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return value;

        var result = value;
        var multiplier = EffectiveImbuedMultiplier(card);

        if (Gems(owner).Charged.Contains(SeabourneGemType.Sapphire) && value > 0m)
        {
            result += value * 0.2m * multiplier;
            Gems(owner).Charged.Remove(SeabourneGemType.Sapphire);
        }

        if (result < 0m)
            result = 0m;

        return decimal.Round(result, 0, MidpointRounding.AwayFromZero);
    }

    public static int ApplyStackMods(SeabourneCard card, CardPlay play, int amount)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return amount;

        var result = amount;
        var multiplier = EffectiveImbuedMultiplier(card);

        if (Gems(owner).Charged.Contains(SeabourneGemType.Emerald))
        {
            result += 1 * multiplier;
            Gems(owner).Charged.Remove(SeabourneGemType.Emerald);
        }

        if (HasPower<JinxPower>(owner, out var jinx))
        {
            var bonusPercent = Math.Max(0, jinx.Amount);
            result += (int)Math.Floor(result * (bonusPercent / 100m));
        }

        return Math.Max(0, result);
    }

    public static int ApplyCastMods(SeabourneCard card, CardPlay play, int amount)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return amount;

        var result = amount;
        var multiplier = EffectiveImbuedMultiplier(card);

        if (Gems(owner).Charged.Contains(SeabourneGemType.Opal))
        {
            result += 1 * multiplier;
            Gems(owner).Charged.Remove(SeabourneGemType.Opal);
        }

        if (HasPower<MasterbaitPower>(owner, out var masterbait))
            result += masterbait.Amount;

        return Math.Max(0, result);
    }

    public static void ApplyCostAndWetMods(SeabourneCard card, CardPlay play)
    {
        var owner = SeabourneReflection.GetOwner(play);
        if (owner is null)
            return;

        var data = Card(card);
        var multiplier = EffectiveImbuedMultiplier(card);

        if (Gems(owner).Charged.Contains(SeabourneGemType.Amber))
        {
            data.TemporaryCostDelta -= Math.Max(1, multiplier);
            data.ExhaustFromAmber = true;
            Gems(owner).Charged.Remove(SeabourneGemType.Amber);
        }

        if (Gems(owner).Charged.Contains(SeabourneGemType.Diamond))
        {
            data.WetStacks += Math.Max(1, multiplier);
            Gems(owner).Charged.Remove(SeabourneGemType.Diamond);
        }
    }

    public static bool HasPower<TPower>(object owner, out TPower power) where TPower : class
    {
        power = SeabourneReflection.FindPower<TPower>(owner)!;
        return power is not null;
    }

    public static IEnumerable<SeabourneRelic> GetSeabourneRelics(object owner)
    {
        return SeabourneReflection.GetRelics(owner).OfType<SeabourneRelic>();
    }

    public static async Task GainBlock(object? source, Creature owner, decimal amount)
    {
        try
        {
            await CreatureCmd.GainBlock(owner, amount, ValueProp.Move, null, false);
        }
        catch
        {
            SeabourneReflection.Invoke(owner, "GainBlock", amount);
        }
    }

    public static void GainEnergy(object owner, int amount)
    {
        if (amount <= 0)
            return;

        var playerCmdType = Type.GetType("MegaCrit.Sts2.Core.Commands.PlayerCmd, sts2");
        var gainEnergyMethod = playerCmdType?.GetMethods().FirstOrDefault(m => m.Name == "GainEnergy" && m.GetParameters().Length >= 2);
        if (gainEnergyMethod is not null)
        {
            var args = gainEnergyMethod.GetParameters().Length == 2
                ? new object?[] { owner, amount }
                : [owner, amount, false];
            gainEnergyMethod.Invoke(null, args);
            return;
        }

        SeabourneReflection.Invoke(owner, "GainEnergy", amount);
    }

    public static void ResetTurn(object owner)
    {
        var turn = Turn(owner);
        turn.GildedTriggered = false;
        turn.ReelsThisTurn = 0;
        turn.ReeledCardsThisTurn = 0;
        turn.EnchantedRodRemaining = HasPower<EnchantedRodPower>(owner, out var rod) ? rod.Amount : 0;
        Gems(owner).Recharge();
    }
}

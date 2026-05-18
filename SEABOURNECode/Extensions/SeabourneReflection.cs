using System.Collections;
using System.Reflection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace SEABOURNE.SEABOURNECode.Extensions;

internal static class SeabourneReflection
{
    private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    public static object? GetProp(object? obj, params string[] names)
    {
        if (obj is null)
            return null;

        var type = obj.GetType();
        foreach (var name in names)
        {
            var prop = type.GetProperty(name, Flags);
            if (prop is not null)
                return prop.GetValue(obj);

            var field = type.GetField(name, Flags);
            if (field is not null)
                return field.GetValue(obj);
        }

        return null;
    }

    public static T? GetProp<T>(object? obj, params string[] names)
    {
        var value = GetProp(obj, names);
        return value is T typed ? typed : default;
    }

    public static object? Invoke(object? obj, string name, params object?[] args)
    {
        if (obj is null)
            return null;

        var method = obj.GetType().GetMethods(Flags).FirstOrDefault(m => m.Name == name && m.GetParameters().Length == args.Length);
        return method?.Invoke(obj, args);
    }

    public static Creature? GetOwner(CardModel card)
    {
        return GetProp<Creature>(card, "Owner", "Creature", "Player")
            ?? GetProp<Creature>(GetProp(card, "Owner"), "Creature", "Owner", "Player");
    }

    public static Creature? GetOwner(CardPlay play)
    {
        var card = GetProp<CardModel>(play, "Card", "CardModel");
        return card is null ? null : GetOwner(card);
    }

    public static Creature? GetTarget(CardPlay play)
    {
        return GetProp<Creature>(play, "Target", "PrimaryTarget")
            ?? GetProp<Creature>(GetProp(play, "ChoiceContext"), "Target")
            ?? GetProp<Creature>(play, "SelectedTarget");
    }

    public static IList? GetPile(object? owner, params string[] pileNames)
    {
        var player = owner ?? throw new InvalidOperationException("Owner/player was null.");
        foreach (var pileName in pileNames)
        {
            var pile = GetProp(player, pileName);
            if (pile is IList list)
                return list;

            var cards = GetProp(pile, "Cards", "Items");
            if (cards is IList nestedList)
                return nestedList;
        }

        return null;
    }

    public static IList? GetHand(object owner) => GetPile(owner, "Hand", "HandPile");
    public static IList? GetDiscard(object owner) => GetPile(owner, "DiscardPile", "Discard", "DiscardCards");
    public static IList? GetDraw(object owner) => GetPile(owner, "DrawPile", "Draw", "Deck");

    public static IEnumerable<Creature> GetEnemies(object? owner)
    {
        var combat = GetProp(owner, "Combat", "CombatState", "CurrentCombat");
        var enemies = GetProp(combat, "Enemies", "EnemyCreatures");
        if (enemies is IEnumerable<Creature> typed)
            return typed;

        if (enemies is IEnumerable enumerable)
            return enumerable.OfType<Creature>();

        return Array.Empty<Creature>();
    }

    public static IEnumerable<object> GetRelics(object? owner)
    {
        var relics = GetProp(owner, "Relics", "OwnedRelics");
        return relics is IEnumerable enumerable ? enumerable.Cast<object>() : Array.Empty<object>();
    }

    public static IEnumerable<object> GetPowers(object? owner)
    {
        var powers = GetProp(owner, "Powers", "AllPowers");
        return powers is IEnumerable enumerable ? enumerable.Cast<object>() : Array.Empty<object>();
    }

    public static TPower? FindPower<TPower>(object? owner) where TPower : class
    {
        return GetPowers(owner).OfType<TPower>().FirstOrDefault();
    }

    public static int GetInt(object? obj, params string[] names)
    {
        var value = GetProp(obj, names);
        return value switch
        {
            int i => i,
            decimal d => (int)d,
            float f => (int)f,
            double dbl => (int)dbl,
            long l => (int)l,
            _ => 0
        };
    }

    public static decimal GetDecimal(object? obj, params string[] names)
    {
        var value = GetProp(obj, names);
        return value switch
        {
            decimal d => d,
            int i => i,
            float f => (decimal)f,
            double dbl => (decimal)dbl,
            long l => l,
            _ => 0m
        };
    }

    public static bool HasMethod(object? obj, string name)
    {
        return obj is not null && obj.GetType().GetMethod(name, Flags) is not null;
    }

    public static void RemoveFromList(IList? list, object item)
    {
        if (list is null)
            return;

        if (list.Contains(item))
            list.Remove(item);
    }

    public static void AddToList(IList? list, object item)
    {
        list?.Add(item);
    }
}

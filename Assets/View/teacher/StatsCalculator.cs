using UnityEngine;
using System.Linq;

public static class StatsCalculator
{
    public static bool HasAnyAttempt(ActivityEntry activity)
    {
        foreach (var lvl in activity.value.levels)
            if (lvl.attempts.Count > 0)
                return true;
        return false;
    }

    public static bool IsActivityCompleted(ActivityEntry activity)
    {
        foreach (var lvl in activity.value.levels)
            if (!lvl.CompletedAtLeastOnce())
                return false;
        return true;
    }

    public static float GetGlobalDomain(ActivityEntry activity)
    {
        int hits = 0, mistakes = 0;

        foreach (var lvl in activity.value.levels)
            foreach (var a in lvl.attempts)
            {
                hits += a.hits;
                mistakes += a.mistakes;
            }

        return (hits + mistakes) == 0 ? 0 :
            (float)hits / (hits + mistakes) * 100f;
    }

    public static float GetTotalTime(ActivityEntry activity)
    {
        float t = 0;
        foreach (var lvl in activity.value.levels)
            foreach (var a in lvl.attempts)
                t += a.time;
        return t;
    }

    public static string FormatTime(float s)
    {
        int m = Mathf.FloorToInt(s / 60);
        int sec = Mathf.FloorToInt(s % 60);
        return $"{m}m {sec}s";
    }

    public static float GetBestAttempt(LevelData level)
    {
        return level.attempts.Count == 0 ? 0 :
            level.attempts.Max(a => a.hits);
    }

    public static float GetWorstAttempt(LevelData level)
    {
        return level.attempts.Count == 0 ? 0 :
            level.attempts.Min(a => a.hits);
    }

    public static string GetSpeedImprovement(LevelData level)
    {
        if (level.attempts.Count < 2) return "--";

        float first = level.attempts[0].time;
        float last = level.attempts[^1].time;

        float diff = first - last;
        return diff > 0 ? $"↑ {diff:0.0}s" : $"↓ {Mathf.Abs(diff):0.0}s";
    }
}

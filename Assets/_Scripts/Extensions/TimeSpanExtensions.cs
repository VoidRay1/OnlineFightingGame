using System;

public static class TimeSpanExtensions
{
    public static string GetTelegramLastSeenStringFormat(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds < 60)
        {
            return "Last seen just now";
        }
        if (timeSpan.TotalMinutes < 2)
        {
            return "Last seen 1 minute ago";
        }
        if (timeSpan.TotalMinutes < 60)
        {
            int minutes = (int)Math.Floor(timeSpan.TotalMinutes);
            return $"Last seen {minutes} minutes ago";
        }
        if (timeSpan.TotalHours < 2)
        {
            return "Last seen 1 hour ago";
        }
        if (timeSpan.TotalHours < 24)
        {
            int hours = (int)Math.Floor(timeSpan.TotalHours);
            return $"Last seen {hours} hours ago";
        }
        if (timeSpan.TotalDays < 2)
        {
            return "Last seen 1 day ago";
        }
        if (timeSpan.TotalDays < 365)
        {
            int days = (int)Math.Floor(timeSpan.TotalDays);
            return $"Last seen {days} days ago";
        }
        if (timeSpan.TotalDays < 730)
        {
            return "Last seen 1 year ago";
        }
        return $"Last seen {(int)(Math.Floor(timeSpan.TotalDays / 365))} years ago";
    }
}
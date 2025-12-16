namespace Mediatheque_DAL.Utils;

public static class ValidData
{
    private static HashSet<string> _status = new()
    { 
        "To Watch", 
        "Favorite", 
        "Seen" 
    };

    public static IReadOnlyCollection<string> Status => _status;
}

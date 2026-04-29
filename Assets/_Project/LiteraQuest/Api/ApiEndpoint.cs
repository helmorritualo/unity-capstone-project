public static class ApiEndpoint
{
    public const string StudentLogin = "/student/auth/login";
    public const string StudentLogout = "/student/auth/logout";
    public const string StudentBootstrap = "/student/bootstrap";

    public static string Terms(string subjectSlug)
    {
        return $"/student/subjects/{subjectSlug}/terms";
    }

    public static string Stations(string subjectSlug, int termNumber)
    {
        return $"/student/subjects/{subjectSlug}/terms/{termNumber}/stations";
    }

    public static string StationContent(int stationId)
    {
        return $"/student/stations/{stationId}/content";
    }

    public static string StartStation(int stationId)
    {
        return $"/student/stations/{stationId}/start";
    }

    public static string SubmitChallengeAttempt(int challengeId)
    {
        return $"/student/challenges/{challengeId}/attempts";
    }

    public static string CompleteStation(int stationId)
    {
        return $"/student/stations/{stationId}/complete";
    }

    public static string ProgressSummary(string subjectSlug)
    {
        return $"/student/progress/summary?subject={subjectSlug}";
    }

    public static string Rewards(string subjectSlug)
    {
        return $"/student/rewards?subject={subjectSlug}";
    }
}
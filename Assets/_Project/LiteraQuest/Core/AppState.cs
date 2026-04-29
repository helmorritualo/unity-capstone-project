using UnityEngine;

public sealed class AppState : MonoBehaviour
{
    public string CurrentSceneName { get; private set; }
    public bool IsLoading { get; private set; }

    public StudentDto Student { get; private set; }
    public SchoolYearDto SchoolYear { get; private set; }
    public ClassroomDto Classroom { get; private set; }
    public SubjectDto Subject { get; private set; }
    public TermDto[] Terms { get; private set; }

    public bool HasActiveSession => Student != null && Classroom != null && Subject != null;

    public string GradeSectionDisplayName
    {
        get
        {
            if (Classroom?.grade_section == null)
            {
                return string.Empty;
            }

            return Classroom.grade_section.display_name;
        }
    }

    public void SetCurrentScene(string sceneName)
    {
        CurrentSceneName = sceneName;
    }

    public void SetLoading(bool isLoading)
    {
        IsLoading = isLoading;
    }

    public void SetSession(BootstrapResponse bootstrap)
    {
        Student = bootstrap.student;
        SchoolYear = bootstrap.school_year;
        Classroom = bootstrap.classroom;
        Subject = bootstrap.subject;
        Terms = bootstrap.terms;
    }

    public void ClearSession()
    {
        Student = null;
        SchoolYear = null;
        Classroom = null;
        Subject = null;
        Terms = null;
    }

    public void ResetApp()
    {
        CurrentSceneName = string.Empty;
        IsLoading = false;
        ClearSession();
    }
}
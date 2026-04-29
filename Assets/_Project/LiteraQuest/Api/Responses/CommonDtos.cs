public sealed class SchoolYearDto
{
    public int id;
    public string name;
}

public sealed class ClassroomDto
{
    public int id;
    public string name;
    public GradeSectionDto grade_section;
}

public sealed class GradeSectionDto
{
    public int id;
    public int grade_level;
    public string section_name;
    public string display_name;
}

public sealed class SubjectDto
{
    public string slug;
    public string name;
    public string status;
}

public sealed class TermDto
{
    public int term_number;
    public string title;
    public bool is_available;
}
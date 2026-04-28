public sealed class StudentLoginRequest
{
    public string lrn;
    public string pin;

    public StudentLoginRequest(string lrn, string pin)
    {
        this.lrn = lrn;
        this.pin = pin;
    }
}
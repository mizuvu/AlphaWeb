namespace Infrastructure.Identity.Models;

public class ActiveDirectory
{
    public bool PreferLDAP { get; set; }
    public string Domain { get; set; } = default!;
    public LDAP LDAP { get; set; } = default!;
}

public class LDAP
{
    public string IpServer { get; set; } = default!;
    public int Port { get; set; }
    public string Connection { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string DomainUser { get; set; } = default!;
    public string Password { get; set; } = default!;
}

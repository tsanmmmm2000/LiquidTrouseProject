using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

public class Principal : IPrincipal
{
    private IIdentity identity;

    private Principal() { }
    public Principal(string ticketName, string userId)
	{
        identity = new Identity(ticketName, userId);
	}

    public IIdentity Identity
    {
        get { return identity; }
    }

    public bool IsInRole(string role)
    {
        throw new Exception("The method or operation is not implemented.");
    }
}

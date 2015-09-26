using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace Sinn.SidToUser
{
    class Program
    {
        static void Main(string[] args)
        {
            // domainSid: WindowsIdentity.GetCurrent().User.AccountDomainSid);
            if (args.Length > 0)
            {
                if (args[0].StartsWith("-?") ||
                    args[0].StartsWith("-h") ||
                    args[0].StartsWith("-help") ||
                    args[0].StartsWith("/?") ||
                    args[0].StartsWith("/h") ||
                    args[0].StartsWith("/help"))
                {
                    ShowHelp();
                }
                else if (Enum.IsDefined(typeof(WellKnownSidType), args[0]))
                {
                    try
                    {
                        WellKnownSidType sidType = (WellKnownSidType)Enum.Parse(typeof(WellKnownSidType), args[0], false);

                        SecurityIdentifier sid = null;
                        if (args[0].StartsWith("Account"))
                        {
                            sid = new SecurityIdentifier(sidType, WindowsIdentity.GetCurrent().User.AccountDomainSid);
                        }
                        else
                        {
                            sid = new SecurityIdentifier(sidType, null);
                        }

                        NTAccount NTUser = (NTAccount)sid.Translate(typeof(System.Security.Principal.NTAccount));                        
                        Console.WriteLine("[" + sidType.ToString() + "]");
                        Console.WriteLine("Name=" + NTUser.ToString());
                        Console.WriteLine("Shortname=" + NTUser.ToString().Substring(NTUser.ToString().IndexOf("\\")+1));
                        Console.WriteLine("SID=" + sid.ToString());
                        Console.WriteLine("IsAccountSid=" + sid.IsAccountSid().ToString().ToUpper());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    if (args[0].StartsWith("S-"))
                    {
                        try
                        {
                            SecurityIdentifier sid = new SecurityIdentifier(args[0]);
                            NTAccount NTUser = (NTAccount)sid.Translate(typeof(System.Security.Principal.NTAccount));
                           
                            Console.WriteLine("[" + sid.ToString() + "]");
                            Console.WriteLine("Name=" + NTUser.ToString());
                            Console.WriteLine("Shortname=" + NTUser.ToString().Substring(NTUser.ToString().IndexOf("\\") + 1));
                            Console.WriteLine("SID=" + sid.ToString());
                            Console.WriteLine("IsAccountSid=" + sid.IsAccountSid().ToString().ToUpper());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            NTAccount NTUser = new NTAccount(args[0]);
                            SecurityIdentifier sid = (SecurityIdentifier)NTUser.Translate(typeof(SecurityIdentifier));

                            Console.WriteLine("[" + NTUser.ToString() + "]");
                            Console.WriteLine("Name=" + NTUser.ToString());
                            Console.WriteLine("Shortname=" + NTUser.ToString().Substring(NTUser.ToString().IndexOf("\\") + 1));
                            Console.WriteLine("SID=" + sid.ToString());
                            Console.WriteLine("IsAccountSid=" + sid.IsAccountSid().ToString().ToUpper());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            else
            {
                ShowHelp();
            }
            

            

        }
        static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Usage: SidToUser <SID|AccountName|Wellknown-SID-Type>");
            Console.WriteLine("");
            Console.WriteLine("SID: \t\t\tSpecifiy SID in the form of S-1-5-32-547");
            Console.WriteLine("AccountName:\t\tSpecify the name of a Windows Account");
            Console.WriteLine("Wellknown-SID-Type:\tUse one of the following Wellknown SID Types");            
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("");
            foreach (string name in Enum.GetNames(typeof(WellKnownSidType)))
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("");
            Console.WriteLine("For mor information on Wellknown SID Types, take a look at:");
            Console.WriteLine("http://msdn.microsoft.com/en-us/library/system.security.principal.wellknownsidtype.aspx");
        }
    }
}

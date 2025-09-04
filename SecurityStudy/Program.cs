using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Permissions;
using System.Security;

namespace SecurityStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Demo2();
            //SignatureService.Run();
            SignatureService2.Run();

        }

        static void Demo1()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            var principal = WindowsPrincipal.Current as WindowsPrincipal;
            var identity = principal.Identity as WindowsIdentity;
            Console.WriteLine($"IdentityType: {identity.ToString()}");
            Console.WriteLine($"Name: {identity.Name}");
            Console.WriteLine($"'User'?: {principal.IsInRole(WindowsBuiltInRole.User)}");
            Console.WriteLine($"'Administrator'?: {principal.IsInRole(WindowsBuiltInRole.Administrator)}");

            Console.WriteLine($"Authenticated: {identity.IsAuthenticated}");
            Console.WriteLine($"AuthType: {identity.AuthenticationType}");
            Console.WriteLine($"Anonymous: {identity.IsAnonymous}");
            Console.WriteLine($"Token: {identity.Token}");
        }

        static void Demo2()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            try
            {
                ShowMessage();
            }
            catch(SecurityException ex)
            {
                Console.WriteLine($"Security exception caught ({ex.Message})");
                Console.WriteLine($"The current principal must be in the local Users Group");
            }
        }
        [PrincipalPermission(SecurityAction.Demand,Role ="BUILTIN\\USERS")]
        static void ShowMessage()
        {
            Console.WriteLine("The current principal is logged in locally");
            Console.WriteLine("(member of the local users group)");
        }
    }
}
